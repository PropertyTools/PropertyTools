---
applyTo: "Source/PropertyTools.Wpf/**/*.cs,Source/PropertyTools.Wpf.ExtendedToolkit/**/*.cs"
---

# WPF Control Instructions

When working with WPF controls in PropertyTools:

## Control Development Standards

### Naming
- Controls: Use descriptive PascalCase names (e.g., `ColorPicker`, `EditableTextBlock`)
- Dependency Properties: Use `{PropertyName}Property` pattern
- Parts (template elements): Use `Part{ElementName}` constants

### Code Organization
- Use `this.` prefix for instance members
- Keep OnApplyTemplate concise
- Separate concerns (rendering, events, data)

### Dependency Properties
Follow WPF patterns:
```csharp
public static readonly DependencyProperty SelectedColorProperty =
    DependencyProperty.Register(
        nameof(SelectedColor),
        typeof(Color),
        typeof(ColorPicker),
        new PropertyMetadata(Colors.Black, OnSelectedColorChanged));
```

### Event Handling
- Use weak event patterns where appropriate
- Properly unsubscribe from events in cleanup
- Handle null references defensively

## WPF-Specific Considerations
- Test with different DPI settings
- Consider accessibility (keyboard navigation, screen readers)
- Be mindful of performance with large data sets
- Support data binding properly
- Follow WPF naming conventions for events (e.g., `SelectionChanged`)

## Multi-Targeting
This code targets:
- .NET Framework 4.6.2
- .NET 8 - Windows
- .NET 9 - Windows
- .NET 10 - Windows

Use conditional compilation (`#if`) if platform-specific APIs are needed.
