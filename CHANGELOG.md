# Change Log
All notable changes to this project will be documented in this file.

## Unreleased

### Added
- Avalonia: Created new PropertyTools.Avalonia library alongside PropertyTools.Wpf with PropertyGrid, DataGrid, and ColorPicker controls #TBD
- Avalonia: Created AvaloniaExample application demonstrating all three controls #TBD
- DataGrid: Added FilteringExample demonstrating how to filter DataGrid items using CollectionViewSource with search text, enum, and boolean filters #392

### Changed
- Created GitHub issue templates (bug report and feature request) and pull request template following best practices #448 #452
- Observable: Added comprehensive unit tests for VerifyProperty method including tests for inherited properties #462
- PropertyGridDemos: Added ValidationErrorStyleExample demonstrating how to use ValidationErrorStyle with a custom ControlFactory #455

### Fixed
- PropertyGrid: Fixed style application issue where implicit styles from Style.Resources were not applied to controls when bound to objects implementing IDataErrorInfo or INotifyDataErrorInfo #455

### Changed
- Custom GitHub Copilot agent for updating package dependencies and target frameworks #422
- GitHub Actions workflows: Support for building with .NET 10 SDK in all workflows #424
- Documentation: Added AGENTS.md with comprehensive coding agent guidelines including code style, test coverage requirements, how to write tests, how to implement demos, and documentation update requirements #420
- Documentation: Added CLAUDE.md as a quick reference guide for Claude AI that refers to AGENTS.md #420
- Support for .NET 10 - Windows #416
- Support for .NET 8 - Windows #367
- ProgressAttribute #391
- DataGrid/PropertyGrid: ILocalizableOperator and ICustomLocalizableOperator interfaces #398
- DataDialog supporting INotifyDataErrorInfo #405
- ItemsBag: Added comprehensive unit tests for value type properties (int, double, enum) #355
- ItemsBag: Added comprehensive unit tests for already-nullable value types to verify PropertyType behavior #355

### Changed
- GitHub Actions: Configure CodeQL workflow to use security-extended query suite for more comprehensive security scanning #437
- PropertyGridDemo: Implemented INotifyPropertyChanged in Example base class and all Example classes, removed Fody dependency #TBD
- Tests: Updated NUnit tests to use constraint syntax (Assert.That with Is.EqualTo) instead of classic assertions (Assert.AreEqual) #417
- Updated DotNetProjects.Extended.Wpf.Toolkit package from version 5.0.103 to 5.0.129
- Tests: Upgraded NUnit from 3.12.0 to 4.4.0 for latest features and improvements
- Tests: Upgraded Microsoft.NET.Test.SDK from 16.0.1 to 18.0.1 for improved test execution
- Tests: Upgraded NUnit3TestAdapter from 3.15.1 to 6.0.1 for compatibility with NUnit 4
- ItemsBag: Improved documentation explaining how it works with type descriptors and property descriptors #355

### Removed
- AboutDialog: Removed from PropertyTools.Wpf library and moved to DialogDemos example #431
- PropertyGridDemo: Removed Fody and PropertyChanged.Fody package dependencies #TBD

### Fixed
- Observable: VerifyProperty now correctly accepts inherited properties in addition to declared properties #462
- DataGrid: Handle SerializationException gracefully in clipboard operations when non-serializable objects are present #460
- Security: FilePicker.Explore() - Validate and escape file paths to prevent command injection attacks #459
- Security: FilePicker.Open() - Validate file paths before opening to prevent path traversal attacks #459
- Security: DirectoryPicker.Explore() - Validate and escape directory paths to prevent command injection attacks #459
- Security: LinkBlock - Restrict URI schemes to safe protocols (http, https, mailto, ftp) to prevent malicious URI execution #459
- DataGrid: Paste operation with active sorting now correctly updates items in source collection instead of incorrect rows #444
- DataGrid: Clipboard commands remain enabled when grid is empty (no rows) #450
- DataGrid: Context menu items should be disabled when operations are not applicable #446
- FilePicker: Fix Open button not opening files with associated applications on .NET Core/.NET 5+ #435
- DirectoryPicker: Use full system path for explorer.exe to prevent PATH-based security vulnerabilities #433
- FilePicker: Use full system path for explorer.exe to prevent PATH-based security vulnerabilities #433
- AboutDialog: Use full system path for msinfo32.exe to prevent PATH-based security vulnerabilities #429
- GitHub Actions: Fix CodeQL workflow failure by upgrading to CodeQL action v3 #426
- DataGrid: Update after pasting values #269
- DataGrid: Deleting last item in a sorted data grid causes an exception #321
- TreeListBox: Fix items being added under collapsed nodes #264
- Update namespace for ButtonChrome and SystemDropShadowChrome controls #316
- DataGrid: Support for untyped lists of lists #343
- PropertyGrid: Fix description icon alignment for HeaderPlacement set to Above #347
- RadioButtonList: Enhance RadioButton Control with Individual Button Enable/Disable Capability #350
- ItemsBag: Ensure that the ItemsBag Property Descriptor does not suppress the property change notifications #354
- ItemsBag: Propagation of IsReadOnly property to the ItemsBag Property Descriptor #369
- TreeListBox: Catching the ArgumentException by message title, fails in non english regions #38 #142

## [3.1.0]
### Added
- Support for .NET Core 3.0 #221
- DataGrid: Support for items source in columns #232
- PropertyGrid: Make PropertyGrid extendable to support warning messages #248
- PropertyGrid: Added property to set duration of tooltip #259

### Fixed
- DataGrid: Returning incorrect 'SelectedItems' when sorted #217
- DataGrid: Fix auto width issue #224
- PropertyGrid: Support for INotifyDataErrorInfo #226
- DataGrid: Align DataGrid scrollbar code #238
- DataGrid: Exception when sorting empty collection #240
- LinkBlock: Fix exception #261

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
- DataGrid: Auto width not working with horizontal stretch #223
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
