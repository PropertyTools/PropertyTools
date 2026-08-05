# Coding Agent Guidelines for PropertyTools

Quick reference for AI coding agents working on PropertyTools - a WPF controls library targeting
.NET Framework 4.6.2, .NET 8, .NET 9 and .NET 10 (Windows for the WPF projects; the core
`PropertyTools` library also targets `netstandard2.0`).

This file follows the [AGENTS.md](https://agents.md) convention read by Claude Code and most other
AI coding tools. GitHub Copilot instead reads [.github/copilot-instructions.md](.github/copilot-instructions.md),
and Claude Code additionally reads [CLAUDE.md](CLAUDE.md) (a thin pointer back to this file). Keep
facts consistent across all three — this file is the detailed, canonical source.

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

Full build and test execution requires Windows (WPF dependency). If you're on Windows, the commands
above are all you need.

### Building on Linux (compile-time verification only)

Coding agents often run on Linux, where the commands above fail immediately with
`NETSDK1100: To build a project targeting Windows on this operating system, set the
EnableWindowsTargeting property to true.` The following lets an agent verify that changes actually
**compile** — including XAML — without Windows. It does **not** let you run the app or execute
`PropertyTools.Wpf.Tests`; that still needs a Windows machine or CI runner.

**1. Install a matching .NET SDK.** On Debian/Ubuntu (this repo currently targets .NET 8 and .NET 10),
the SDK is in the standard archive — no Microsoft package feed or network access beyond the distro
mirrors is needed:

```bash
apt-get update
apt-get install -y dotnet-sdk-10.0   # or dotnet-sdk-8.0
```

**2. Pass `-p:EnableWindowsTargeting=true` to `dotnet build`.** This tells the SDK to restore the
`Microsoft.WindowsDesktop.App` *reference assemblies* from NuGet (metadata only, needed to compile
against `System.Windows.*`) instead of requiring the real Windows Desktop runtime:

```bash
# A single project (fast; prefer this for verifying a specific change)
dotnet build Source/PropertyTools.Wpf/PropertyTools.Wpf.csproj -p:EnableWindowsTargeting=true
dotnet build Source/PropertyTools.Wpf.Tests/PropertyTools.Wpf.Tests.csproj -p:EnableWindowsTargeting=true
dotnet build Source/Examples/DataGrid/DataGridDemo/DataGridDemo.csproj -p:EnableWindowsTargeting=true

# The whole solution
dotnet build Source/PropertyTools.sln -p:EnableWindowsTargeting=true
```

This compiles C# **and** XAML (markup compilation works cross-platform on modern SDKs) and reports
real compiler errors — it caught real bugs during development of the SpreadsheetExample model, for
example. Prefer building the specific project(s) you touched: a known pre-existing issue in
`Examples/ItemsBag/ItemsBagDemo` (an ambiguous `CategoryAttribute`/`DescriptionAttribute` reference,
unrelated to any particular change) currently fails a full solution build regardless of platform.

**3. Do not attempt `dotnet test` or `dotnet run` here.** They fail with "You must install or update
.NET to run this application... Framework: 'Microsoft.WindowsDesktop.App'" — the actual WPF runtime
(native rendering, not just reference assemblies) only ships for Windows. A clean `dotnet build`
with `EnableWindowsTargeting=true` and zero errors is the full extent of what Linux can verify;
call this out explicitly when reporting results instead of claiming tests passed.

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
- [ ] Compatible with all target frameworks (.NET Framework 4.6.2, .NET 8, .NET 9, .NET 10)
