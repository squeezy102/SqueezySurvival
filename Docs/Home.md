# SqueezySurvival Wiki

The single source of truth for the design, engineering and decisions behind SqueezySurvival.

## Design
- [Game Design Document](Design/GameDesignDocument.md) - vision, pillars, core loop, MVP scope.
- [Open Questions](Design/OpenQuestions.md) - unresolved design decisions.
- [Launcher](Design/Launcher.md) - launcher layout, behavior and states.
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

1. **Docs first.** A system is designed and documented before it is implemented.
2. **Docs are part of done.** A change is not complete until every affected document is updated in the same commit.
3. **One home per fact.** Each rule, formula or value is defined in exactly one document and linked from elsewhere, never copied.
4. **Decisions are recorded.** Any significant or hard-to-reverse choice gets an ADR.
5. **Format.** Markdown (`.md`) only. Plain hyphens, no em dashes. Units are imperial where applicable.
6. **Status markers.** Design documents carry a status: `Concept`, `Draft`, `Approved`, or `Implemented`.

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
