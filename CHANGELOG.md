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
- Explore: each explore takes 15 minutes, raises the zone's exploration progress, finds 1 to 3 loose items for the ground pool and sometimes reveals a zone feature, using placeholder Beach content. The game screen shows the zone's progress, features and ground pool. See [Exploration: wireframe stage](Docs/Design/Systems/Exploration.md#wireframe-stage).
- Grid panels: Zone features and On the ground are scrollable grids of placeholder icons with counts. Each panel's scroll direction, columns or rows, and scroll wheel inversion are set from a Settings button on the panel or from the new Options screen (main menu and pause menu), and saved between sessions. The scrollbar always keeps its place so cells never resize, and is enabled only when there is something to scroll, and hovering a cell shows a tooltip with the name, description and weights. See [Grid Panels](Docs/Design/UI/GridPanels.md) and [Options](Docs/Design/UI/Options.md).
- Drag and drop in grid panels: grids are fixed slots that can be left empty. While dragging, nothing moves: a copy of the icon follows the pointer, the target slot is outlined and a tooltip says what will happen. Letting go over an empty slot moves the thing there; over another thing's edge, the two swap; over its center, they combine (nothing combines until crafting exists). See [Drag and Drop](Docs/Design/UI/DragAndDrop.md).
- Design principle: nothing happens until you let go. See [GDD: design principles](Docs/Design/GameDesignDocument.md#design-principles).
- [Testing and Quality Standards](Docs/Engineering/TestingStandards.md) based on ISTQB CTFL v4.0 and CT-GaMe, binding on contributors and AI assistants. The bug form adds severity, environment and design reference.
- Combat presentation follows Pokémon and EarthBound instead of Final Fantasy: the survivor is never shown, and text boxes announce each action and wait for the player. See [Combat: presentation](Docs/Design/Systems/Combat.md#presentation).
- Design: settings belong to each run, can be captured as a global preference, and every settings menu can return to defaults ([Options](Docs/Design/UI/Options.md#where-settings-come-from)); built with saving in #46.
- [Getting Started](GETTING_STARTED.md): a plain-language guide for contributors new to git and code.
- Runtime assemblies `SqueezySurvival.Core` and `SqueezySurvival.Presentation`. See [ADR-0007](Docs/Decisions/ADR-0007-Runtime-UI-And-Assembly-Layout.md).

### Removed
- Template packages outside MVP scope. See [ADR-0003](Docs/Decisions/ADR-0003-Package-Baseline.md).
