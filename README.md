# SqueezySurvival

A graphically basic, strategically deep, single-player survival game built in Unity. You wake up stranded on a beach on a large island. Every action costs time, and everything you find opens new possibilities.

> Working title. Status: pre-production, design phase. The distribution pipeline is in place; no gameplay code is written until the core designs are concrete.

## Play

Player builds are published to the public [SqueezySurvival-Releases](https://github.com/squeezy102/SqueezySurvival-Releases) repository. Windows only at this stage. Distribution design: [ADR-0004](Docs/Decisions/ADR-0004-Distribution-And-Self-Update.md).

## Documentation

All project documentation lives in [`Docs/`](Docs/Home.md). Start at **[Docs/Home.md](Docs/Home.md)**.

| Area | Entry point |
|---|---|
| Game design | [Game Design Document](Docs/Design/GameDesignDocument.md) |
| Design questions | [Open Questions](Docs/Design/OpenQuestions.md) |
| Engineering standards | [Coding Standards](Docs/Engineering/CodingStandards.md) |
| Decisions | [Architecture Decision Records](Docs/Decisions/README.md) |
| Changes | [CHANGELOG](CHANGELOG.md) |

## Development setup

| Requirement | Version |
|---|---|
| Unity Editor | 6000.4.2f1 |
| Render pipeline | URP, 2D Renderer |
| Source control | Git with Git LFS |

1. Install Git and Git LFS, then run `git lfs install` once.
2. Clone the repository.
3. Open the project folder in Unity Hub with the editor version above.

Full details: [Project Setup](Docs/Engineering/ProjectSetup.md).

## License

All rights reserved. No license is granted at this time.
