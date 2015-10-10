[![Build status](https://ci.appveyor.com/api/projects/status/b3579d3lja65kt6m)](https://ci.appveyor.com/project/objorke/propertytools)

```
License:       The MIT License (MIT)
Project page:  https://github.com/objorke/PropertyTools/
NuGet:         https://www.nuget.org/packages/PropertyTools.Wpf/
Gitter chat:   https://gitter.im/objorke/PropertyTools
```

| Control           | Description                                                                              | Status |
|-------------------|------------------------------------------------------------------------------------------|--------|
| PropertyGrid      | A control that shows properties of an object or a collection of objects.                 | Stable |
| DataGrid          | A data grid with an "Excel feel" (note that the control is not virtualized)              | Stable |
| TreeListBox       | A ListBox that looks and feels like a TreeView (supports multi-select and drag-drop)     | Some bugs remaining |
| ColorPicker       | A color picker                                                                           | Stable |
| RadioButtonList   | A collection of radio buttons that binds to an enum                                      | Stable |
| EnumMenuItem      | A collection of checkable menuitems that binds to an enum                                | Stable |
| EditableTextBlock | A TextBlock that can be changed into a TextBox, useful for in-place editing in TreeViews | Stable |
| FilePicker        | A TextBox with browse for file button                                                    | Stable |
| DirectoryPicker   | A TextBox with browse for directory button                                               | Stable |
| DockPanelSplitter | A splitter for DockPanels                                                                | Stable |
| SpinControl       | A numeric up/down spinner control                                                        | Stable |
| LinkBlock         | A hyperlink on a TextBlock                                                               | Stable |
| SliderEx          | A Slider that calls IEditableObject.BeginEdit/EndEdit when thumb dragging                | Stable |
| PopupBox          | A restyled `ComboBox` where you can put anything in the Popup                            | Stable |
| FormattingTextBox | A `TextBox` where you can bind the StringFormat                                          | Stable |

### PropertyGrid

![PropertyGrid](/Images/PropertyGrid.png)

### DataGrid

![DataGrid](/Images/DataGrid.png)

### TreeListBox

![TreeListBox](/Images/TreeListBox.png)

### ColorPicker

![ColorPicker](/Images/ColorPicker.png) ![ColorPicker](/Images/ColorPicker2.png)

### AboutDialog

Generic "About" dialog

- Titles and version is taken from AssemblyInfo
- Open System Info...
- Copy report

![AboutDialog](/Images/AboutDialog.png)

### PropertyDialog

Below is an example `PropertyDialog` bound to `Settings.Default`:

![OptionsDialog](/Images/PropertyDialog.png)

### Build requirements

- Microsoft 4.0 or later
- Portable library tools
- Visual Studio 2010 or later

### Links

Semantic versioning [semver.org](http://semver.org/)
Semver tool: [GitVersion](https://github.com/GitTools/GitVersion)
Branching strategy: [GitHub Flow](https://guides.github.com/introduction/flow/index.html)
