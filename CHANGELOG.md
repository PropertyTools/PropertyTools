# Change Log
All notable changes to this project will be documented in this file.

## [3.0.0]
### Changed
- DataGrid commands moved to DataGridCommands

### Removed
- DataGrid: AutoSizeColumns property
- Observable methods based on expressions
- ExpressionUtilities class

### Fixed
- DataGrid: auto column width
- DataGrid: scrolling row headers with horizontal scroll bar
- DataGrid: set correct row when sorting rows and adding a new row 
- PropertyGrid: support BrowsableAttribute on enum items when shown as ComboBox or ListBox #133

### Added
- EditableAttribute
- DataGrid: sorting commands #124
- DataGrid: IsMoveAfterEnterEnabled property #158
- PropertyGrid: support nullable enums #129
- PropertyGrid: option to disable auto fill #143
- DataGrid: register converters in cell definition factory #160

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

[Unreleased]: https://github.com/objorke/PropertyTools/compare/v2.0.1...HEAD
[2.0.1]: https://github.com/objorke/PropertyTools/compare/v2.0.1...v1.1.0
[1.1.0]: https://github.com/objorke/PropertyTools/compare/v1.0.0...v1.1.0
[1.0.0]: https://github.com/objorke/PropertyTools/compare/v0.1.0...v1.0.0
