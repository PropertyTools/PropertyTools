# TreeListBox Crash When Used in TabControl

## Technical Summary

The `TreeListBox` control throws a `NullReferenceException` when placed inside a `TabControl` and the user switches from an empty tab to a tab containing the TreeListBox with bound items. The crash occurs during the item insertion process due to a race condition in the control's initialization sequence.

**Severity:** High - Prevents use of TreeListBox in TabControl scenarios  
**Impact:** Application crash when switching to tabs containing TreeListBox  
**Affected Component:** `PropertyTools.Wpf.TreeListBox`

## Suspected Root Cause

**File:** `/Source/PropertyTools.Wpf/TreeListBox/TreeListBox.cs`  
**Method:** `InsertItem(int index, object item, object parent)`  
**Line:** 760

### The Problem

The crash is caused by a **race condition** during TreeListBox initialization:

1. **Line 528-544:** `HierarchySourceChanged()` is called when the TabControl switches tabs
2. **Line 528:** `ClearItems()` removes all entries from internal dictionaries, including `rootNode`
3. **Line 538:** `SubscribeForCollectionChanges(hierarchySource)` subscribes to collection changes
4. **Lines 540-543:** Items are added via `AddItem()` in a loop
5. **RACE CONDITION:** Between clearing and re-adding `rootNode` (line 535), collection change events can fire
6. **Line 760:** `InsertItem()` tries to access `this.itemLevelMap[parent]` where parent is `rootNode`
7. **Result:** `KeyNotFoundException` because `rootNode` was removed but not yet re-added

### Why TabControl Triggers This

TabControl uses **virtualization and deferred loading** which causes:
- Controls in non-visible tabs load asynchronously
- Multiple rapid property changes during tab switching
- Unpredictable WPF binding resolution order
- Collection change event subscriptions activating before parent items are fully initialized

### Code Evidence

**The crash location (Line 760):**
```csharp
private void InsertItem(int index, object item, object parent)
{
    // ... validation code ...
    
    this.itemToParentMap[item] = parent;
    var children = this.GetChildrenCollectionByReflection(item);
    this.itemToChildrenMap[item] = children;
    this.childrenToItemMap[children] = item;
    this.SubscribeForCollectionChanges(children);
    
    // CRASH HERE: parent (rootNode) not in dictionary
    this.itemLevelMap[item] = this.itemLevelMap[parent] + 1;  // LINE 760
    
    this.isExpandedMap[item] = false;
    this.Items.Insert(index, item);
}
```

**The initialization sequence (Lines 508-544):**
```csharp
private void HierarchySourceChanged(DependencyPropertyChangedEventArgs e)
{
    // ... cleanup old source ...
    
    this.ClearItems();  // LINE 528 - Removes rootNode from all dictionaries
    
    var hierarchySource = this.HierarchySource as IList ?? this.HierarchySource?.Cast<object>().ToList();
    if (hierarchySource != null)
    {
        this.childrenToItemMap.Add(hierarchySource, this.rootNode);
        this.itemToChildrenMap.Add(this.rootNode, hierarchySource);
        this.itemLevelMap[this.rootNode] = -1;  // LINE 535 - Re-adds rootNode
        this.isExpandedMap[this.rootNode] = true;
        
        this.SubscribeForCollectionChanges(hierarchySource);  // LINE 538 - Events can now fire!
        
        // During this loop, collection change events can trigger InsertItem
        foreach (var item in hierarchySource)  // LINES 540-543
        {
            this.AddItem(item);
        }
    }
}
```

**The clearing code (Lines 694-703):**
```csharp
private void ClearItems()
{
    // ... cleanup code ...
    
    this.itemToParentMap.Clear();
    this.itemToChildrenMap.Clear();
    this.childrenToItemMap.Clear();
    this.itemLevelMap.Clear();      // LINE 701 - rootNode removed here
    this.isExpandedMap.Clear();     // LINE 703
}
```

## Reproduction Steps for AI Coding Agent

### Setup

