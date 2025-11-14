# Coding Agent Guidelines for PropertyTools

This document provides comprehensive guidelines for AI coding agents (GitHub Copilot, Claude, etc.) working on the PropertyTools repository. Following these guidelines ensures consistent, high-quality contributions.

## Table of Contents

1. [Project Overview](#project-overview)
2. [Code Style and Standards](#code-style-and-standards)
3. [Building and Testing](#building-and-testing)
4. [Test Coverage Requirements](#test-coverage-requirements)
5. [How to Write Tests](#how-to-write-tests)
6. [How to Implement Demos](#how-to-implement-demos)
7. [Required Documentation Updates](#required-documentation-updates)
8. [Pull Request Checklist](#pull-request-checklist)

## Project Overview

PropertyTools is a collection of custom controls for WPF applications that provide enhanced property editing, data grid, and tree list capabilities.

### Key Components

- **PropertyTools.Wpf**: Main WPF control library with custom controls (PropertyGrid, DataGrid, TreeListBox, ColorPicker, etc.)
- **PropertyTools**: Core library with shared utilities and helpers
- **PropertyTools.Wpf.Extended.Toolkit**: Extended toolkit controls
- **Examples**: Demo applications showcasing each control's capabilities
- **PropertyTools.Wpf.Tests**: NUnit test suite for all components

### Target Frameworks

- .NET Framework 4.6.2
- .NET 8 - Windows

**Important**: This is a Windows-only WPF project. All code must be compatible with both target frameworks unless explicitly targeting a specific version.

### Development Branch

- Main development branch: `develop`
- Use GitVersion for semantic versioning

## Code Style and Standards

### Namespace Conventions

- Use `PropertyTools.Wpf` for WPF-specific code
- Use `PropertyTools` for core/shared code
- Use descriptive namespaces for examples (e.g., `ExampleLibrary`, `ControlDemos`)

### Copyright Headers

**CRITICAL**: All source files MUST include the standard copyright header:

```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileName.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
```

Replace `FileName.cs` with the actual file name.

### Naming Conventions

#### Classes and Interfaces

- **Classes**: Use PascalCase (e.g., `PropertyGrid`, `ColorPicker`, `DataGrid`)
- **Test Classes**: `{ClassUnderTest}Tests` (e.g., `ColorHelperTests`, `NaturalStringComparerTests`)
- **Helper Classes**: `{Purpose}Helper` (e.g., `ColorHelper`, `TypeHelper`, `ReflectionHelper`)
- **Converters**: `{Type}Converter` or `{Type}To{Type}Converter` (e.g., `EnumValuesConverter`, `BrushToColorConverter`)
- **Controls**: Descriptive PascalCase names (e.g., `EditableTextBlock`, `DockPanelSplitter`)

#### Methods

- Use PascalCase for all method names
- Use descriptive, action-oriented names (e.g., `ChangeAlpha`, `Interpolate`, `Parse`, `Format`)

#### Unit Test Methods

**Follow the pattern**: `MethodName_StateUnderTest_ExpectedBehavior`

Examples:
- `Parse_Days_ReturnsCorrectValue`
- `ChangeAlpha_ValidColor_ReturnsCorrectValue`
- `HexToColor_InvalidColors_ReturnsUndefined`
- `Interpolate_ValidColors_ReturnsCorrectValue`

Reference: [Roy Osherove's naming standards](https://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html)

#### Fields

- **Private fields**: Use camelCase (e.g., `colorPickerPanel`, `selectedColor`, `itemsSource`)
- **Private const fields**: Use PascalCase (e.g., `PartColorPickerPanel`, `DefaultItemHeight`)
- **Static readonly fields**: Use PascalCase (e.g., `SelectedColorProperty` for dependency properties)

#### Properties

- Use PascalCase for all property names (e.g., `SelectedColor`, `UndefinedColor`, `ItemsSource`)

#### Parameters and Local Variables

- Use camelCase (e.g., `color`, `alpha`, `targetType`, `itemIndex`)

### XML Documentation

Use XML documentation comments (`///`) for all public APIs:

```csharp
/// <summary>
/// Changes the alpha value of a color.
/// </summary>
/// <param name="c">The source color.</param>
/// <param name="alpha">The new alpha value.</param>
/// <returns>The color with the new alpha value.</returns>
public static Color ChangeAlpha(this Color c, byte alpha)
{
    return Color.FromArgb(alpha, c.R, c.G, c.B);
}
```

Include:
- `<summary>` for all public members
- `<param>` for all parameters
- `<returns>` for methods returning values
- `<remarks>` for additional implementation notes when needed

### Code Formatting

- Follow the existing code style in the repository
- Use meaningful variable and method names that convey intent
- Keep methods focused and concise (single responsibility)
- Use `this.` prefix for instance members in WPF controls
- Place opening braces on new lines (Allman style)
- Use 4 spaces for indentation (not tabs)

## Building and Testing

### Build Requirements

- **Solution File**: `Source/PropertyTools.sln`
- **Build System**: MSBuild / .NET SDK
- **Windows Required**: Must build on Windows due to WPF dependencies

### Build Commands

```bash
# Restore packages
dotnet restore Source/PropertyTools.sln

# Build solution
dotnet build Source/PropertyTools.sln --configuration Release

# Or use MSBuild
msbuild Source/PropertyTools.sln /t:Restore /p:Configuration=Release
msbuild Source/PropertyTools.sln /p:Configuration=Release
```

### Running Tests

```bash
# Run all tests
dotnet test Source/PropertyTools.sln

# Run tests with detailed output
dotnet test Source/PropertyTools.sln --logger "console;verbosity=detailed"
```

**Test Framework**: NUnit  
**Test Project**: `PropertyTools.Wpf.Tests`

## Test Coverage Requirements

### Coverage Goals

- **New Features**: 80% or higher code coverage for all new functionality
- **Bug Fixes**: Include tests that reproduce the bug before fixing
- **Critical Paths**: 100% coverage for critical functionality (data binding, collection operations, etc.)

### What Needs Tests

1. **All public APIs** in PropertyTools and PropertyTools.Wpf
2. **Helper and utility methods** (ColorHelper, TypeHelper, etc.)
3. **Converters** (value converters, type converters)
4. **Extensions** (extension methods)
5. **Core control functionality** (property changes, events, validation)
6. **Edge cases and error conditions**

### What Doesn't Need Tests

- Simple getters/setters without logic
- UI-only code (pure XAML without code-behind logic)
- Demo applications in the Examples folder
- Auto-generated code

## How to Write Tests

### Test File Organization

Place test files in `Source/PropertyTools.Wpf.Tests` following this structure:

```
PropertyTools.Wpf.Tests/
├── Comparers/
│   └── NaturalStringComparerTests.cs
├── Converters/
│   ├── ConverterTests.cs
│   └── ValueToBooleanConverterTests.cs
├── Extensions/
│   └── ReflectionExtensionsTests.cs
├── Helpers/
│   ├── ColorHelperTests.cs
│   ├── TypeHelperTests.cs
│   └── TimeSpanParserTests.cs
└── ItemsBag/
    └── ItemsBagTests.cs
```

### Test Class Template

```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorHelperTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Windows.Media;
    using NUnit.Framework;

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

        [Test]
        public void HexToColor_InvalidColors_ReturnsUndefined()
        {
            // Assert that invalid hex colors return UndefinedColor
            Assert.That(ColorHelper.HexToColor("#FFFG00FF"), Is.EqualTo(ColorHelper.UndefinedColor));
            Assert.That(ColorHelper.HexToColor("#FFFG00F"), Is.EqualTo(ColorHelper.UndefinedColor));
            Assert.That(ColorHelper.HexToColor("-1"), Is.EqualTo(ColorHelper.UndefinedColor));
        }
    }
}
```

### Testing Best Practices

1. **Use NUnit Constraint Syntax**
   ```csharp
   // GOOD: Modern constraint syntax
   Assert.That(result, Is.EqualTo(expected));
   Assert.That(collection, Has.Count.EqualTo(5));
   Assert.That(value, Is.Not.Null);
   
   // AVOID: Classic assertions (deprecated)
   Assert.AreEqual(expected, result);
   Assert.IsNotNull(value);
   ```

2. **Follow AAA Pattern**
   - **Arrange**: Set up test data and preconditions
   - **Act**: Execute the method being tested
   - **Assert**: Verify the expected outcome

3. **Test One Thing per Test**
   - Each test should verify a single behavior
   - Multiple assertions are OK if testing the same behavior

4. **Use Descriptive Test Names**
   - Follow `MethodName_StateUnderTest_ExpectedBehavior` pattern
   - Name should clearly describe what is being tested

5. **Test Edge Cases**
   - Null values
   - Empty collections
   - Boundary values
   - Invalid input
   - Exception scenarios

6. **Use TestCase for Parameterized Tests**
   ```csharp
   [TestCase("#FF0000FF", "Blue")]
   [TestCase("#FF008000", "Green")]
   [TestCase("#FFFF0000", "Red")]
   public void HexToColor_ValidColors_ReturnsCorrectColor(string hex, string expectedColorName)
   {
       var expected = Color.FromName(expectedColorName);
       Assert.That(ColorHelper.HexToColor(hex), Is.EqualTo(expected));
   }
   ```

7. **Avoid Test Interdependencies**
   - Each test should be independent
   - Tests should pass regardless of execution order

### Testing WPF Controls

When testing WPF controls, focus on:
- Property changes and notifications
- Data binding scenarios
- Event handling
- Validation logic
- Collection operations

Example:
```csharp
[Test]
public void SelectedColor_SetValue_RaisesPropertyChanged()
{
    var colorPicker = new ColorPicker();
    bool eventRaised = false;
    
    colorPicker.PropertyChanged += (s, e) =>
    {
        if (e.PropertyName == nameof(ColorPicker.SelectedColor))
            eventRaised = true;
    };

    colorPicker.SelectedColor = Colors.Blue;

    Assert.That(eventRaised, Is.True);
}
```

## How to Implement Demos

Demo applications showcase control features and provide usage examples. Follow these guidelines when creating or updating demos.

### Demo Project Structure

Each demo should follow this standard structure:

```
DemoName/
├── App.xaml                    # Application entry point
├── App.xaml.cs                 # Application code-behind
├── MainWindow.xaml             # Main window UI
├── MainWindow.xaml.cs          # Main window code-behind
├── MainWindowViewModel.cs      # ViewModel (if using MVVM)
├── DemoName.csproj            # Project file
├── Properties/
│   └── AssemblyInfo.cs        # Assembly information
└── [Model classes].cs         # Domain models as needed
```

### Demo Requirements

1. **Purpose**: Each demo must clearly demonstrate specific control features
2. **Simplicity**: Keep demos focused and easy to understand
3. **Best Practices**: Demonstrate proper usage patterns
4. **MVVM Pattern**: Use MVVM when appropriate (ViewModel for data binding)
5. **Documentation**: Include comments explaining non-obvious code

### Demo Project Template

#### App.xaml
```xaml
<Application x:Class="DemoName.App"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             StartupUri="MainWindow.xaml">
    <Application.Resources>
    </Application.Resources>
</Application>
```

#### MainWindow.xaml
```xaml
<Window x:Class="DemoName.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:pt="http://propertytools.org/wpf"
        Title="Demo Name - PropertyTools Demo" 
        Height="600" Width="800">
    <Grid Margin="10">
        <!-- Demo content here -->
        <pt:PropertyGrid SelectedObject="{Binding DemoObject}" />
    </Grid>
</Window>
```

#### MainWindow.xaml.cs
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DemoName
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel();
        }
    }
}
```

#### MainWindowViewModel.cs
```csharp
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DemoName
{
    using System.ComponentModel;

    /// <summary>
    /// Provides the view model for the main window.
    /// </summary>
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private object demoObject;

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the demo object.
        /// </summary>
        public object DemoObject
        {
            get => this.demoObject;
            set
            {
                this.demoObject = value;
                this.RaisePropertyChanged(nameof(this.DemoObject));
            }
        }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
```

### Demo Categories

Organize demos in the `Source/Examples` folder:

- **Controls/**: General control demonstrations
- **PropertyGrid/**: PropertyGrid-specific demos
- **DataGrid/**: DataGrid-specific demos
- **TreeListBox/**: TreeListBox-specific demos
- **ItemsBag/**: ItemsBag-specific demos

### Demo Best Practices

1. **Show Real-World Usage**: Demonstrate practical scenarios
2. **Include Comments**: Explain complex or important code
3. **Use Sample Data**: Provide meaningful sample data
4. **Handle Events**: Show event handling when relevant
5. **Demonstrate Features**: Cover key features of the control
6. **Keep It Simple**: Don't over-complicate demos
7. **Make It Interactive**: Allow users to interact and see results

## Required Documentation Updates

### 1. CHANGELOG.md (CRITICAL)

**Every pull request MUST update CHANGELOG.md.** This is non-negotiable.

#### Location and Format

Update the `## Unreleased` section at the top of `CHANGELOG.md`:

```markdown
## Unreleased

### Added
- Brief description of new features #IssueNumber

### Fixed
- Brief description of bug fixes #IssueNumber

### Changed
- Brief description of changes to existing functionality #IssueNumber

### Removed
- Brief description of removed features #IssueNumber
```

#### Categories

- **Added**: New features, capabilities, or controls
- **Fixed**: Bug fixes
- **Changed**: Changes to existing functionality
- **Removed**: Removed features or capabilities

#### Guidelines

- Use clear, concise descriptions
- Include issue/PR number when applicable (e.g., `#123`)
- Place entries in the most appropriate category
- If multiple categories apply, add entries to all relevant sections
- Maintain logical ordering within categories
- Focus on user-visible changes

#### Examples

```markdown
### Added
- PropertyGrid: Support for INotifyDataErrorInfo validation #226
- DataGrid: Sorting commands for programmatic sorting #124

### Fixed
- DataGrid: Fix null reference exception when sorting empty collection #240
- TreeListBox: Fix items being added under collapsed nodes #264

### Changed
- DataGrid: Commands moved to DataGridCommands class
- Tests: Updated to use NUnit constraint syntax #417
```

### 2. CONTRIBUTORS File

When adding a new contributor, update the `CONTRIBUTORS` file:

1. Add the person's name and email in alphabetical order by first name
2. Use the format: `Name <email address>`
3. Keep the list sorted

Example:
```
John Smith <john.smith@example.com>
```

**When to Update**: 
- When accepting a PR from a new contributor
- When a contributor makes their first significant contribution

### 3. README.md

Update README.md when:
- Adding a new control (add to the controls table)
- Changing supported frameworks
- Updating installation instructions
- Changing the project structure significantly

### 4. XML Documentation

Update XML documentation for:
- All new public APIs
- Modified public APIs
- Complex internal methods that need explanation

## Pull Request Checklist

Before submitting a pull request, ensure:

### Code Quality
- [ ] Code follows the established style guidelines
- [ ] All files include copyright headers
- [ ] Naming conventions are followed
- [ ] XML documentation is added for public APIs
- [ ] Code is compatible with both .NET 4.6.2 and .NET 8

### Testing
- [ ] New tests are added for new functionality
- [ ] Existing tests pass
- [ ] Tests follow the naming convention: `MethodName_StateUnderTest_ExpectedBehavior`
- [ ] Tests use NUnit constraint syntax (Assert.That)
- [ ] Test coverage meets the 80% goal for new code

### Documentation
- [ ] **CHANGELOG.md is updated** (CRITICAL - always required)
- [ ] CONTRIBUTORS file is updated (if adding new contributor)
- [ ] README.md is updated (if adding new controls or features)
- [ ] XML documentation is complete and accurate
- [ ] Demo applications are updated or added (if applicable)

### Building and Validation
- [ ] Solution builds without errors
- [ ] Solution builds without warnings (or warnings are documented)
- [ ] All tests pass
- [ ] Changes are tested on Windows

### Git
- [ ] Commit messages are clear and descriptive
- [ ] Branch is based on `develop`
- [ ] No unrelated changes are included
- [ ] No build artifacts or temporary files are committed

## Common Scenarios

### Adding a New Control

1. Create the control class in `PropertyTools.Wpf`
2. Add XAML resources if needed (themes, default styles)
3. Create a demo application in `Source/Examples/Controls/`
4. Write unit tests in `PropertyTools.Wpf.Tests`
5. Update README.md to include the control in the table
6. Update CHANGELOG.md under `### Added`
7. Add XML documentation for all public APIs

### Fixing a Bug

1. Create a failing test that reproduces the bug
2. Implement the fix with minimal changes
3. Verify the test passes
4. Run all tests to check for regressions
5. Update CHANGELOG.md under `### Fixed`
6. Document the fix in the PR description

### Adding a Helper Method

1. Add the method to the appropriate helper class
2. Include XML documentation
3. Write comprehensive unit tests
4. Update CHANGELOG.md if the method is public
5. Consider adding usage examples in comments

### Refactoring

1. Ensure all tests pass before starting
2. Make incremental, reviewable changes
3. Run tests after each significant change
4. Update documentation if APIs change
5. Update CHANGELOG.md under `### Changed` if user-visible

## Resources

- [CHANGELOG.md](CHANGELOG.md) - Project changelog
- [CONTRIBUTORS](CONTRIBUTORS) - List of contributors
- [README.md](README.md) - Project overview
- [LICENSE](LICENSE) - MIT License
- [GitHub Copilot Best Practices](https://docs.github.com/en/copilot/tutorials/coding-agent/get-the-best-results)
- [Roy Osherove's Test Naming Standards](https://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html)

## Questions?

If you're unsure about any aspect of contributing:
1. Check existing code for examples
2. Review recent pull requests for patterns
3. Look at the CHANGELOG.md for similar changes
4. Ask in the pull request or issue

## Remember

✅ **ALWAYS update CHANGELOG.md**  
✅ Include copyright headers  
✅ Write and run tests  
✅ Follow naming conventions  
✅ Document public APIs  
✅ Test with both target frameworks when possible  
✅ Keep changes minimal and focused
