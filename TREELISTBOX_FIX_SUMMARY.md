# TreeListBox TabControl Crash - Quick Reference

## The Problem
**TreeListBox crashes when used in a TabControl** with `NullReferenceException` at line 760:
```csharp
this.itemLevelMap[item] = this.itemLevelMap[parent] + 1;
```

## Root Cause (TL;DR)
**Race condition** between `ClearItems()` and collection change events, triggered by TabControl's deferred loading/virtualization.

## What Happens

1. TabControl activates a tab containing TreeListBox
2. `HierarchySourceChanged()` is called
3. `ClearItems()` removes `rootNode` from `itemLevelMap`
4. `rootNode` is re-added to `itemLevelMap`
5. `SubscribeForCollectionChanges()` is called
6. **🔥 Collection change event fires immediately (or `HierarchySourceChanged` is called again)**
7. `InsertItem()` tries to access `itemLevelMap[rootNode]`
8. **💥 Crash: `rootNode` not found** (removed by concurrent `ClearItems()` or event fired before re-add)

## Why TabControl?

TabControl uses **virtualization** and **deferred loading**:
- Controls in inactive tabs aren't fully initialized
- Switching tabs triggers multiple rapid property changes
- Bindings resolve asynchronously
- Multiple initialization cycles can occur
- Events fire in unpredictable order

## The Fix (Recommended)

Apply **three changes** to TreeListBox.cs:

### Change 1: Make ClearItems() safer (line 690)
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
    
    // ADD THESE TWO LINES:
    this.itemLevelMap[this.rootNode] = -1;
    this.isExpandedMap[this.rootNode] = true;
}
```

### Change 2: Defer collection subscriptions (line 508)
```csharp
private void HierarchySourceChanged(DependencyPropertyChangedEventArgs e)
{
    var oldTreeSource = e.OldValue as IEnumerable;
    if (oldTreeSource != null)
    {
        foreach (var item in oldTreeSource)
        {
            var container = this.GetContainerFromItem(item);
            if (container == null)
            {
                continue;
            }
            
            if (container.IsSelected)
            {
                this.SelectedItems.Remove(item);
            }
        }
    }
    
    this.ClearItems();
    
    var hierarchySource = this.HierarchySource as IList ?? this.HierarchySource?.Cast<object>().ToList();
    if (hierarchySource != null)
    {
        this.childrenToItemMap.Add(hierarchySource, this.rootNode);
        this.itemToChildrenMap.Add(this.rootNode, hierarchySource);
        this.itemLevelMap[this.rootNode] = -1;
        this.isExpandedMap[this.rootNode] = true;
        
        // MOVE THIS LINE FROM HERE:
        // this.SubscribeForCollectionChanges(hierarchySource);
        
        // Add all items FIRST
        foreach (var item in hierarchySource)
        {
            this.AddItem(item);
        }
        
        // TO HERE (after the loop):
        this.SubscribeForCollectionChanges(hierarchySource);
    }
}
```

### Change 3: Add defensive check (line 733)
```csharp
private void InsertItem(int index, object item, object parent)
{
    if (item == null)
    {
        throw new ArgumentNullException(nameof(item));
    }
    
    // ADD THIS CHECK:
    if (!this.itemLevelMap.ContainsKey(parent))
    {
        throw new InvalidOperationException(
            $"Parent item not found in level map. Parent: {parent}");
    }

#if DEBUG
    if (this.Items.Contains(item))
    {
        throw new InvalidOperationException("The item is already be added to the TreeListBox.");
    }
#endif
    
    this.itemToParentMap[item] = parent;
    
    // ... rest of method unchanged ...
}
```

## Why This Works

1. **Change 1**: Ensures `rootNode` is ALWAYS in `itemLevelMap`, even immediately after clearing
2. **Change 2**: Prevents collection events from firing before initialization is complete
3. **Change 3**: Catches any remaining edge cases with a clear error message

## Testing Checklist

After applying the fix, test:

- [ ] TreeListBox in TabControl (2+ tabs)
- [ ] Switch between tabs multiple times rapidly
- [ ] TreeListBox in second/third tab (not first)
- [ ] Pre-populated ObservableCollection
- [ ] Collection that changes during tab switch
- [ ] Nested TabControls
- [ ] Debug and Release builds

## Files to Modify

- `/Source/PropertyTools.Wpf/TreeListBox/TreeListBox.cs`
  - Line ~690: `ClearItems()` method
  - Line ~508: `HierarchySourceChanged()` method  
  - Line ~733: `InsertItem()` method

## Additional Notes

### Other Affected Lines
These lines also access `itemLevelMap` and may need defensive checks in future:
- Line 451: `container.Level = this.itemLevelMap[item];` (in `PrepareContainerForItemOverride`)

### Similar Issue
Line 752-755 has a check for similar timing issue:
```csharp
if (this.childrenToItemMap.ContainsKey(children))
{
    throw new InvalidOperationException("Children collection already observed.");
}
```
This could also be triggered by TabControl rapid init/deinit.

## Impact Assessment

### Breaking Changes
❌ None - Changes are internal implementation details

### Performance
✅ Minimal - Subscription moved by ~microseconds, no user-visible impact

### Compatibility
✅ Full - All existing code continues to work
✅ Fixes previously broken TabControl scenarios

## References

- Full analysis: `TREELISTBOX_TABCONTROL_BUG_ANALYSIS.md`
- Timing diagrams: `TREELISTBOX_TIMING_DIAGRAM.md`
- Example code: `Source/Examples/TreeListBox/AddRemoveDemo/MainWindow.xaml` (lines 35-50)

## Questions?

**Q: Why not just add the defensive check?**  
A: That masks the symptom but doesn't fix the root cause. Events could still fire at wrong times, causing other issues.

**Q: Why not use locks/synchronization?**  
A: WPF controls should run on UI thread. Locks could cause deadlocks and performance issues.

**Q: Will this fix other timing issues?**  
A: It should fix most race conditions related to initialization order. Some edge cases may remain.

**Q: What if I only want the minimal fix?**  
A: Apply Changes 1 and 3. Change 2 is more defensive but not strictly required if Change 1 works.

---

**Last Updated:** 2024  
**Affects:** PropertyTools.Wpf TreeListBox  
**Severity:** High (causes crash in common scenario)  
**Fix Complexity:** Low (3 small changes, ~10 lines of code)
