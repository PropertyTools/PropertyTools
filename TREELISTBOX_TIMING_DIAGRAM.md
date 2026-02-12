# TreeListBox TabControl Race Condition - Timing Diagram

## Normal Initialization (Works - e.g., in Window)

```
Time →
│
├─ [1] HierarchySourceChanged() called
│  │
│  ├─ [2] ClearItems()
│  │      • itemLevelMap.Clear()
│  │      • All dictionaries cleared
│  │
│  ├─ [3] Add rootNode to dictionaries
│  │      • itemLevelMap[rootNode] = -1  ✓
│  │      • isExpandedMap[rootNode] = true
│  │
│  ├─ [4] SubscribeForCollectionChanges()
│  │      • Subscribes to collection events
│  │      • No events fire yet (collection stable)
│  │
│  └─ [5] foreach item: AddItem()
│         │
│         └─ InsertItem() called
│            • Accesses itemLevelMap[rootNode]  ✓ (exists, added at step 3)
│            • itemLevelMap[item] = itemLevelMap[parent] + 1
│            • Success! ✓
│
└─ [6] Initialization complete ✓
```

---

## Race Condition (Crashes - TabControl scenario)

```
Time →
│
├─ [1] HierarchySourceChanged() called (TabControl activates tab)
│  │
│  ├─ [2] ClearItems()
│  │      • itemLevelMap.Clear()  
│  │      • rootNode REMOVED from all dictionaries
│  │
│  ├─ [3] Add rootNode to dictionaries
│  │      • itemLevelMap[rootNode] = -1  ✓
│  │      • isExpandedMap[rootNode] = true
│  │
│  ├─ [4] SubscribeForCollectionChanges()
│  │      • Subscribes to collection events
│  │      │
│  │      ╰─ 🔥 ASYNCHRONOUS EVENT FIRES (TabControl binding/virtualization)
│  │         │
│  │         └─> ChildCollectionChanged() [PARALLEL EXECUTION]
│  │             │
│  │             └─> InsertItems() 
│  │                 │
│  │                 └─> InsertItem(item, parent=rootNode)
│  │                     │
│  │                     ├─ Tries: itemLevelMap[rootNode]
│  │                     │
│  │                     └─ 💥 CRASH! KeyNotFoundException
│  │                        (rootNode might not be in map yet, 
│  │                         or was just cleared, or being cleared again)
│  │
│  └─ [5] foreach item: AddItem() 
│         ↑
│         └─ Never reached OR executing concurrently with event above
│
└─ [6] 💥 Exception thrown
```

---

## Race Condition - Alternative Scenario

```
Time →
│
├─ [1] First HierarchySourceChanged() called
│  │
│  ├─ [2] ClearItems()
│  │      • itemLevelMap[rootNode] removed
│  │
│  ├─ [3] itemLevelMap[rootNode] = -1  ✓
│  │
│  ├─ [4] SubscribeForCollectionChanges()
│  │
│  └─ [5] Starting foreach loop...
│         └─ AddItem(item[0]) in progress...
│
├─ [6] Second HierarchySourceChanged() called! 
│  │   (TabControl virtualization causes re-binding)
│  │
│  ├─ [7] ClearItems() AGAIN
│  │      • itemLevelMap.Clear()
│  │      • rootNode REMOVED AGAIN! ✗
│  │
│  └─ [8] Meanwhile, first foreach loop still running...
│         │
│         └─> AddItem(item[1])
│             │
│             └─> InsertItem(item[1], parent=rootNode)
│                 │
│                 └─ 💥 CRASH! rootNode not in itemLevelMap
│                    (was removed by second ClearItems() call)
```

---

## Why TabControl Causes This

### TabControl Behavior
```
┌─────────────────────────────────────────────┐
│ TabControl                                   │
│                                             │
│ ┌─────────┐  ┌─────────────────────────┐  │
│ │  Tab 1  │  │  Tab 2 (TreeListBox)   │  │
│ │ (Active)│  │  (Not Loaded Yet)       │  │
│ └─────────┘  └─────────────────────────┘  │
│      │                                      │
│      ├─ Fully initialized                  │
│      └─ Visual tree connected              │
└─────────────────────────────────────────────┘

User clicks Tab 2 →

┌─────────────────────────────────────────────┐
│ TabControl                                   │
│                                             │
│ ┌─────────┐  ┌─────────────────────────────┐
│ │  Tab 1  │  │  Tab 2 (TreeListBox)       │
│ └─────────┘  │  (LOADING...)              │
│              │                             │
│              │  Multiple events fire:      │
│              │  • Loaded                   │
│              │  • DataContext changed      │
│              │  • Bindings activated       │
│              │  • HierarchySource set      │
│              │  • Collection events        │
│              │  • Measure/Arrange          │
│              │  • ItemContainerGenerator   │
│              │                             │
│              │  ⚠️ Order not guaranteed!   │
│              └─────────────────────────────┘
└─────────────────────────────────────────────┘

All of these can trigger HierarchySourceChanged():
• Initial binding resolution
• DataContext inheritance
• Dependency property coercion
• Template application
• Visual tree connection
```

---

## Dictionary State During Race Condition

### Before ClearItems()
```
itemLevelMap = {
    rootNode: -1,
    item1: 0,
    item2: 0,
    item3: 1,
    ...
}
```

