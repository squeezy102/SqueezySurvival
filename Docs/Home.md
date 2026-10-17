# SqueezySurvival Documentation

The single source of truth for the design, engineering and decisions behind SqueezySurvival.

## Design

- [Game Design Document](Design/GameDesignDocument.md) - vision, pillars, core loop, MVP scope.
- [Open Questions](Design/OpenQuestions.md) - design questions still waiting for an answer.
- [Question Log](Design/QuestionLog.md) - every design question in ID order, with its status and resolution.
- [Glossary](Glossary.md) - canonical game terms.

### Systems

| Group | Document | Covers |
|---|---|---|
| Foundation | [Time](Design/Systems/Time.md) | Time units, passing time, day phases. |
| | [Actions](Design/Systems/Actions.md) | Action anatomy, availability, light level, condition gates. |
| | [Runs](Design/Systems/Runs.md) | Death, points earned, casual, easy and hard modes, saving. |
| Survivor | [Survival Stats](Design/Systems/SurvivalStats.md) | Stat bars, bands, contentment and sanity, body temperature. |
| | [Food](Design/Systems/Food.md) | Eating and food tolerance. |
| | [Exertion](Design/Systems/Exertion.md) | Stamina, exertion risk, passing out. |
| | [Injuries and Health](Design/Systems/Injuries.md) | Two-layer health, body locations, injuries, treatment. |
| | [Skills](Design/Systems/Skills.md) | Skills identified so far. |
| World | [Zones](Design/Systems/Zones.md) | Island, biomes, zones, sub-zones, ground pool. |
| | [Exploration](Design/Systems/Exploration.md) | Progress, finds, milestones, scripted first explores. |
| | [Resource Nodes](Design/Systems/ResourceNodes.md) | Renewable and destructive harvesting, replenishment. |
| | [Encounters](Design/Systems/Encounters.md) | Wildlife encounters, fight or flight, ambushes. |
| | [Combat](Design/Systems/Combat.md) | Animal decks, duds, weapons, tells. |
| | [Research](Design/Systems/Research.md) | Passive research, queue, material-discovery unlocks, idle warning. |
| | [Weather](Design/Systems/Weather.md) | Weather states, seasons, wildfire. |
| Items | [Inventory](Design/Systems/Inventory.md) | Personal weight limit, load, carriers. |
| | [Equipment](Design/Systems/Equipment.md) | Slots, loadouts, treatment conflicts. |
| | [Decay](Design/Systems/Decay.md) | Item deterioration and storage. |

Planned: Crafting, Buildings, Light Sources, Illness.

### UI

- [Screen Layout](Design/UI/ScreenLayout.md) - main screen regions and organization tools.
- [HUD](Design/UI/HUD.md) - static status area, temperature, weather icons, event log.
- [Controls](Design/UI/Controls.md) - mouse and modifiers, hotkeys, remapping.
- [Startup Flow](Design/UI/StartupFlow.md) - splash logos, main menu, New Game screen and starting a run.
- [Grid Panels](Design/UI/GridPanels.md) - configurable scrollable grids for zone features and the ground pool.
- [Drag and Drop](Design/UI/DragAndDrop.md) - moving, reordering and combining things by dragging.
- [Options](Design/UI/Options.md) - the Options screen and where it opens from.
- [Day Summary](Design/UI/DaySummary.md) - the screen that marks the end of each day (concept).
- [Launcher](Design/Launcher.md) - launcher layout, behavior and states.

## Project

Files at the repository root:

- [README](../README.md) - what the game is and how to build it.
- [Getting Started](../GETTING_STARTED.md) - a plain-language guide for newcomers: words, setup, AI assistants, finding work.
- [CONTRIBUTING](../CONTRIBUTING.md) - how ideas, issues and changes are made.
- [Code of Conduct](../CODE_OF_CONDUCT.md) - rules for Discussions, issues and pull requests.
- [AGENTS.md](../AGENTS.md) - start here for AI assistants and LLM-assisted contributors.
- [CHANGELOG](../CHANGELOG.md) - notable changes, release by release.

## Engineering

- [Coding Standards](Engineering/CodingStandards.md) - testability, naming, architecture, warnings policy.
- [Testing and Quality Standards](Engineering/TestingStandards.md) - ISTQB-based test design, game testing, defect reports, definition of done.
- [Project Setup](Engineering/ProjectSetup.md) - editor version, packages, folder layout.
- [Source Control](Engineering/SourceControl.md) - branching, commits, Git LFS.
- [Release Pipeline](Engineering/ReleasePipeline.md) - building, packaging and publishing player builds.
- [Asset Credits](Engineering/AssetCredits.md) - source and license of every shipped asset.
- [Playtest Log](Engineering/PlaytestLog.md) - playtest sessions and the issues filed from them.
- [Work Tracking](Engineering/WorkTracking.md) - GitHub Issues for stories, bugs and tasks; labels, milestones, linking.
- Spikes - time-boxed experiments that answer a technical question:
  - [SPIKE-001: Velopack with a Unity player](Engineering/Spikes/SPIKE-001-Velopack-With-Unity.md)

## Decisions

- [Architecture Decision Records](Decisions/README.md) - why things are the way they are.

## Documentation policy

1. **Docs first.** A system is designed and documented before it is implemented. The project has moved from pure design into a playable wireframe: code is written only for systems that are already designed.
2. **Work is tracked in issues.** Every bug, request or playtest finding is filed as a GitHub issue before it is worked on ([Work Tracking](Engineering/WorkTracking.md)). Docs remain the single source of truth for design; issues link to them.
3. **Docs are part of done.** A change is not complete until every affected document is updated in the same commit.
4. **One home per fact.** Each rule, formula or value is defined in exactly one document and linked from elsewhere, never copied.
5. **Decisions are recorded.** Any significant or hard-to-reverse choice gets an ADR.
6. **Format.** Markdown (`.md`) only. Plain hyphens, no em dashes. Units are imperial where applicable.
7. **Status markers.** Design documents carry a status: `Concept`, `Draft`, `Approved`, or `Implemented`.
8. **Designs are working drafts.** System designs are introductory and expected to change as the game is built and played. Changes are made in the owning document and recorded in its history, never by working around it.

## Folder layout

| Folder | Holds |
|---|---|
| `Docs/Design/` | The GDD, open questions, question log, and the launcher design. |
| `Docs/Design/Systems/` | One document per game system. New systems are added here as they move from `Concept` to `Draft`. |
| `Docs/Design/UI/` | Screens, HUD and controls. |
| `Docs/Engineering/` | Standards, setup, source control, release pipeline. Spikes in `Spikes/SPIKE-NNN-*.md`. |
| `Docs/Decisions/` | ADRs as `ADR-NNNN-*.md`, indexed in `README.md`. |

The index above is the full list of documents; it is the one place they are listed.
