# TreeListBox TabControl Crash - Root Cause Analysis

## Issue Summary
TreeListBox crashes with `NullReferenceException` at line 760 when used inside a TabControl (specifically when switching to a tab containing the control).

**Exception Location:** `/Source/PropertyTools.Wpf/TreeListBox/TreeListBox.cs:760`  
**Failing Line:** `this.itemLevelMap[item] = this.itemLevelMap[parent] + 1;`  
**Error:** `parent` (which is `this.rootNode`) is not found in the `itemLevelMap` dictionary

---

## Root Cause

### The Race Condition

The bug is a **race condition** in the initialization sequence, exacerbated by TabControl's virtualization and deferred loading behavior.

### Critical Code Flow

1. **HierarchySourceChanged() is triggered** (line 508-545)
   ```csharp
   private void HierarchySourceChanged(DependencyPropertyChangedEventArgs e)
   {
       // ... handle old value ...
       
       this.ClearItems();  // Line 528 - CLEARS ALL DICTIONARIES
       
       var hierarchySource = this.HierarchySource as IList ?? this.HierarchySource?.Cast<object>().ToList();
       if (hierarchySource != null)
       {
           this.childrenToItemMap.Add(hierarchySource, this.rootNode);
           this.itemToChildrenMap.Add(this.rootNode, hierarchySource);
           this.itemLevelMap[this.rootNode] = -1;  // Line 535 - Adds rootNode
           this.isExpandedMap[this.rootNode] = true;
           
           this.SubscribeForCollectionChanges(hierarchySource);  // Line 538
           
           foreach (var item in hierarchySource)
           {
               this.AddItem(item);  // Line 542
           }
       }
   }
   ```

2. **The Vulnerability Window**
   - **Line 528:** `ClearItems()` removes ALL entries from `itemLevelMap`, including `rootNode`
   - **Line 535:** `rootNode` is re-added to `itemLevelMap`
   - **Line 538:** Subscribes to collection change events
   - **Line 540-543:** Iterates and adds items

3. **The Race:**
   - If `SubscribeForCollectionChanges(hierarchySource)` at line 538 triggers immediate events (possible with some ObservableCollection implementations)
   - OR if external modifications to `hierarchySource` occur during the iteration (lines 540-543)
   - OR if TabControl's virtualization causes multiple rapid initialization cycles
   - THEN `ChildCollectionChanged()` can be called before rootNode is properly registered

4. **ChildCollectionChanged() → InsertItems() → InsertItem()**
   ```csharp
   private void InsertItem(int index, object item, object parent)
   {
       // ... validation ...
       
       this.itemToParentMap[item] = parent;
       var children = this.GetChildrenCollectionByReflection(item);
       this.itemToChildrenMap[item] = children;
       this.childrenToItemMap[children] = item;
       this.SubscribeForCollectionChanges(children);
       
       this.itemLevelMap[item] = this.itemLevelMap[parent] + 1;  // LINE 760 - CRASH!
       // ↑ Assumes parent (rootNode) is in itemLevelMap
   }
   ```

---

## Why TabControl Triggers This

### TabControl-Specific Behaviors

1. **Deferred Loading/Virtualization**
   - Controls in non-selected tabs may not be fully initialized immediately
   - Visual tree connections happen asynchronously
   - DataContext and bindings resolve in unexpected order

2. **Multiple Initialization Cycles**
   - Switching between tabs can cause controls to reload
   - Property values (like HierarchySource) may be re-applied
   - Multiple calls to `HierarchySourceChanged()` can occur rapidly

3. **Binding Resolution Timing**
   - When you switch TO a tab with TreeListBox:
     - The control becomes visible
     - Bindings activate
     - `HierarchySource` may be set/reset
     - Collection change subscriptions activate
   - All of these can happen in quick succession or even concurrently

4. **Visual Tree State**
   - The control might be constructed but not "Loaded"
   - ItemContainerGenerator might not be ready
   - This affects how `Items.Insert()` behaves at line 764

