# Demo Launcher

The Demo Launcher is a centralized application that discovers and launches all example windows from PropertyTools example assemblies.

## Features

- **Automatic Discovery**: Automatically finds all Window classes ending with "Example" from loaded assemblies
- **Filtering**: Filter examples by title, description, tags, or assembly name
- **Metadata Support**: Use the `[Example]` attribute to provide title, description, and tags
- **Keyboard Support**: Press Enter to launch the selected example
- **Command Line**: Launch specific examples directly from the command line
- **Screenshot Capture**: Capture screenshots of all examples with `--capture` argument

## Usage

### Running the Launcher

Simply run `DemoLauncher.exe` to see all available examples in a list.

### Launching a Specific Example

```bash
# By example title
DemoLauncher.exe "Example Name"

# By class name
DemoLauncher.exe ExampleClassName

# By full type name (namespace.ClassName)
DemoLauncher.exe DataGridDemo.FilteringExample
```

### Capturing Screenshots

**Capture all examples to default folder (Screenshots):**
```bash
DemoLauncher.exe --capture
```

**Capture all examples to a custom folder:**
```bash
DemoLauncher.exe --capture output
DemoLauncher.exe --capture C:\MyScreenshots
```

**Capture a single example with default filename:**
```bash
DemoLauncher.exe FilteringExample --capture
```

**Capture a single example with custom filename:**
```bash
DemoLauncher.exe FilteringExample --capture FilteringExample.png
DemoLauncher.exe DataGridDemo.FilteringExample --capture MyCustomName.png
```

## Adding Examples

Examples are discovered automatically if they follow these conventions:

1. Class must be a `Window` type
2. Class name must end with "Example"
3. Class must be public and not abstract

### Using the Example Attribute

To provide better metadata, decorate your example class with the `[Example]` attribute:

```csharp
[Example("My Example Title", "This is a description of what the example demonstrates")]
public partial class MyExample : Window
{
    public MyExample()
    {
        InitializeComponent();
    }
}
```

You can also add tags:

```csharp
[Example("My Example", "Description", Tags = new[] { "PropertyGrid", "Advanced", "Validation" })]
public partial class MyExample : Window
{
    // ...
}
```

If no attribute is provided, the launcher will create an instance of the window and use its `Title` property.
