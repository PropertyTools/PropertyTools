---
applyTo: "Source/Examples/**/*.cs,Source/Examples/**/*.xaml"
---

# Example/Demo Application Instructions

When working with example and demo applications:

## Structure Requirements
Each demo should follow this structure:
```
DemoName/
├── App.xaml
├── App.xaml.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── MainWindowViewModel.cs  # MVVM pattern with INotifyPropertyChanged
└── [Model classes].cs
```

## Code Standards
- Use MVVM pattern with proper separation of concerns
- Implement `INotifyPropertyChanged` in ViewModels
- Keep UI code in XAML, logic in ViewModels
- Use meaningful example data that demonstrates the control's capabilities

## Documentation
- Add XML comments to explain what the demo demonstrates
- Include comments for non-obvious implementation details
- Examples should be self-documenting and educational

## Namespace Convention
Use appropriate namespace for example-specific code (e.g., `ExampleLibrary`, `PropertyGridDemo`)

## Best Practices
- Demonstrate one clear concept per example
- Keep examples focused and concise
- Use realistic sample data
- Show best practices for using the control
