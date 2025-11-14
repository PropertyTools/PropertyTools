# Coding Agent Guidelines for PropertyTools

Quick reference for AI coding agents working on PropertyTools - a WPF controls library targeting .NET 4.6.2 and .NET 8 - Windows.

## Project Structure

- **PropertyTools.Wpf**: Main WPF control library
- **PropertyTools**: Core library
- **Examples**: Demo applications
- **PropertyTools.Wpf.Tests**: NUnit test suite

Development branch: `develop`

## Code Style

### Copyright Headers (Required)

```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileName.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
```

### Naming Conventions

- **Classes**: PascalCase (`PropertyGrid`, `ColorHelper`)
- **Test Classes**: `{ClassUnderTest}Tests`
- **Test Methods**: `MethodName_StateUnderTest_ExpectedBehavior`
- **Fields**: camelCase for private, PascalCase for const/static readonly
- **Properties/Methods**: PascalCase

## Building and Testing

```bash
# Build
dotnet build Source/PropertyTools.sln --configuration Release

# Test
dotnet test Source/PropertyTools.sln
```

## Writing Tests

- Use NUnit with constraint syntax: `Assert.That(result, Is.EqualTo(expected))`
- Target 80% coverage for new code
- Follow AAA pattern (Arrange, Act, Assert)
- Test edge cases (null, empty, boundaries)

Example:
```csharp
[TestFixture]
public class ColorHelperTests
{
    [Test]
    public void ChangeAlpha_ValidColor_ReturnsCorrectValue()
    {
        // Arrange
        var color = Colors.Lavender;
        byte alpha = 127;

        // Act
        var result = ColorHelper.ChangeAlpha(color, alpha);

        // Assert
        Assert.That(ColorHelper.ColorToHex(result), Is.EqualTo("#7FE6E6FA"));
    }
}
```

## Demo Applications

Structure:
```
DemoName/
├── App.xaml
├── MainWindow.xaml
├── MainWindowViewModel.cs  # MVVM pattern with INotifyPropertyChanged
└── [Model classes].cs
```

## Documentation Updates (Critical)

### CHANGELOG.md (Always Required)

Update `## Unreleased` section. **Each entry must end with `#IssueNumber`**:

```markdown
### Added
- Brief description of feature #123

### Fixed
- Brief description of fix #456

### Changed
- Brief description of change #789
```

### CONTRIBUTORS

Add new contributors alphabetically:
```
Name <email@example.com>
```

### README.md

Update when adding new controls or changing features.

## Pull Request Checklist

- [ ] Code follows style guidelines with copyright headers
- [ ] Tests added for new functionality (80% coverage goal)
- [ ] Tests use naming pattern and constraint syntax
- [ ] **CHANGELOG.md updated with issue reference**
- [ ] CONTRIBUTORS updated (if new contributor)
- [ ] README.md updated (if new control/feature)
- [ ] All tests pass
- [ ] Compatible with both .NET 4.6.2 and .NET 8
