# Changelog

All notable changes to this project are documented here.

Format: [Keep a Changelog](https://keepachangelog.com/en/1.1.0/). Versioning: [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Unity 6 (6000.4.2f1) Universal 2D project with URP 2D Renderer.
- Unity `.gitignore` and `.gitattributes` with Git LFS for binary assets.
- Project documentation structure: README, Game Design Document, Open Questions, Coding Standards, Source Control, Project Setup, Glossary, and Architecture Decision Records.

- Launcher (Avalonia, Velopack) with unit tests. See [Launcher](Docs/Design/Launcher.md).
- Release pipeline: `Tools/Publish-Release.cmd`. See [Release Pipeline](Docs/Engineering/ReleasePipeline.md).
- Developer scripts in `Tools/`.
- Startup flow: "Made with Unity" and placeholder "Pj's Gameroom" splash logos, and a main menu (New Game, Continue, Load Game, Options, Quit) with the version in the bottom right. See [Startup Flow](Docs/Design/UI/StartupFlow.md).
- New Game screen: required dominant hand choice (no default), run options with their point rules, and a live summary of what the run earns.
- `SqueezySurvival > Build > Build And Run Locally` editor command.
- `AGENTS.md`: the starting point and baseline for AI coding assistants, read by Claude Code through `CLAUDE.md`.
- `CODE_OF_CONDUCT.md`: community rules for Discussions, issues and pull requests.
- GitHub Discussions for ideas, questions, polls and announcements, with an Ideas form. See [Work Tracking](Docs/Engineering/WorkTracking.md#from-idea-to-work).
- Work tracking in GitHub Issues: issue forms for stories, bugs and tasks; labels; Wireframe and MVP milestones. See [Work Tracking](Docs/Engineering/WorkTracking.md).
- [Playtest Log](Docs/Engineering/PlaytestLog.md) recording every finding from playing builds and its resolution.
- Game screen wireframe: Day 1 at dawn on the Beach, game clock with day phases, Wait and Rest, event log, pause menu, and placeholders for unbuilt regions. See [Screen Layout](Docs/Design/UI/ScreenLayout.md#wireframe-stage).
- Runtime assemblies `SqueezySurvival.Core` and `SqueezySurvival.Presentation`. See [ADR-0007](Docs/Decisions/ADR-0007-Runtime-UI-And-Assembly-Layout.md).

### Removed
- Template packages outside MVP scope. See [ADR-0003](Docs/Decisions/ADR-0003-Package-Baseline.md).
