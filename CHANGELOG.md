# Changelog

All notable changes to this project are documented here.

Format: [Keep a Changelog](https://keepachangelog.com/en/1.1.0/). Versioning: [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.1.1] - 2026-10-20

### Fixed
- Text could run past its container, as "No game in progress" did on the main menu. Text now wraps everywhere and buttons grow to fit it (#67). See [Screen Layout: text containment](Docs/Design/UI/ScreenLayout.md#text-containment).
- Text is set in two pixel fonts instead of Pixelify Sans, whose 5 and S, 2 and Z, and 8 and B were hard to tell apart (#60): Press Start 2P for buttons, titles, headings and the time, and the more compact Jersey 15 for everything else. See [Art Direction: font](Docs/Design/ArtDirection.md#font).

### Changed
- `.gitignore` ignores an `offline/` folder at the repository root, for the owner's untracked notes.

## [0.1.0] - 2026-10-17

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
- Inventory grids, like Diablo's inventory: On the ground and Zone features are fixed 16 x 4 grids of placeholder icons with counts; the inventory shows 8 slots plus the tool hand and offhand, set apart. Grids never scroll. When the ground is full, the game asks whether to leave a new find behind or discard something to make room, both gone for good. Sort packs a grid by type (item category), with alphabetical, quantity and weight in the menu beside it. Tooltips show the category. See [Inventory Grids](Docs/Design/UI/InventoryGrids.md).
- Drag and drop in inventory grids: while dragging, nothing moves: a copy of the icon follows the pointer, an outline shows where it would go and a tooltip says what will happen. Letting go over an empty slot moves the thing there; over another thing, the two swap; held over another thing, or over its own stack, for a second, they combine if they can, and if they can't, the thing goes back and a brief message says why. Escape or letting go elsewhere cancels. See [Drag and Drop](Docs/Design/UI/DragAndDrop.md).
- Art direction: icons are 16 x 16 pixel art with top-left light and colored outlines; panels are see-through navy glass with gold trim, with blue buttons, sunken slots and pale tooltips, in the spirit of Chrono Trigger; the beach backdrop is pixel art painted by the game. See [Art Direction](Docs/Design/ArtDirection.md).
- Nav bar: icon buttons (a map, a hammer and anvil, a book, a vial and atom, a crossed sword and pickaxe, a steaming pot) with tooltips, and Options as a gear at the bottom. The bar is narrower. See [Screen Layout](Docs/Design/UI/ScreenLayout.md).
- Item tooltips show the item's icon. Clicking an item selects it and keeps its tooltip open until the next click elsewhere, drag or Escape. See [Inventory Grids](Docs/Design/UI/InventoryGrids.md#selecting).
- Day and night dial in the HUD: half day sky with the sun, half night sky with the moon and stars, turning with the clock. See [Time: display](Docs/Design/Systems/Time.md#display).
- Interface text is set in Pixelify Sans, a pixel font under the SIL Open Font License. See [Art Direction: font](Docs/Design/ArtDirection.md#font).
- The hand slots are set apart from the inventory by a bold gold rule and a wide gap, and an empty hand slot shows a faint left or right hand. See [Inventory Grids](Docs/Design/UI/InventoryGrids.md#the-grids).
- The HUD clock shows the day, the time and the day phase on three rows. See [Time: display](Docs/Design/Systems/Time.md#display).
- Design principle: nothing happens until you let go. See [GDD: design principles](Docs/Design/GameDesignDocument.md#design-principles).
- [Testing and Quality Standards](Docs/Engineering/TestingStandards.md) based on ISTQB CTFL v4.0 and CT-GaMe, binding on contributors and AI assistants. The bug form adds severity, environment and design reference.
- Combat presentation follows Pokémon and EarthBound instead of Final Fantasy: the survivor is never shown, and text boxes announce each action and wait for the player. See [Combat: presentation](Docs/Design/Systems/Combat.md#presentation).
- Settings rules designed: a run starts from the player's global preference and keeps its own settings; every settings menu will have Return to default settings and Capture these settings as my global preference ([Options](Docs/Design/UI/Options.md#where-settings-come-from)). They are built with the first settings (#46). Options opens from the main menu, the pause menu and the nav bar.
- [Getting Started](GETTING_STARTED.md): a plain-language guide for contributors new to git and code.
- Runtime assemblies `SqueezySurvival.Core` and `SqueezySurvival.Presentation`. See [ADR-0007](Docs/Decisions/ADR-0007-Runtime-UI-And-Assembly-Layout.md).

### Removed
- Template packages outside MVP scope. See [ADR-0003](Docs/Decisions/ADR-0003-Package-Baseline.md).
