# Custom GitHub Copilot Agents

This directory contains custom agent descriptions for GitHub Copilot to provide specialized assistance for the PropertyTools project.

## Available Agents

### Dependency Updater (`dependency-updater.md`)

A specialized agent for managing package dependencies and target frameworks.

**Use this agent when you need to:**
- Update NuGet package dependencies
- Migrate between .NET target framework versions
- Update multiple projects consistently across the solution
- Check for security vulnerabilities in dependencies
- Resolve dependency conflicts

**Key capabilities:**
- Understands PropertyTools' multi-targeting strategy (net462, net8.0-windows, net9.0-windows, netstandard2.0)
- Provides step-by-step guidance for safe updates
- Integrates security scanning with gh-advisory-database
- Maintains backwards compatibility
- Ensures all tests pass after updates

## How to Use Custom Agents

GitHub Copilot will automatically recognize and utilize these custom agents when working on related tasks. You can also explicitly invoke them by mentioning the type of work you need (e.g., "update the NuGet packages" or "migrate to .NET 9").

## Adding New Custom Agents

To add a new custom agent:

1. Create a new markdown file in this directory with a descriptive name
2. Follow the structure of existing agents
3. Include clear responsibilities, guidelines, and examples
4. Update this README with information about the new agent

## More Information

For more details about GitHub Copilot custom agents, see the [GitHub Copilot documentation](https://docs.github.com/en/copilot).
