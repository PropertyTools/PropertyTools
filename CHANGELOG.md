# Change Log
All notable changes to this project will be documented in this file.

## [3.0.0]
### Changed
- DataGrid: Commands moved to DataGridCommands
- DataGrid: DeleteOverride renamed to ClearOverride
- DataGrid: The EasyInsert property is replaced by IsEasyInsertByKeyboardEnabled and IsEasyInsertByMouseEnabled #179
- DataGrid: Changed the IDataGridOperator interface
- PropertyTools NuGet package targets NET45 and .NET Standard 2.0

### Removed
- DataGrid: AutoSizeColumns property
- Observable methods based on expressions
- ExpressionUtilities class

### Fixed
- DataGrid: Auto column width
- DataGrid: Scrolling row headers with horizontal scroll bar
- DataGrid: Set correct row when sorting rows and adding a new row 
- DataGrid: Handle exception when pressing Delete #163
- DataGrid: Clear enum values when pressing Delete #165
- DataGrid: Null ref checks on CollectionView
- DataGrid: Item type when binding to lists of object
- DataGrid: Paste values outside grid boundaries #171
- DataGrid: Auto-insert for IList<IList<>> sources
- DataGrid: Hide selection when disabled #178
- DataGrid: Exception in AddDisplayControl #181
- DataGrid: Support for custom type descriptors #200
- PropertyGrid: Support BrowsableAttribute on enum items when shown as ComboBox or ListBox #133
- PropertyGrid: Hidden error info when HeaderPlacement = Above #212
- TreeListBox: Applied workaround for "Height must be non-negative" exception #38 #142

### Added
- EditableAttribute
- DataGrid: Sorting commands #124
- DataGrid: IsMoveAfterEnterEnabled property #158
- PropertyGrid: Support nullable enums #129
- PropertyGrid: Option to disable auto fill #143
- PropertyGrid: Update SelectedObjects to be bindable when collection changes #175
- DataGrid: Register converters in cell definition factory #160
- DataGrid: CanClear property

## [2.0.1]
### Changed
- DataGrid: IDataGridControlFactory interface
- DataGrid: TemplateColumnDefinition binding path
- Target .NET 4.5 #117
- PropertyGrid: Renamed PropertyControlFactory to PropertyGridControlFactory
- PropertyGrid: Renamed PropertyItemFactory to PropertyGridOperator

### Added
- DataGrid: Support for different types in the same column #118
- DataGrid: Support for cell backgrounds #119
- DataGrid: CellDefinitionFactory property #120

## [1.1.0]
### Added
- DataGrid: Support for IsEnabled on cell level

## [1.0.0] - 2016-09-08
### Added
- First semantic version release

[Unreleased]: https://github.com/PropertyTools/PropertyTools/compare/v3.0.0...HEAD
[3.0.0]: https://github.com/PropertyTools/PropertyTools/compare/v3.0.0...v2.0.1
[2.0.1]: https://github.com/PropertyTools/PropertyTools/compare/v2.0.1...v1.1.0
[1.1.0]: https://github.com/PropertyTools/PropertyTools/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/PropertyTools/PropertyTools/compare/v0.1.0...v1.0.0
