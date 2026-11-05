# AGENTS.md - start here

Instructions for any AI coding assistant (Claude, Gemini, ChatGPT, Copilot, Cursor or other) working in this repository, and the baseline for contributors who work with one. Read this file first, before changing anything. It is a prime deliverable, maintained by Claude and kept current in the same pull request as any convention it describes.

## The project

**SqueezySurvival**: a graphically basic, strategically deep, single-player survival game in Unity. The survivor wakes on a beach; every action costs time; discovery drives progression. Windows first, kept portable. Status: playable wireframe (menus, New Game, first game screen).

The owner, **squeezy102** (Pj), has the final say on every change.

## Read before working

| Read | For |
|---|---|
| [Docs/Home.md](Docs/Home.md) | Index of all documentation and the documentation policy |
| [Docs/Design/GameDesignDocument.md](Docs/Design/GameDesignDocument.md) | Vision, pillars, design principles |
| [Docs/Engineering/CodingStandards.md](Docs/Engineering/CodingStandards.md) | Code rules |
| [Docs/Engineering/Architecture.md](Docs/Engineering/Architecture.md) | How the runtime is organized: services, time, events, composition, element lifetime |
| [Docs/Engineering/TestingStandards.md](Docs/Engineering/TestingStandards.md) | Test design (ISTQB), game testing, definition of done |
| [Docs/Engineering/WorkTracking.md](Docs/Engineering/WorkTracking.md) | Issues, labels, lifecycle |
| [Docs/Engineering/SourceControl.md](Docs/Engineering/SourceControl.md) | Branches, commits, protected branches |
| [CONTRIBUTING.md](CONTRIBUTING.md) | How changes are proposed and approved |

Then read the design document for the system you are touching (`Docs/Design/Systems/`, `Docs/Design/UI/`).

## Non-negotiable rules

1. **Docs are the single source of truth.** Design, rules and decisions live in `Docs/`, version-controlled and approved through pull requests. No wiki. Ideas are discussed in GitHub Discussions and work is tracked in issues, but neither is a source of truth. If the docs and the code disagree, the docs win until the docs are changed.
2. **Docs first.** Design and document a system before writing code for it. Code is written only for systems that are already designed. A change is not done until every affected document is updated in the same pull request.
3. **One home per fact.** Define each rule or value in exactly one document and link to it elsewhere. Never copy.
4. **Never guess.** Ground every technical decision in official product documentation or established precedent (Unity, Microsoft and .NET docs, GitHub docs, ISTQB syllabi for testing, reputable forums), and cite it. No "try it and see", no hacks or workarounds.
5. **Report first.** Every bug, request or playtest finding is filed as a GitHub issue before it is worked on, even if the fix takes minutes.
6. **Nothing reaches a protected branch without approval.** `main` requires a pull request approved by squeezy102; `PjDev` belongs to squeezy102 alone. Never push to either.
7. **Unit tests are mandatory.** All game logic is unit tested, with tests designed to the [Testing and Quality Standards](Docs/Engineering/TestingStandards.md) (ISTQB): every test can fail for a real defect. Zero compiler and Console warnings: fix them, or suppress at the narrowest scope with a comment and a changelog entry.

## Architecture

| Assembly | Folder | Rule |
|---|---|---|
| `SqueezySurvival.Core` | `Assets/SqueezySurvival/Core/` | Plain C#, **no Unity references** (compiler-enforced). All game and screen logic lives here. |
| `SqueezySurvival.Presentation` | `Assets/SqueezySurvival/Presentation/` | One composition root (a MonoBehaviour) per scene, and thin presenters that bind UI to Core. No game rules. |
| `SqueezySurvival.Editor` | `Assets/SqueezySurvival/Editor/` | Build and setup commands; `Editor/Diagnostics/` writes compile and test results to `Diagnostics/`. |
| `*.Tests` | `Assets/SqueezySurvival/Tests/` | Edit Mode tests (NUnit). |

