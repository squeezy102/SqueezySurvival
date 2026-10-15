# Project Setup

**Status:** Implemented

## Editor

| Item | Value |
|---|---|
| Unity Editor | 6000.4.2f1 |
| Template | Universal 2D |
| Render pipeline | URP with 2D Renderer (`Assets/Settings/Renderer2D.asset`) |
| Default editor mode | 2D |

## Packages

Package choices and rationale: [ADR-0003](../Decisions/ADR-0003-Package-Baseline.md). The authoritative list is `Packages/manifest.json`.

## Folder layout

| Path | Purpose | Versioned |
|---|---|---|
| `Assets/` | Game content and code | Yes |
| `Packages/` | Package manifest and lock file | Yes |
| `ProjectSettings/` | Unity project settings | Yes |
| `Docs/` | Project documentation | Yes |
| `Library/`, `Temp/`, `Logs/`, `UserSettings/` | Generated locally by Unity | No |
| `*.sln`, `*.csproj` | Generated IDE files | No |

The layout inside `Assets/` is defined when the first code and content are added.

## First-time setup

1. Install Unity Hub and editor 6000.4.2f1.
2. Install Git and Git LFS. Run `git lfs install` once per machine.
3. Clone the repository and open the folder in Unity Hub.
