# Claude AI Agent Instructions for PropertyTools

This document provides specific instructions for Claude AI when working on the PropertyTools repository.

## Quick Start

**👉 Read [AGENTS.md](AGENTS.md) for comprehensive coding guidelines.**

The AGENTS.md file contains detailed information about:
- Project structure and components
- Code style and naming conventions
- Building and testing procedures
- Test coverage requirements and how to write tests
- How to implement demo applications
- Required documentation updates (CHANGELOG.md, CONTRIBUTORS, README.md)
- Pull request checklist

## Core Requirements Summary

### 1. CHANGELOG.md Updates (CRITICAL)

**Every PR must update CHANGELOG.md.** Add entries to the `## Unreleased` section under the appropriate category (Added, Fixed, Changed, Removed).

### 2. Code Style

- Use copyright headers in all files
- Follow naming conventions (PascalCase for classes/methods, camelCase for fields/variables)
- Test methods: `MethodName_StateUnderTest_ExpectedBehavior`
- Use NUnit constraint syntax: `Assert.That(result, Is.EqualTo(expected))`

### 3. Testing Requirements

- Add tests for all new functionality
- Target 80% or higher coverage for new code
- Tests must pass before submitting PR
- Use the test structure in `PropertyTools.Wpf.Tests`

### 4. Documentation

- Add XML documentation for public APIs
- Update README.md for new controls
- Update CONTRIBUTORS for new contributors

## Project Specifics

- **Framework**: WPF (Windows Presentation Foundation)
- **Target**: .NET Framework 4.6.2 and .NET 8 - Windows
- **Test Framework**: NUnit
- **Build**: Requires Windows (WPF dependency)
- **Development Branch**: `develop`

## Before You Start

1. ✅ Read [AGENTS.md](AGENTS.md) thoroughly
2. ✅ Review the [Pull Request Checklist](AGENTS.md#pull-request-checklist)
3. ✅ Understand the [test requirements](AGENTS.md#test-coverage-requirements)
4. ✅ Know how to [write tests](AGENTS.md#how-to-write-tests)
5. ✅ Understand [demo implementation](AGENTS.md#how-to-implement-demos)

## Common Commands

### Building
```bash
dotnet restore Source/PropertyTools.sln
dotnet build Source/PropertyTools.sln --configuration Release
```

### Testing
```bash
dotnet test Source/PropertyTools.sln
```

## Key Files to Update

For every change, consider:
- ✅ **CHANGELOG.md** - Always required
- ✅ **Source code** - Your actual changes
- ✅ **Tests** - Add or update tests
- ⚠️ **CONTRIBUTORS** - If new contributor
- ⚠️ **README.md** - If new control or major feature

## References

- [AGENTS.md](AGENTS.md) - Complete coding agent guidelines (READ THIS FIRST)
- [CHANGELOG.md](CHANGELOG.md) - Project changelog
- [README.md](README.md) - Project overview
- [.github/copilot-instructions.md](.github/copilot-instructions.md) - GitHub Copilot specific instructions

## Remember

🔴 **CRITICAL**: Update CHANGELOG.md in every PR  
🟡 **IMPORTANT**: Include tests for new functionality  
🟢 **HELPFUL**: Follow existing patterns in the codebase  

---

**For detailed information, see [AGENTS.md](AGENTS.md)**
