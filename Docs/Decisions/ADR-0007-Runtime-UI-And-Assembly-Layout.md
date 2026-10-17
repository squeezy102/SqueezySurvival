# ADR-0007: Runtime UI technology and assembly layout

**Status:** Accepted
**Date:** 2026-10-16

## Context

The first runtime code is the main menu ([Startup Flow](../Design/UI/StartupFlow.md)). Two decisions are needed before it is written: which Unity UI system builds the game's interface, and how runtime code is split into assemblies ([Coding Standards: assemblies](../Engineering/CodingStandards.md#assemblies)).

The game is presented almost entirely through windows, lists, bars and text ([GDD: presentation](../Design/GameDesignDocument.md#presentation)).

## Decision

### UI: UI Toolkit

Runtime UI is built with UI Toolkit: layout in UXML, styling in USS, behavior in C#.

- UXML and USS are text files: reviewable in pull requests and diff-friendly, unlike uGUI prefabs.
- A web-like layout model suits a window- and list-heavy game.
- UI Toolkit handles runtime input without a uGUI EventSystem when it is the only UI system in a scene ([Unity Manual: runtime UI event system](https://docs.unity3d.com/6000.0/Documentation/Manual/UIE-Runtime-Event-System.html)).

### Assemblies

| Assembly | Folder | References | Contents |
|---|---|---|---|
| `SqueezySurvival.Core` | `Assets/SqueezySurvival/Core/` | None (`noEngineReferences`) | Game and screen logic in plain C#. Fully unit testable. |
| `SqueezySurvival.Presentation` | `Assets/SqueezySurvival/Presentation/` | Core, Unity | MonoBehaviours, UI Toolkit binding, composition roots. Kept thin. |
| `SqueezySurvival.Editor` | `Assets/SqueezySurvival/Editor/` | Core, Presentation | Editor tooling: release build, project setup commands. |
| `SqueezySurvival.Core.Tests` | `Assets/SqueezySurvival/Tests/Core/` | Core | Edit Mode tests for Core. |
| `SqueezySurvival.Editor.Tests` | `Assets/SqueezySurvival/Tests/Editor/` | Editor | Edit Mode tests for editor tooling. |

Core cannot reference Unity at all; the compiler enforces the simulation and presentation split ([Coding Standards: architecture](../Engineering/CodingStandards.md#architecture)). The launcher follows the same Core and App split ([ADR-0005](ADR-0005-Launcher-Technology.md)).

## Alternatives considered

- **uGUI.** Mature and widely documented. Unity's comparison recommends it where UI must be referenced easily from MonoBehaviours, and notes it is often used for UI driven by keyframed animation ([Unity Manual: comparison of UI systems](https://docs.unity3d.com/Manual/UI-system-compare.html)). Rejected because its layouts live in scenes and prefabs that are hard to review, and this game's UI is windows, lists and text rather than animated scenes.
- **One runtime assembly.** Simpler, but nothing would stop game logic from depending on Unity, defeating Edit Mode testability.

## Consequences

- Scenes and UI assets that must be created inside Unity are produced by editor setup commands, so they are reproducible and reviewable as code.
- UI Toolkit does not support in-scene authoring, serialized events, or Animation Clip and Timeline integration ([Unity Manual: comparison of UI systems](https://docs.unity3d.com/Manual/UI-system-compare.html)). When UI animation arrives ([GDD: presentation](../Design/GameDesignDocument.md#presentation)), it uses USS transitions or code, not keyframed clips.