### Why It Works in a Plain Window

- Control is loaded synchronously during window initialization
- Visual tree is fully connected before bindings resolve
- HierarchySource is typically set once during load
- No virtualization or deferred loading complications

---

## Evidence in the Code

### 1. ClearItems() is Too Aggressive
```csharp
private void ClearItems()
{
    foreach (var children in this.childrenToItemMap.Keys)
    {
        this.UnsubscribeCollectionChanges(children);
    }
    
    this.Items.Clear();
    this.itemToParentMap.Clear();
    this.itemToChildrenMap.Clear();
    this.childrenToItemMap.Clear();
    this.itemLevelMap.Clear();        // ← Removes rootNode
    this.isExpandedMap.Clear();
}
```

### 2. Existing Awareness of Timing Issues
The code already catches timing-related exceptions at lines 766-779:
```csharp
try
{
    this.Items.Insert(index, item);
}
catch (ArgumentException e)
{
    if (e.TargetSite?.Name == "set_Height")
    {
        return;  // Known timing issue
    }
    
    if (e.TargetSite?.Name == "ExtendViewport")
    {
        return;  // Known timing issue
    }
    
    throw;
}
```

This shows the developers were already aware of similar timing/initialization issues.

### 3. No Defensive Checks
Line 760 assumes `parent` is always in `itemLevelMap`:
```csharp
this.itemLevelMap[item] = this.itemLevelMap[parent] + 1;
// No check: if (!this.itemLevelMap.ContainsKey(parent)) ...
```

---

## Proposed Solutions

### Solution 1: Defensive Check (Quick Fix)
Add a safety check before accessing the dictionary:

```csharp
private void InsertItem(int index, object item, object parent)
{
    if (item == null)
    {
        throw new ArgumentNullException(nameof(item));
    }

    // Add defensive check
    if (!this.itemLevelMap.ContainsKey(parent))
    {
        // Parent not in map - likely due to timing issue
        // This can happen in TabControl scenarios during initialization
        return; // or throw an appropriate exception
    }

    // ... rest of method ...
    this.itemLevelMap[item] = this.itemLevelMap[parent] + 1;
    // ...
}
```

### Solution 2: Ensure rootNode is Always Present (Better Fix)
Modify `ClearItems()` to preserve rootNode or re-initialize it immediately:

```csharp
private void ClearItems()
{
    foreach (var children in this.childrenToItemMap.Keys)
    {
        this.UnsubscribeCollectionChanges(children);
    }
    
    this.Items.Clear();
    this.itemToParentMap.Clear();
    this.itemToChildrenMap.Clear();
    this.childrenToItemMap.Clear();
    this.itemLevelMap.Clear();
    this.isExpandedMap.Clear();
    
    // Immediately reinitialize rootNode to prevent race conditions
    this.itemLevelMap[this.rootNode] = -1;
    this.isExpandedMap[this.rootNode] = true;
}
```

### Solution 3: Defer Collection Subscriptions (Most Robust)
Subscribe to collection changes AFTER all items are added:

```csharp
private void HierarchySourceChanged(DependencyPropertyChangedEventArgs e)
{
    var oldTreeSource = e.OldValue as IEnumerable;
    if (oldTreeSource != null)
    {
        // ... handle old value ...
    }
    
    this.ClearItems();
    
    var hierarchySource = this.HierarchySource as IList ?? this.HierarchySource?.Cast<object>().ToList();
    if (hierarchySource != null)
    {
        this.childrenToItemMap.Add(hierarchySource, this.rootNode);
        this.itemToChildrenMap.Add(this.rootNode, hierarchySource);
        this.itemLevelMap[this.rootNode] = -1;
        this.isExpandedMap[this.rootNode] = true;
        
        // Add all items FIRST
        foreach (var item in hierarchySource)
        {
            this.AddItem(item);
        }
        
        // THEN subscribe to changes (moved after loop)
        this.SubscribeForCollectionChanges(hierarchySource);
    }
}
```