- **UI:** UI Toolkit. Layout in UXML, style in USS, behavior in C# ([ADR-0007](Docs/Decisions/ADR-0007-Runtime-UI-And-Assembly-Layout.md)).
- **Dependency injection:** constructor injection; services and volatile dependencies as interfaces, rules and data as plain instances ([Coding Standards: architecture](Docs/Engineering/CodingStandards.md#architecture)); one composition root per scene. No global singletons for game state.
- **Scenes are generated:** scenes and UI assets are created by commands under the **SqueezySurvival > Setup** menu, never edited by hand ([Project Setup](Docs/Engineering/ProjectSetup.md#setup-commands)).
- **Content is data:** items, zone features and zones are JSON files in `Assets/SqueezySurvival/Content/`, the one home of their values ([ADR-0009](Docs/Decisions/ADR-0009-Data-Driven-Content.md)). Adding one is a data change, plus a placeholder icon until real art exists; after adding a zone file, run **SqueezySurvival > Setup > Configure Game Screen** so the game scene references it. New record types go under `SqueezySurvival.Core.Content.Records`, which `link.xml` keeps in builds. Tests build their own definitions and do not depend on the shipped files.
- **Launcher:** a separate .NET solution in `Launcher/` with the same Core and App split ([ADR-0005](Docs/Decisions/ADR-0005-Launcher-Technology.md)).

## Code conventions

- Names say what a thing is and what it is for: `MainMenuOptionStateEvaluator`, not `MenuHelper`. No abbreviations.
- XML doc comments on public members. Inline comments explain why, never what. No commented-out code; no TODO, FIXME or HACK.
- Test names describe behavior: `PassTime_Wait_AdvancesFifteenMinutesAndLogs`. Arrange, act, assert. Design cases with equivalence partitions and boundary values ([Testing and Quality Standards](Docs/Engineering/TestingStandards.md)).

## Writing conventions

Applies to docs, UI text, commit messages and issues.

- Use a single hyphen ( - ), never an em dash.
- Imperial units wherever units apply.
- The survivor has no gender. Game text says "you" or "the survivor".
- Event log voice: second person, past tense; what happened, then how it felt.
- Design questions: never edit a question's wording. If an answer settles only part of one, resolve it and open a new question for the rest ([Question Log](Docs/Design/QuestionLog.md)).

## Workflow

1. Find or file the issue ([Work Tracking](Docs/Engineering/WorkTracking.md)). Assign yourself and add `status: in progress`.
2. Branch from your own stage branch, never from `main` or another collaborator's stage branch ([Source Control: branching](Docs/Engineering/SourceControl.md#branching); squeezy102's is `PjDev`, and a contributor cuts theirs from `main` once): `feature/3-explore-beach`, `fix/32-event-log-scroll`, `docs/...`. Nobody works on `main`.
3. Commit with an imperative subject of 72 characters or fewer; reference the issue (`Refs #3`). Update `CHANGELOG.md` under Unreleased.
4. Merge finished work back into the same stage branch.
5. When the stage branch holds tested work ready for `main`, open a pull request from it into `main` with the template, listing `Closes #N` for each finished issue. squeezy102 reviews, approves and merges it ([Source Control: branching](Docs/Engineering/SourceControl.md#branching)).

## Unity specifics

- Unity **6000.4.2f1**, Universal 2D (URP 2D Renderer). Open the project with exactly this version.
- Run tests: **Window > General > Test Runner > EditMode > Run All**.
- See built-only features (splash logos): **SqueezySurvival > Build > Build And Run Locally**.
- Builds change two `ProjectSettings` files; revert them before committing ([#10](https://github.com/squeezy102/SqueezySurvival/issues/10)).
- Binary assets use Git LFS. Credit every asset in [Asset Credits](Docs/Engineering/AssetCredits.md).
- Do not add packages without an ADR ([ADR-0003](Docs/Decisions/ADR-0003-Package-Baseline.md)).

## Never

- Commit secrets. The release token lives only in the owner's user environment.
- Edit generated scenes or `ProjectSettings` by hand when a setup command owns them.
- Write code for a system that has not been designed.
- Use the GitHub wiki.

## For AI assistants

- Claude Code reads this file through [CLAUDE.md](CLAUDE.md). For other tools, point the tool's project instructions at `AGENTS.md`.
- Before proposing anything, read the design document involved and the open questions for that area ([Open Questions](Docs/Design/OpenQuestions.md)). Do not answer an open question yourself; propose options and let squeezy102 decide.
- **Preflight, before any work.** A session must be able to read and write this repository's issues (create, assign, label, comment, close), because every change needs an issue ([rule 5](#non-negotiable-rules)) and its lifecycle is part of the work. Check with `gh api repos/squeezy102/SqueezySurvival/issues/1`, and confirm the .NET SDK if the sandbox has no Unity (below). If the check answers that GitHub access is not enabled for the session, attach the repository with push access (`add_repo` with `access: "push"`). The attach is denied unless squeezy102 has approved it explicitly in the conversation, so ask him first, then retry. If issue access is still missing, stop and tell squeezy102 what to enable. Never skip the issue, the branch or the documentation because a change looks small.
- **Work in squeezy102's local repository, never in a cloud copy.** He tests in Unity before anything is committed. If the conversation is not linked to the repository folder on his PC, say so and ask him to add it before changing any code. Edit the files there and leave them uncommitted until he has tested them and says to commit. He never runs git: you do, there, including pulling. Follow [Source Control: working on the owner's PC](Docs/Engineering/SourceControl.md#working-on-the-owners-pc), starting by asking for delete permission for the repository folder.
- **Git on the PC cannot push.** It has no GitHub credentials; push a commit through a bundle as described in [Source Control: committing from a sandbox](Docs/Engineering/SourceControl.md#committing-from-a-sandbox).
- **No Unity in your sandbox?** Install the .NET SDK at the start of every session and compile Core and its tests before handing work back ([Testing and Quality Standards: checking Core without Unity](Docs/Engineering/TestingStandards.md#checking-core-without-unity)).
- **Read results from files, not screenshots.** After every compile and test run, Unity writes `Diagnostics/latest-compilation.md`, `Diagnostics/latest-test-summary.md` and `Diagnostics/latest-test-results.xml` at the project root. Read them to see what failed and why; check their timestamps ([Testing and Quality Standards: reading results without Unity](Docs/Engineering/TestingStandards.md#reading-results-without-unity)).
- State the source for every technical claim.
- Keep responses concise and work one step at a time with the person you are helping.
