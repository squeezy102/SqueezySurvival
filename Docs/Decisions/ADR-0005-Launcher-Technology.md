# ADR-0005: Launcher technology

**Status:** Proposed
**Date:** 2026-10-15

## Context

[ADR-0004](ADR-0004-Distribution-And-Self-Update.md) requires a launcher that is Velopack's main executable. The launcher has a small graphical interface ([Launcher](../Design/Launcher.md)), must be unit testable, must need no runtime installed by the player, and must not exclude a future macOS or Linux port.

## Decision

| Concern | Choice | Version |
|---|---|---|
| Runtime | .NET 10 (LTS, supported until 2028-11-14) | 10.0 |
| UI framework | Avalonia | 12.1.4 |
| MVVM support | CommunityToolkit.Mvvm (Microsoft, .NET Foundation) | 8.4.2 |
| Installer and updates | Velopack | 1.2.161 |
| Packaging tool | `vpk`, pinned as a local .NET tool | 1.2.161 |
| Tests | NUnit, matching the Unity Test Framework | Current stable |
| Publish mode | Self-contained, `win-x64`, not trimmed, not Native AOT | - |

### Architecture

Model-View-ViewModel. Views contain no logic. View models depend only on interfaces, so every behavior in the [Launcher](../Design/Launcher.md) state table is unit tested without a window or network.

| Interface | Responsibility | Production implementation |
|---|---|---|
| `IGameUpdateService` | Installed version, check, download, apply | Wraps Velopack `UpdateManager` with a GitHub source |
| `IGameProcessStarter` | Starts the game executable | Wraps `System.Diagnostics.Process` |
| `IUserPromptService` | Confirmation and message dialogs | Avalonia dialogs |

### Repository layout

```
Launcher/
  SqueezySurvival.Launcher.sln
  SqueezySurvival.Launcher/           Avalonia application
  SqueezySurvival.Launcher.Tests/     NUnit tests
```

The Unity `.gitignore` ignores all `*.sln` and `*.csproj` files because Unity regenerates its own. Explicit exceptions are added for `Launcher/`.

## Rationale and precedent

- **Velopack's official Avalonia sample** is the reference integration: `VelopackApp.Build().Run()` first in `Main()`, then Avalonia startup ([sample](https://github.com/velopack/velopack/tree/develop/samples/CSharpAvalonia), [Velopack samples](https://docs.velopack.io/category/sample-apps)).
- **Avalonia is cross-platform**, preserving the portability requirement in ADR-0004.
- **.NET 10** is the current LTS. .NET 8 reaches end of support on 2026-11-10 ([.NET support policy](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core)).
- **Self-contained publish** is Velopack's recommended mode for C# apps ([Velopack C# guide](https://docs.velopack.io/getting-started/csharp)) and needs no runtime on the player's PC.

## Alternatives considered

| Alternative | Reason rejected |
|---|---|
| Velopack's UnityMono sample (Velopack inside the Unity player) | Requires rebuilding Unity's C++ player in Visual Studio for every build, supports only the Mono backend, and targets Unity 2022 ([sample README](https://github.com/velopack/velopack/tree/develop/samples/CSharpUnityMono)). No launcher UI. |
| WPF or Windows Forms | Windows only, contradicting the portability requirement. Microsoft disables trimming for both ([.NET trimming incompatibilities](https://learn.microsoft.com/dotnet/core/deploying/trimming/incompatibilities)). |
| Native AOT | Velopack does not state Native AOT support; a reported AOT runtime failure led the maintainer to call AOT support future work ([velopack#122](https://github.com/velopack/velopack/issues/122)). Revisit if Velopack documents support. |
| Framework-dependent publish with `--framework` runtime install | Adds a runtime download and a failure point to installation. |
| Launcher built in Unity | A second Unity player is heavy, and Unity cannot host `VelopackApp.Run()` in `Main()`. |

## Consequences

- The installer is larger than a framework-dependent build because it carries the .NET runtime. Subsequent updates use Velopack delta packages.
- Developer machines need the .NET 10 SDK.
- The launcher is a second codebase in the repository, held to the same [Coding Standards](../Engineering/CodingStandards.md).
