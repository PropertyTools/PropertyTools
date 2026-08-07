# Claude AI Agent Instructions for PropertyTools

This is Claude Code's project entry point. It is deliberately thin: **[AGENTS.md](AGENTS.md) is the
canonical, detailed guide** (project structure, naming conventions, how to write tests, how to
implement demos, the full PR checklist) — read it before making changes. This file only exists
because Claude Code loads `CLAUDE.md` automatically; GitHub Copilot instead reads
[.github/copilot-instructions.md](.github/copilot-instructions.md). Keep all three in sync — if you
change a fact in one (target frameworks, build commands, changelog rules), update the others too.

## The three things you must not forget

🔴 **Every PR updates `CHANGELOG.md`** — add an entry to `## Unreleased`, under `### Added` /
`### Fixed` / `### Changed` / `### Removed`, ending with `#IssueNumber`.

🟡 **Tests use NUnit constraint syntax** (`Assert.That(result, Is.EqualTo(expected))`) and the
`MethodName_StateUnderTest_ExpectedBehavior` naming pattern, in `PropertyTools.Wpf.Tests`, targeting
80%+ coverage of new code.

🟢 **This is a WPF library** (.NET Framework 4.6.2, .NET 8/9/10 - Windows) — building, running, and
`dotnet test` all require Windows. On Linux, `dotnet build -p:EnableWindowsTargeting=true` verifies
compilation only (details, including its limits: [AGENTS.md#building-on-linux](AGENTS.md#building-on-linux-compile-time-verification-only)).

## References

- [AGENTS.md](AGENTS.md) — full coding agent guidelines (read this first)
- [CHANGELOG.md](CHANGELOG.md), [README.md](README.md), [CONTRIBUTORS](CONTRIBUTORS)
- [.github/copilot-instructions.md](.github/copilot-instructions.md) — GitHub Copilot's equivalent of this file
