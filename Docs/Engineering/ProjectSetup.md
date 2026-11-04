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
| `Launcher/` | Launcher solution ([ADR-0005](../Decisions/ADR-0005-Launcher-Technology.md)) | Yes |
| `Launcher/**/bin/`, `Launcher/**/obj/` | Launcher build output | No |
| `Tools/` | Developer machine scripts | Yes |
| `Tools/Output/` | Script logs, local only | No |
| `Library/`, `Temp/`, `Logs/`, `UserSettings/` | Generated locally by Unity | No |
| `*.sln`, `*.csproj` | Generated IDE files | No |
| `Diagnostics/` | The last compile's and test run's results, written by the editor ([Testing Standards: reading results without Unity](TestingStandards.md#reading-results-without-unity)) | No |

### Inside `Assets/`

| Path | Contents | Assembly |
|---|---|---|
| `Assets/SqueezySurvival/Core/` | Game and screen logic, no Unity references | `SqueezySurvival.Core` |
| `Assets/SqueezySurvival/Presentation/` | MonoBehaviours and UI binding; the content reader | `SqueezySurvival.Presentation` |
| `Assets/SqueezySurvival/Content/` | Game content as JSON text assets: `Items.json`, `ZoneFeatures.json` and one file per zone under `Zones/` ([ADR-0009](../Decisions/ADR-0009-Data-Driven-Content.md)) | - |
| `Assets/SqueezySurvival/Editor/` | Editor-only tooling: release build, setup commands | `SqueezySurvival.Editor` |
| `Assets/SqueezySurvival/Editor/Diagnostics/` | Writes compile and test results to `Diagnostics/` | `SqueezySurvival.Editor.Diagnostics` |
| `Assets/SqueezySurvival/Tests/Core/` | Edit Mode tests for Core | `SqueezySurvival.Core.Tests` |
| `Assets/SqueezySurvival/Tests/Editor/` | Edit Mode tests for editor tooling | `SqueezySurvival.Editor.Tests` |
| `Assets/SqueezySurvival/Tests/Presentation/` | Edit Mode tests for Presentation, against the real UI layouts ([ADR-0008](../Decisions/ADR-0008-Presentation-Testing.md)) | `SqueezySurvival.Presentation.Tests` |
| `Assets/SqueezySurvival/UI/` | UXML layouts, USS styles, theme and panel settings | - |
| `Assets/SqueezySurvival/Art/` | Sprites and textures ([Asset Credits](AssetCredits.md)) | - |
| `Assets/SqueezySurvival/Scenes/` | Scenes | - |
| `Assets/SqueezySurvival/link.xml` | Keeps the content record types from code stripping in builds ([ADR-0009](../Decisions/ADR-0009-Data-Driven-Content.md)) | - |

All project code lives under `Assets/SqueezySurvival/` in assembly definitions, never in the default `Assembly-CSharp`. Assembly layout: [ADR-0007](../Decisions/ADR-0007-Runtime-UI-And-Assembly-Layout.md).

## Setup commands

Scenes and assets that must be created inside Unity are made by commands under the **SqueezySurvival** menu, so they are reproducible.

| Command | Does |
|---|---|
| SqueezySurvival > Setup > Configure Startup Screens | Imports the placeholder art, sets the splash logos, creates the shared panel settings and the main menu scene, and puts the scene first in the build. Safe to run again. |
| SqueezySurvival > Setup > Configure Game Screen | Creates the game scene, gives its composition root the content files (`Items.json`, `ZoneFeatures.json` and every file under `Content/Zones/`), and adds the scene to the build after the main menu. Run Configure Startup Screens first. Run it again after adding a zone file. Safe to run again. |
| SqueezySurvival > Build > Build And Run Locally | Builds the Windows player into `Builds/Local` (not versioned) and runs it. Use it to see things that only exist in a built game, such as the splash logos. |

## First-time setup

1. Install Unity Hub and editor 6000.4.2f1.
2. Install Git and Git LFS. Run `git lfs install` once per machine.
3. Install the .NET 10 SDK (pinned by `Launcher/global.json`).
4. Clone the repository and open the folder in Unity Hub.

## Developer scripts

Machine setup and diagnostics are scripted in `Tools/` so every step is repeatable and versioned. Each script writes its output to `Tools/Output/<ScriptName>.log`.

| Script | Purpose |
|---|---|
| `Check-DevEnvironment.cmd` | Reports Windows, .NET SDK, Git, Git LFS and winget versions. |
| `Build-Launcher.cmd` | Builds the launcher solution with warnings as errors and runs its unit tests. |
| `Run-Launcher.cmd` | Runs the launcher from source for visual checks. Update actions are disabled when not installed. |
| `Set-ReleaseToken.cmd` | Stores the GitHub release token for the current Windows user, with hidden input. |
| `Publish-Release.cmd` | Builds, tests, packages and publishes a release. See [Release Pipeline](ReleasePipeline.md). |
