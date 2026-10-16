# SqueezySurvival Wiki

The single source of truth for the design, engineering and decisions behind SqueezySurvival.

## Design
- [Game Design Document](Design/GameDesignDocument.md) - vision, pillars, core loop, MVP scope.
- [Open Questions](Design/OpenQuestions.md) - unresolved design decisions.
- [Launcher](Design/Launcher.md) - launcher layout, behavior and states.
- Systems:
  - [Time](Design/Systems/Time.md) - time units, passing time, day phases.
  - [Actions](Design/Systems/Actions.md) - action anatomy, availability, light level, condition gates.
  - [Survival Stats](Design/Systems/SurvivalStats.md) - macros, bar conventions, bands.
  - [Food](Design/Systems/Food.md) - eating and per-food tolerance.
  - [Exploration](Design/Systems/Exploration.md) - progress, finds, milestones, scripted first explores.
  - [Resource Nodes](Design/Systems/ResourceNodes.md) - renewable and destructive harvesting, replenishment.
  - [Decay](Design/Systems/Decay.md) - item deterioration as a resource sink.
  - [Ground Pool](Design/Systems/GroundPool.md) - items on the ground in a zone.
  - [Inventory](Design/Systems/Inventory.md) - personal weight limit and carriers.
  - [Exertion](Design/Systems/Exertion.md) - stamina, exertion risk, injury, passing out.
- UI:
  - [HUD](Design/UI/HUD.md) - static status area, temperature, weather icons, event log.
  - [Controls](Design/UI/Controls.md) - mouse and modifiers, hotkeys, remapping.
  - [Screen Layout](Design/UI/ScreenLayout.md) - main screen regions and organization tools.
- [Glossary](Glossary.md) - canonical game terms.

## Engineering
- [Coding Standards](Engineering/CodingStandards.md) - testability, naming, architecture, warnings policy.
- [Project Setup](Engineering/ProjectSetup.md) - editor version, packages, folder layout.
- [Source Control](Engineering/SourceControl.md) - branching, commits, Git LFS.
- [Release Pipeline](Engineering/ReleasePipeline.md) - building, packaging and publishing player builds.
- Spikes - time-boxed experiments that answer a technical question:
  - [SPIKE-001: Velopack with a Unity player](Engineering/Spikes/SPIKE-001-Velopack-With-Unity.md)

## Decisions
- [Architecture Decision Records](Decisions/README.md) - why things are the way they are.

## Documentation policy

1. **Docs first.** A system is designed and documented before it is implemented. The project is currently in the design phase: no gameplay code until the core designs are concrete.
2. **Docs are part of done.** A change is not complete until every affected document is updated in the same commit.
3. **One home per fact.** Each rule, formula or value is defined in exactly one document and linked from elsewhere, never copied.
4. **Decisions are recorded.** Any significant or hard-to-reverse choice gets an ADR.
5. **Format.** Markdown (`.md`) only. Plain hyphens, no em dashes. Units are imperial where applicable.
6. **Status markers.** Design documents carry a status: `Concept`, `Draft`, `Approved`, or `Implemented`.
7. **Designs are working drafts.** System designs are introductory and expected to change as the game is built and played. Changes are made in the owning document and recorded in its history, never by working around it.

## Document map

```
README.md                  Project overview and quick start
CHANGELOG.md               Release history
Docs/
  Home.md                  This page
  Glossary.md
  Design/
    GameDesignDocument.md
    Launcher.md
    Systems/
      Time.md
      Actions.md
      SurvivalStats.md
      Food.md
      Exploration.md
      ResourceNodes.md
      Decay.md
      GroundPool.md
      Inventory.md
      Exertion.md
    UI/
      HUD.md
      Controls.md
      ScreenLayout.md
    OpenQuestions.md
  Engineering/
    CodingStandards.md
    ProjectSetup.md
    SourceControl.md
    ReleasePipeline.md
    Spikes/
      SPIKE-NNN-*.md
  Decisions/
    README.md              ADR index and template
    ADR-NNNN-*.md
```

System design documents (for example `Design/Systems/Time.md`) are added under `Design/Systems/` as each system moves from `Concept` to `Draft`.