1. **Navigate to TreeListBoxDemo project:**
   ```bash
   cd /home/runner/work/PropertyTools/PropertyTools/Source/Examples/TreeListBox/TreeListBoxDemo
   ```

2. **Create test scenario - Modify MainWindow.xaml:**
   ```xml
   <Window x:Class="TreeListBoxDemo.MainWindow"
           xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
           xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
           xmlns:pt="http://propertytools.org/wpf"
           xmlns:local="clr-namespace:TreeListBoxDemo"
           Title="TreeListBox TabControl Bug" Height="600" Width="800">
       <Window.Resources>
           <DataTemplate DataType="{x:Type local:NodeViewModel}">
               <Grid>
                   <Grid.ColumnDefinitions>
                       <ColumnDefinition Width="Auto"/>
                       <ColumnDefinition Width="*"/>
                   </Grid.ColumnDefinitions>
                   <Image Grid.Column="0" Source="/folder.png" Margin="0 0 4 0"/>
                   <pt:EditableTextBlock Grid.Column="1" Text="{Binding Name}"/>
               </Grid>
           </DataTemplate>
       </Window.Resources>
       
       <TabControl>
           <!-- Empty tab -->
           <TabItem Header="Empty Tab">
               <TextBlock Text="This tab is empty" VerticalAlignment="Center" HorizontalAlignment="Center"/>
           </TabItem>
           
           <!-- Tab with TreeListBox -->
           <TabItem Header="TreeListBox Tab">
               <pt:TreeListBox 
                   x:Name="tree1" 
                   Indentation="12" 
                   HierarchySource="{Binding Roots}"
                   BorderThickness="1"/>
           </TabItem>
       </TabControl>
   </Window>
   ```

3. **Create MainWindow.xaml.cs with view model:**
   ```csharp
   using System.Collections.ObjectModel;
   using System.Windows;
   
   namespace TreeListBoxDemo
   {
       public partial class MainWindow : Window
       {
           public MainWindow()
           {
               InitializeComponent();
               DataContext = new MainWindowViewModel();
           }
       }
       
       public class MainWindowViewModel
       {
           public ObservableCollection<NodeViewModel> Roots { get; set; }
           
           public MainWindowViewModel()
           {
               Roots = new ObservableCollection<NodeViewModel>
               {
                   new NodeViewModel { Name = "Root 1" },
                   new NodeViewModel { Name = "Root 2" },
                   new NodeViewModel { Name = "Root 3" }
               };
               
               // Add children to each root
               foreach (var root in Roots)
               {
                   root.Children.Add(new NodeViewModel { Name = $"{root.Name} - Child 1" });
                   root.Children.Add(new NodeViewModel { Name = $"{root.Name} - Child 2" });
               }
           }
       }
   }
   ```

4. **Update App.xaml to launch MainWindow:**
   ```xml
   <Application x:Class="TreeListBoxDemo.App"
                xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                StartupUri="MainWindow.xaml">
   </Application>
   ```

### Steps to Reproduce the Crash

1. **Build the project:**
   ```bash
   dotnet build TreeListBoxDemo.csproj
   ```

2. **Run the application:**
   ```bash
   dotnet run --project TreeListBoxDemo.csproj
   ```

3. **Trigger the crash:**
   - Application starts with "Empty Tab" selected
   - Click on "TreeListBox Tab" to switch tabs
   - **Expected:** TreeListBox displays with items
   - **Actual:** Application crashes with `NullReferenceException`

### Expected Error

```
System.Collections.Generic.KeyNotFoundException: 
The given key was not present in the dictionary.
   at System.Collections.Generic.Dictionary`2.get_Item(TKey key)
   at PropertyTools.Wpf.TreeListBox.InsertItem(Int32 index, Object item, Object parent) 
      in TreeListBox.cs:line 760
```

OR

```
System.NullReferenceException: 
Object reference not set to an instance of an object.
   at PropertyTools.Wpf.TreeListBox.InsertItem(Int32 index, Object item, Object parent) 
      in TreeListBox.cs:line 760
