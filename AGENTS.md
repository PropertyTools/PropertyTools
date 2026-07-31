# Coding Agent Guidelines for PropertyTools

Quick reference for AI coding agents working on PropertyTools - a WPF controls library targeting .NET 4.6.2 and .NET 8 - Windows.

## Project Structure

- **PropertyTools.Wpf**: Main WPF control library
- **PropertyTools**: Core library
- **Examples**: Demo applications
- **PropertyTools.Tests**: Cross-platform NUnit test suite (no WPF; runs on Linux and Windows)
- **PropertyTools.Wpf.Tests**: NUnit test suite (headless WPF component tests; Windows only)
- **PropertyTools.Wpf.ExampleTests**: Example-driven smoke and visual snapshot tests (Windows only)
- **PropertyTools.Wpf.UITests**: End-to-end UI automation tests using FlaUI (Windows desktop session; nightly workflow)

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
# Build (works on Linux and Windows thanks to EnableWindowsTargeting)
dotnet build Source/PropertyTools.sln --configuration Release

# Run all tests in the solution (Windows only - includes WPF tests)
dotnet test Source/PropertyTools.sln
```

### Test Layers

The tests are organized in layers with NUnit categories so they can be filtered with `dotnet test --filter`:

| Layer | Project | Category | Runs on |
|-------|---------|----------|---------|
| 1. Cross-platform logic tests | `Source/PropertyTools.Tests` | `CrossPlatform` | Linux + Windows |
| 2. Headless WPF component tests | `Source/PropertyTools.Wpf.Tests` | `WpfHeadless` | Windows (headless, no display interaction) |
| 3. Example smoke tests | `Source/PropertyTools.Wpf.ExampleTests` | `WpfHeadless`, `ExampleSmoke` | Windows (headless) |
| 3. Visual snapshot tests | `Source/PropertyTools.Wpf.ExampleTests` | `Visual` | Windows (headless) |
| 4. End-to-end UI automation | `Source/PropertyTools.Wpf.UITests` | `E2E` | Windows desktop session (nightly `ui-tests.yml` workflow) |

```bash
# Cross-platform tests - the ONLY tests that can be executed in a Linux sandbox
dotnet test Source/PropertyTools.Tests/PropertyTools.Tests.csproj

# Headless WPF component tests (Windows)
dotnet test Source/PropertyTools.Wpf.Tests/PropertyTools.Wpf.Tests.csproj

# Example smoke + snapshot tests (Windows)
dotnet test Source/PropertyTools.Wpf.ExampleTests/PropertyTools.Wpf.ExampleTests.csproj

# End-to-end UI automation (Windows; build DemoLauncher first)
dotnet build Source/Examples/DemoLauncher/DemoLauncher.csproj --configuration Release
dotnet test Source/PropertyTools.Wpf.UITests/PropertyTools.Wpf.UITests.csproj --configuration Release --filter TestCategory=E2E
```

### Notes for AI agents (Linux sandboxes)

- The whole solution **builds** on Linux, but WPF tests can only **run** on Windows.
- Always build the solution and run `Source/PropertyTools.Tests` locally; rely on the CI Windows job for the WPF test layers.
- New headless WPF component tests should derive from `WpfTestBase` in `Source/PropertyTools.Wpf.Tests/Harness`, which provides STA setup, layout helpers (`PrepareForLayout`), dispatcher pumping (`DoEvents`) and visual-tree search (`FindVisualChildren`).
- New example windows (public `Window` subclasses whose names end with `Example` in the demo assemblies) are picked up automatically by the smoke and snapshot tests.
- Visual snapshot baselines live in `Source/PropertyTools.Wpf.ExampleTests/Snapshots` (see the README there for how to add/update them).

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
