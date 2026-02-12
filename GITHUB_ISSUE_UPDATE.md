# GitHub Issue #312 - Ready for Update

## Instructions for Updating Issue #312

This document contains the exact content to update GitHub issue #312 with comprehensive analysis and reproduction steps.

---

## 1. Issue Description Update

**Replace the existing issue description with the following:**

---

# TreeListBox Crash When Used in TabControl

## Problem

The `TreeListBox` control throws a `NullReferenceException` when placed inside a `TabControl` and the user switches from an empty tab to a tab containing the TreeListBox with bound items.

**Original Reporter:** @Pedrodeo  
**Exception Location:** `TreeListBox.cs`, line 760 in `InsertItem()` method  
**Exception Type:** `NullReferenceException` / `KeyNotFoundException`

## Technical Summary

The crash is caused by a **race condition** during TreeListBox initialization when the control is in a non-visible TabControl tab.

### Root Cause

**File:** `/Source/PropertyTools.Wpf/TreeListBox/TreeListBox.cs`  
**Method:** `InsertItem(int index, object item, object parent)`  
**Line:** 760

The problematic line:
```csharp
this.itemLevelMap[item] = this.itemLevelMap[parent] + 1;
```

**What happens:**
1. When `HierarchySourceChanged()` is called, `ClearItems()` removes all entries from `itemLevelMap` (line 701)
2. `rootNode` is re-added to `itemLevelMap` at line 535
3. Collection change subscriptions are activated at line 538
4. During the item insertion loop (lines 540-543), collection change events can fire
5. These events call `InsertItem()` expecting `parent` (rootNode) to be in the dictionary
6. **CRASH:** If events fire before rootNode initialization completes, `itemLevelMap[parent]` throws exception

### Why TabControl Triggers This

TabControl uses **virtualization and deferred loading** for non-visible tabs:
- Controls aren't fully initialized until the tab is first selected
- Causes asynchronous property changes during tab switching
- Unpredictable WPF binding resolution order
- User observation confirms: **"only happens when the tab has never been opened before"**

## Reproduction Steps

### Quick Reproduction

1. **Modify an existing TreeListBox example window** (e.g., `SingleRootWindow.xaml`):
   ```xml
   <TabControl>
       <TabItem Header="Empty Tab">
           <TextBlock Text="Empty" />
       </TabItem>
       <TabItem Header="TreeListBox Tab">
           <pt:TreeListBox 
               x:Name="tree1" 
               Indentation="12" 
               HierarchySource="{Binding Root}"
               BorderThickness="0"/>
       </TabItem>
   </TabControl>
   ```

2. **Run the application**
3. **Application starts on "Empty Tab"**
4. **Click "TreeListBox Tab"** to switch tabs
5. **CRASH:** `NullReferenceException` at line 760

### Expected Error Message

```
System.Collections.Generic.KeyNotFoundException: 
The given key was not present in the dictionary.
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at PropertyTools.Wpf.TreeListBox.InsertItem(Int32 index, Object item, Object parent) 
      in TreeListBox.cs:line 760
```

## Recommended Fix

Three minimal changes to `TreeListBox.cs` (total ~10 lines):

### Fix #1: Ensure rootNode Always Present After Clearing

**Method:** `ClearItems()`  
**Change:** Immediately re-initialize rootNode after clearing dictionaries

```csharp
private void ClearItems()
{
    // ... existing cleanup code ...
    
    this.itemToParentMap.Clear();
    this.itemToChildrenMap.Clear();
    this.childrenToItemMap.Clear();
    this.itemLevelMap.Clear();
    this.isExpandedMap.Clear();
    
    // ADD: Prevent race condition by ensuring rootNode is always present
    this.itemLevelMap[this.rootNode] = -1;
    this.isExpandedMap[this.rootNode] = true;
}
```

### Fix #2: Defer Collection Change Subscriptions

**Method:** `HierarchySourceChanged()`  
**Change:** Subscribe to collection changes AFTER adding all items

```csharp
private void HierarchySourceChanged(DependencyPropertyChangedEventArgs e)
{
    // ... existing cleanup code ...
    
    this.ClearItems();
    
    var hierarchySource = this.HierarchySource as IList ?? this.HierarchySource?.Cast<object>().ToList();
    if (hierarchySource != null)
    {
        this.childrenToItemMap.Add(hierarchySource, this.rootNode);
        this.itemToChildrenMap.Add(this.rootNode, hierarchySource);
        this.itemLevelMap[this.rootNode] = -1;
        this.isExpandedMap[this.rootNode] = true;
        
        // REMOVE THIS LINE: this.SubscribeForCollectionChanges(hierarchySource);
        
        // Add all items first
        foreach (var item in hierarchySource)
        {
            this.AddItem(item);
        }
        
        // ADD: Subscribe AFTER items are added to prevent race condition
        this.SubscribeForCollectionChanges(hierarchySource);
    }
}
```