```

### Alternative Reproduction (Faster)

Instead of creating new files, modify the existing `TreeListBoxDemo/Examples/SingleRootExample/SingleRootWindow.xaml`:

```xml
<!-- Wrap the existing TreeListBox in a TabControl -->
<TabControl>
    <TabItem Header="Empty"/>
    <TabItem Header="TreeListBox">
        <pt:TreeListBox 
            x:Name="tree1" 
            Indentation="12" 
            HierarchySource="{Binding Root}"
            BorderThickness="0"/>
    </TabItem>
</TabControl>
```

## Recommended Fix Strategy

### Fix #1: Ensure rootNode is Always in Dictionary (Minimal Impact)

**File:** `TreeListBox.cs`  
**Method:** `ClearItems()`  
**Change:** Immediately re-initialize rootNode after clearing

```csharp
private void ClearItems()
{
    // ... existing cleanup code ...
    
    this.itemToParentMap.Clear();
    this.itemToChildrenMap.Clear();
    this.childrenToItemMap.Clear();
    this.itemLevelMap.Clear();
    this.isExpandedMap.Clear();
    
    // ADD: Immediately reinitialize rootNode to prevent race conditions
    this.itemLevelMap[this.rootNode] = -1;
    this.isExpandedMap[this.rootNode] = true;
}
```

### Fix #2: Defer Collection Change Subscriptions (Better)

**File:** `TreeListBox.cs`  
**Method:** `HierarchySourceChanged()`  
**Change:** Move subscription after items are added

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
        
        // REMOVE: this.SubscribeForCollectionChanges(hierarchySource);
        
        // Add all items first
        foreach (var item in hierarchySource)
        {
            this.AddItem(item);
        }
        
        // ADD: Subscribe AFTER items are added to prevent race conditions
        this.SubscribeForCollectionChanges(hierarchySource);
    }
}
```

### Fix #3: Add Defensive Check (Safety Net)

**File:** `TreeListBox.cs`  
**Method:** `InsertItem()`  
**Change:** Validate parent is in dictionary before accessing

```csharp
private void InsertItem(int index, object item, object parent)
{
    if (item == null)
    {
        throw new ArgumentNullException(nameof(item));
    }
    
    // ADD: Defensive check for parent
    if (!this.itemLevelMap.ContainsKey(parent))
    {
        throw new InvalidOperationException(
            $"Parent item not found in level map. This typically indicates a " +
            $"race condition during control initialization. Parent: {parent}");
    }
    
    // ... rest of existing code ...
}
```

### Recommended Approach

**Apply all three fixes together** for maximum robustness:

1. **Fix #1** ensures rootNode is never missing (prevents the race)
2. **Fix #2** eliminates the primary cause (proper initialization order)
3. **Fix #3** provides clear diagnostics if other issues arise (safety net)

**Impact:** Minimal - all changes are internal to TreeListBox.cs, no public API changes

## Testing Requirements

After applying the fix, verify:

1. ✅ TreeListBox works in direct Window placement (existing behavior)
2. ✅ TreeListBox works in TabControl with initial tab switch
3. ✅ TreeListBox works in TabControl with rapid tab switching
4. ✅ TreeListBox works with dynamic collection changes during tab switches
5. ✅ Multiple TreeListBox instances in different tabs work correctly

## Related Issues

- Issue #282: Fixed multiple root items expansion issue (different problem, but related to TreeListBox initialization)
- Lines 766-779 in TreeListBox.cs show awareness of similar timing issues with ArgumentException catches for "set_Height" and "ExtendViewport"

## Additional Context

- **AddRemoveDemo** already uses TreeListBox in a TabControl (Source/Examples/TreeListBox/AddRemoveDemo/MainWindow.xaml lines 35-50), suggesting TabControl usage is an expected scenario
- The fix should maintain compatibility with existing demos and tests
- No public API changes required - all fixes are internal implementation details

---

**Status:** Ready for implementation  
**Priority:** High - Blocking TabControl usage scenarios  
**Complexity:** Low - 3 small code changes totaling ~10 lines  
**Breaking Changes:** None  
**Test Coverage:** Unit tests recommended (see TREELISTBOX_TEST_CASES.cs)
