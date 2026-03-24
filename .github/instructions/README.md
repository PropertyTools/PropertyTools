# Path-Specific Copilot Instructions

This directory contains path-specific custom instructions for GitHub Copilot. These instructions are automatically applied when Copilot is working with files matching the specified patterns.

## Available Instructions

### `tests.instructions.md`
**Applies to:** `**/*Tests.cs`, `**/*Test.cs`

Provides specific guidance for writing and organizing unit tests:
- NUnit test framework conventions
- Test naming patterns (`MethodName_StateUnderTest_ExpectedBehavior`)
- AAA (Arrange-Act-Assert) pattern
- Coverage goals and best practices

### `examples.instructions.md`
**Applies to:** `Source/Examples/**/*.cs`, `Source/Examples/**/*.xaml`

Guidelines for creating demo applications:
- MVVM pattern requirements
- Project structure standards
- Documentation expectations
- Best practices for educational examples

### `wpf-controls.instructions.md`
**Applies to:** `Source/PropertyTools.Wpf/**/*.cs`, `Source/PropertyTools.Wpf.ExtendedToolkit/**/*.cs`

WPF-specific control development guidelines:
- Dependency property patterns
- Event handling best practices
- Multi-targeting considerations
- Accessibility and performance guidelines

### `core-library.instructions.md`
**Applies to:** `Source/PropertyTools/**/*.cs`
**Excluded from:** Code review agent

Core library development standards:
- .NET Standard 2.0 compatibility requirements
- Platform-agnostic code patterns
- Minimal dependency principles

## How Path-Specific Instructions Work

When Copilot is working on a file:
1. The `applyTo` glob pattern determines which files use these instructions
2. Instructions are combined with repository-wide instructions from `.github/copilot-instructions.md`
3. Both sets of instructions are provided to Copilot for context

## Adding New Instructions

To add new path-specific instructions:

1. Create a new `NAME.instructions.md` file in this directory
2. Add frontmatter with `applyTo` glob pattern:
   ```markdown
   ---
   applyTo: "path/pattern/**/*.ext"
   ---
   ```
3. Write your instructions in natural language using Markdown
4. Optionally add `excludeAgent: "code-review"` or `excludeAgent: "coding-agent"` to exclude specific agents

## Glob Pattern Examples

- `*.py` - All `.py` files in current directory
- `**/*.py` - All `.py` files recursively
- `src/**/*.ts` - All `.ts` files under `src/` directory
- `**/test/**/*.js` - All `.js` files in any `test` directory

## Related Documentation

- [Main Copilot Instructions](../copilot-instructions.md) - Repository-wide instructions
- [Agent Guidelines](../../AGENTS.md) - Comprehensive coding guidelines
- [Claude Instructions](../../CLAUDE.md) - Claude-specific instructions
- [Custom Agents](../agents/) - Specialized agent definitions

## References

- [GitHub Copilot Custom Instructions Documentation](https://docs.github.com/en/copilot/customizing-copilot/adding-custom-instructions-for-github-copilot)