### During ClearItems() (Step 2)
```
itemLevelMap = {}  ← EMPTY!
```

### After rootNode Added (Step 3)
```
itemLevelMap = {
    rootNode: -1  ← Only entry
}
```

### If Event Fires Before Step 5
```
Event: ChildCollectionChanged()
 ↓
InsertItem(newItem, parent=rootNode)
 ↓
Tries: itemLevelMap[rootNode] + 1
 ↓
Depends on timing:
 • If between step 2-3: KeyNotFoundException (rootNode not in map)
 • If during step 2: KeyNotFoundException (dictionary being modified)
 • If second ClearItems() called: KeyNotFoundException (rootNode removed again)
```

---

## Fix Strategy Comparison

### Option 1: Defensive Check
```csharp
if (!itemLevelMap.ContainsKey(parent)) {
    return; // or throw
}
```
**Pros:** Simple, quick fix  
**Cons:** Masks the root cause, items may be silently skipped

### Option 2: Preserve rootNode
```csharp
private void ClearItems() {
    // ... clear everything ...
    
    // Immediately reinitialize rootNode
    itemLevelMap[rootNode] = -1;
    isExpandedMap[rootNode] = true;
}
```
**Pros:** Ensures rootNode always exists  
**Cons:** Doesn't prevent concurrent access issues

### Option 3: Defer Subscriptions (RECOMMENDED)
```csharp
// Before:
SubscribeForCollectionChanges();    // Line 538
foreach (item in source) {          // Lines 540-543
    AddItem(item);
}

// After:
foreach (item in source) {          // Do this FIRST
    AddItem(item);
}
SubscribeForCollectionChanges();    // THEN subscribe
```
**Pros:** Prevents race condition at the source  
**Cons:** Slightly delays event subscription

### Option 4: Synchronization Lock
```csharp
private readonly object syncLock = new object();

private void HierarchySourceChanged(...) {
    lock (syncLock) {
        // ... all logic ...
    }
}

private void ChildCollectionChanged(...) {
    lock (syncLock) {
        // ... all logic ...
    }
}
```
**Pros:** Thread-safe, prevents all races  
**Cons:** Performance overhead, WPF threading concerns

---

## Recommended Solution: Combination Approach

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
    
    // FIX 1: Ensure rootNode is always present
    this.itemLevelMap[this.rootNode] = -1;
    this.isExpandedMap[this.rootNode] = true;
}

private void HierarchySourceChanged(DependencyPropertyChangedEventArgs e)
{
    // ... handle old value ...
    
    this.ClearItems();
    
    var hierarchySource = this.HierarchySource as IList ?? this.HierarchySource?.Cast<object>().ToList();
    if (hierarchySource != null)
    {
        this.childrenToItemMap.Add(hierarchySource, this.rootNode);
        this.itemToChildrenMap.Add(this.rootNode, hierarchySource);
        this.itemLevelMap[this.rootNode] = -1;
        this.isExpandedMap[this.rootNode] = true;
        
        // FIX 2: Add all items BEFORE subscribing to changes
        foreach (var item in hierarchySource)
        {
            this.AddItem(item);
        }
        
        // Subscribe AFTER initial population (moved from before loop)
        this.SubscribeForCollectionChanges(hierarchySource);
    }
}

private void InsertItem(int index, object item, object parent)
{
    if (item == null)
    {
        throw new ArgumentNullException(nameof(item));
    }
    
    // FIX 3: Defensive check (belt and suspenders)
    if (!this.itemLevelMap.ContainsKey(parent))
    {
        // This should not happen with fixes 1 & 2, but just in case...
        throw new InvalidOperationException(
            $"Parent item not found in level map. This may indicate a timing issue. Parent: {parent}");
    }
    
    // ... rest of method ...
}
```

This combines:
1. ✅ Ensures rootNode always exists (prevents the error)
2. ✅ Defers subscriptions (prevents the race condition)
3. ✅ Adds defensive check (catches unexpected edge cases)
4. ✅ Maintains backward compatibility
5. ✅ Minimal performance impact

---

## Testing the Fix

### Test Cases

1. **Basic TabControl**
   ```xaml
   <TabControl>
       <TabItem Header="Other">...</TabItem>
       <TabItem Header="Tree">
           <TreeListBox HierarchySource="{Binding Items}"/>
       </TabItem>
   </TabControl>
   ```

2. **Rapid Tab Switching**
   ```csharp
   for (int i = 0; i < 100; i++) {
       tabControl.SelectedIndex = 0;
       await Task.Delay(10);
       tabControl.SelectedIndex = 1;
       await Task.Delay(10);
   }
   ```

3. **Dynamic Collection**
   ```csharp
   var items = new ObservableCollection<Node>();
   treeListBox.HierarchySource = items;
   
   // Rapid adds while in non-active tab
   for (int i = 0; i < 1000; i++) {
       items.Add(new Node());
   }
   
   // Then switch to tab
   tabControl.SelectedIndex = 1;
   ```

4. **Nested TabControls**
   ```xaml
   <TabControl>
       <TabItem>
           <TabControl>
               <TabItem>
                   <TreeListBox .../>
               </TabItem>
           </TabControl>
       </TabItem>
   </TabControl>
   ```
