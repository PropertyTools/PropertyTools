# TreeListBox Demo

This demo application showcases the PropertyTools TreeListBox control with multiple examples.

## Structure

The application has been restructured to support multiple examples:

- **LauncherWindow**: Main window that allows you to select which example to run
- **Examples/SingleRootExample**: Original demo showing a tree with one root item
- **Examples/MultipleRootsExample**: Demonstrates the fix for issue #282 with multiple root items

## Examples

### Single Root Example

The original TreeListBox demo with a single root node and multiple levels of children. This example demonstrates:

- Hierarchical data display
- Drag and drop support
- Add/Delete operations (F2 to edit, Delete key to remove, + key to add child)
- Expand/collapse functionality
- Adjustable indentation

### Multiple Roots Example

New example showcasing the TreeListBox with multiple root items. This demonstrates:

- Multiple root items in HierarchySource
- Correct child placement under respective parents (fix for issue #282)
- Expand/Collapse all functionality
- Each root has its own subtree

**Issue #282 Fix**: Before the fix, expanding any root item would incorrectly place its children at the end of the list (under the last root item). After the fix, children are correctly positioned under their respective parent root items.

## Keyboard Shortcuts

- **F2**: Edit the selected item name
- **Delete**: Remove the selected item(s)
- **+**: Add a child to the selected item
- **Left/Right Arrow**: Navigate the tree

## Running the Demo

1. Launch the application
2. Select an example from the launcher window
3. Click "Launch Example" or double-click an example to open it
4. Experiment with the TreeListBox features

## Code Organization

```
TreeListBoxDemo/
├── App.xaml                    # Application entry point
├── LauncherWindow.xaml         # Example selection launcher
├── Examples/
│   ├── SingleRootExample/      # Single root demo
│   └── MultipleRootsExample/   # Multiple roots demo (issue #282)
├── Model/                      # Shared data models
│   ├── Node.cs
│   └── CompositeNode.cs
└── ViewModel/                  # Shared view models
    ├── Observable.cs           # Base observable class
    ├── NodeViewModel.cs        # Node view model
    └── MainViewModel.cs        # Single root view model
```

## Related Issues

- [#282](https://github.com/PropertyTools/PropertyTools/issues/282) - TreeListBox HierarchySource - multiple root items expansion not working
