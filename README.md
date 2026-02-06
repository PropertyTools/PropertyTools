# PropertyTools for Avalonia

[![License](https://img.shields.io/github/license/PropertyTools/PropertyTools.svg)](https://github.com/PropertyTools/PropertyTools/blob/develop/LICENSE) 
[![NuGet](https://img.shields.io/nuget/v/PropertyTools.Wpf.svg)](https://nuget.org/packages/PropertyTools.Wpf) 

**PropertyTools** is a collection of custom controls for Avalonia applications. This library is being actively migrated from WPF to Avalonia UI framework.

## Available Controls

| Control           | Description                                                                              | Status        |
|-------------------|------------------------------------------------------------------------------------------|---------------|
| ColorPicker       | A color picker control for selecting colors                                              | Implemented   |
| EditableTextBlock | A TextBlock that can be changed into a TextBox for in-place editing                      | Implemented   |
| PropertyGrid      | A control that shows properties of an object or a collection of objects                  | Implemented   |
| DataGrid          | A data grid with an "Excel feel"                                                         | Planned       |
| TreeListBox       | A ListBox that looks and feels like a TreeView (supports multi-select and drag-drop)    | Planned       |
| RadioButtonList   | A collection of radio buttons that binds to an enum                                      | Planned       |
| EnumMenuItem      | A collection of checkable menu items that binds to an enum                               | Planned       |
| FilePicker        | A TextBox with browse for file button                                                    | Planned       |
| DirectoryPicker   | A TextBox with browse for directory button                                               | Planned       |
| SpinControl       | A numeric up/down spinner control                                                        | Planned       |

## Getting Started

### Installation

```bash
dotnet add package PropertyTools.Wpf
```

### Basic Usage

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:controls="using:PropertyTools.Wpf.Controls">
    <StackPanel>
        <controls:ColorPicker />
        <controls:EditableTextBlock Text="Click to edit" />
        <controls:PropertyGrid SelectedObject="{Binding MyObject}" />
    </StackPanel>
</Window>
```

## Example Application

See the `Source/Examples/AvaloniaDemo` project for working examples of all implemented controls.

### Supported frameworks

- Microsoft .NET 8
- Microsoft .NET 9  
- Microsoft .NET 10

## Migration from WPF

This library is being actively migrated from WPF to Avalonia. Controls are being implemented one by one with full Avalonia support. If you need the WPF version, please refer to earlier releases.

## Contributing

Contributions are welcome! Please read our contributing guidelines before submitting pull requests.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