### Fix #3: Add Defensive Check

**Method:** `InsertItem()`  
**Change:** Validate parent exists before accessing

```csharp
private void InsertItem(int index, object item, object parent)
{
    if (item == null)
    {
        throw new ArgumentNullException(nameof(item));
    }
    
    // ADD: Defensive check for race condition
    if (!this.itemLevelMap.ContainsKey(parent))
    {
        throw new InvalidOperationException(
            $"Parent item not found in level map. This indicates a race condition " +
            $"during control initialization. Parent: {parent}");
    }
    
    // ... rest of existing code ...
}
```

## Impact Assessment

- **Severity:** High - Blocks TabControl usage scenarios
- **Complexity:** Low - 3 small changes totaling ~10 lines
- **Breaking Changes:** None - all changes are internal to TreeListBox
- **Public API:** No changes
- **Risk:** Low - changes are localized and well-understood

## Testing Requirements

After implementing the fix, verify:
- ✅ TreeListBox in direct Window placement (existing behavior)
- ✅ TreeListBox in TabControl with initial tab switch
- ✅ TreeListBox in TabControl with rapid tab switching  
- ✅ TreeListBox with dynamic collection changes during tab switches
- ✅ Multiple TreeListBox instances in different tabs

## Related Information

- **Similar Issue:** #282 (Different problem - multiple root items, already fixed)
- **Evidence of Known Pattern:** Lines 766-779 already catch ArgumentException for timing issues
- **TabControl Usage:** AddRemoveDemo already uses TreeListBox in TabControl (MainWindow.xaml)

## Comprehensive Documentation

Full technical analysis available in PR branch `copilot/fix-treelistbox-crash`:
- `REFINED_ISSUE_DESCRIPTION.md` - This document in extended form
- `TREELISTBOX_TABCONTROL_BUG_ANALYSIS.md` - Deep technical dive
- `TREELISTBOX_TIMING_DIAGRAM.md` - Visual timing diagrams  
- `TREELISTBOX_FIX_SUMMARY.md` - Quick reference
- `TREELISTBOX_TEST_CASES.cs` - 5 comprehensive test scenarios

---

## 2. Label to Add

Add label: **`ai-ready`**

This label indicates:
- Issue has been fully analyzed
- Root cause identified with specific code locations
- Reproduction steps documented
- Fix recommendations provided with code examples
- Ready for implementation by AI coding agent or human developer

---

## 3. Comment to Post (Optional)

**Title:** Comprehensive Analysis Complete

**Body:**
```markdown
I've completed a comprehensive analysis of this TreeListBox crash issue. 

### Quick Summary

**Root Cause:** Race condition at line 760 - `itemLevelMap[parent]` accessed when parent (rootNode) has been cleared but not yet re-added during TabControl's deferred loading.

**Why TabControl:** Deferred loading of non-visible tabs causes asynchronous initialization and unpredictable event firing order.

**Fix:** 3 minimal changes (~10 lines total):
1. Ensure rootNode always present after clearing dictionaries
2. Defer collection change subscriptions until after items added  
3. Add defensive check with clear error message

### Documentation Created

Full technical documentation in PR branch `copilot/fix-treelistbox-crash`:
- **REFINED_ISSUE_DESCRIPTION.md** - Complete technical analysis (12KB)
- **TREELISTBOX_TABCONTROL_BUG_ANALYSIS.md** - Deep dive (11KB)
- **TREELISTBOX_TIMING_DIAGRAM.md** - Visual diagrams (10KB)
- **TREELISTBOX_FIX_SUMMARY.md** - Quick reference (6KB)
- **TREELISTBOX_TEST_CASES.cs** - 5 test scenarios (16KB)

All documents include specific file paths, line numbers, and ready-to-run code.

### Impact

- Low complexity implementation
- No breaking changes
- No public API changes
- High confidence fix based on thorough analysis

This issue is now **ready for implementation** with complete context and recommended solution.
```

---

## Summary

To update issue #312:

1. ✅ **Replace issue description** with content from section 1 above
2. ✅ **Add `ai-ready` label** to indicate readiness for implementation  
3. ✅ **Post analysis comment** (optional) from section 3 above

All supporting documentation is available in the PR branch for detailed reference.
