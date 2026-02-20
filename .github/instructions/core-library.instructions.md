---
applyTo: "Source/PropertyTools/**/*.cs"
excludeAgent: "code-review"
---

# Core Library Instructions

When working with the core PropertyTools library (non-WPF):

## Target Frameworks
This core library targets:
- .NET Framework 4.6.2
- .NET Standard 2.0

Ensure code is compatible with both frameworks.

## Namespace
Use `PropertyTools` namespace for core/shared code (not `PropertyTools.Wpf`).

## Dependencies
- **No dependencies outside System.* libraries**
- Keep dependencies minimal - only use built-in .NET framework libraries
- Avoid platform-specific dependencies
- This library shall be WPF-agnostic

## Best Practices
- Write portable, cross-platform compatible code
- Use framework-agnostic APIs
- Include XML documentation for all public APIs
- Follow .NET Standard guidelines