### Solution 4: Use Loaded Event (Alternative)
Defer HierarchySource processing until the control is fully loaded:

```csharp
public TreeListBox()
{
    this.Loaded += OnLoaded;
}

private bool isLoaded = false;
private IEnumerable pendingHierarchySource = null;

private void OnLoaded(object sender, RoutedEventArgs e)
{
    this.isLoaded = true;
    if (this.pendingHierarchySource != null)
    {
        ApplyHierarchySource(this.pendingHierarchySource);
        this.pendingHierarchySource = null;
    }
}

private void HierarchySourceChanged(DependencyPropertyChangedEventArgs e)
{
    if (!this.isLoaded)
    {
        this.pendingHierarchySource = this.HierarchySource;
        return;
    }
    
    ApplyHierarchySource(this.HierarchySource);
}

private void ApplyHierarchySource(IEnumerable hierarchySource)
{
    // ... existing logic ...
}
```

---

## Recommended Fix

**I recommend Solution 2 + Solution 3 combined:**

1. **Ensure rootNode is always in dictionaries** (makes the code more robust)
2. **Defer collection change subscriptions** (prevents race conditions)

This combination:
- Fixes the immediate crash
- Prevents future similar issues
- Maintains backward compatibility
- Minimal performance impact
- Cleaner than adding checks everywhere

---

## Additional Observations

### Potential Related Issues

1. **Line 752-755:** Similar risk with `childrenToItemMap`
   ```csharp
   if (this.childrenToItemMap.ContainsKey(children))
   {
       throw new InvalidOperationException("Children collection already observed.");
   }
   ```
   This could also be hit in TabControl scenarios with rapid init/deinit cycles.

2. **Line 451:** Another location that accesses `itemLevelMap[item]`
   ```csharp
   container.Level = this.itemLevelMap[item];
   ```
   Should also have defensive checks.

3. **PrepareContainerForItemOverride** (line 429-467):
   The DEBUG assertion at lines 444-447 could catch this issue in debug builds:
   ```csharp
   #if DEBUG
   if (!this.itemToParentMap.ContainsKey(item))
   {
       throw new InvalidOperationException($"Missing parent for item {item}");
   }
   #endif
   ```

---

## Reproduction Steps

To reproduce this issue:

1. Create a TabControl with at least 2 tabs
2. Place TreeListBox in the **second tab** (or any non-default tab)
3. Bind TreeListBox.HierarchySource to an ObservableCollection
4. Ensure the collection is populated **before** the window loads OR
5. Use a fast-updating collection that fires change events rapidly
6. Switch from the first tab to the tab containing TreeListBox
7. **Crash occurs** during tab switch

### Minimal Repro XAML
```xaml
<TabControl>
    <TabItem Header="Tab 1">
        <TextBlock>First Tab</TextBlock>
    </TabItem>
    <TabItem Header="Tab 2">
        <pt:TreeListBox HierarchySource="{Binding Items}" 
                        ChildrenPath="Children"
                        IsExpandedPath="IsExpanded"/>
    </TabItem>
</TabControl>
```

---

## Testing Recommendations

After implementing the fix:

1. Test with TreeListBox in TabControl (multiple tabs)
2. Test rapid tab switching
3. Test with pre-populated collections
4. Test with collections that change during initialization
5. Test with nested TreeListBox controls
6. Test with virtualization enabled/disabled
7. Test in both Debug and Release builds

---

## References

- TreeListBox.cs lines: 508-545 (HierarchySourceChanged)
- TreeListBox.cs lines: 690-703 (ClearItems)
- TreeListBox.cs lines: 733-780 (InsertItem)
- TreeListBox.cs lines: 211-254 (ChildCollectionChanged)
- Example: AddRemoveDemo/MainWindow.xaml (lines 35-42, shows TabControl usage)
