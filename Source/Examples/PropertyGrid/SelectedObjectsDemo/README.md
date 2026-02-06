# SelectedObjects Demo

This demo demonstrates the PropertyGrid SelectedObjects binding feature.

## What it shows

- Binding an ObservableCollection to PropertyGrid.SelectedObjects
- Dynamic updates when items are added or removed from the collection
- How PropertyGrid handles multiple selected objects using ItemsBag

## Key Features

1. **Initial Binding**: On startup, two TestObject instances are bound to the PropertyGrid
2. **Add/Remove Items**: Use the buttons to dynamically modify the collection
3. **PropertyGrid Updates**: The PropertyGrid automatically updates as the collection changes

## How to Use

1. Launch the application - you'll see a PropertyGrid displaying properties common to both initial objects
2. Click "Add Item" to add more objects to the collection
3. Click "Remove Item" to remove the last object from the collection
4. Click "Clear All" to remove all objects (PropertyGrid will become empty)

## Technical Details

The fix includes:
- Using `FrameworkPropertyMetadata` with `BindsTwoWayByDefault` for proper binding support
- Initializing CurrentObject when an ObservableCollection is first bound
- Handling INotifyCollectionChanged events to track collection changes

