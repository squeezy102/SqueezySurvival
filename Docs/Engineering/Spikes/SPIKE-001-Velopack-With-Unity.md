# SPIKE-001: Velopack with a Unity player

**Status:** Planned
**Decides:** [ADR-0004](../../Decisions/ADR-0004-Distribution-And-Self-Update.md)

## Goal

Prove or disprove that Velopack can install, launch and self-update a Unity Windows player, end to end, from the public releases repository.

## Background

| Fact | Source |
|---|---|
| Velopack NuGet package 1.2.161 targets .NET Standard 2.0, .NET Framework 4.7.2 and .NET 8 to 10. | [NuGet: Velopack](https://www.nuget.org/packages/velopack) |
| The .NET Standard 2.0 build depends on `Newtonsoft.Json` and `Microsoft.Win32.Registry`. | [NuGet: Velopack](https://www.nuget.org/packages/velopack) |
| `VelopackApp.Build().Run()` must run once, as early as possible at startup, to handle install, update and uninstall hooks. Windows hooks time out after 30 seconds (install, uninstall) and 15 seconds (update). | [Velopack: Integrating](https://docs.velopack.io/integrating/overview) |
| The `vpk` packaging tool 1.2.161 is a .NET tool targeting .NET 8. | [NuGet: vpk](https://www.nuget.org/packages/vpk) |
| Packing produces `Setup.exe`, a portable zip and a `releases.{channel}.json` feed. | [Velopack: Packaging](https://docs.velopack.io/packaging/overview) |

Unity players have no `Main()` method. The earliest hook available to game code is `[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]`, which runs after the Unity engine has started.

## Questions

| ID | Question | Pass criterion |
|---|---|---|
| Q1 | Do the Velopack library and its dependencies load in a Unity Mono player? | No load errors in the player log. |
| Q2 | Does calling `VelopackApp.Build().Run()` from `BeforeSceneLoad` handle Velopack's hook invocations within the time limits? | Install, update and uninstall complete without timeout. |
| Q3 | Can `vpk pack` package a Unity player folder with `SqueezySurvival.exe` as the main executable? | `Setup.exe` and feed produced without errors. |
| Q4 | Does `Setup.exe` install per-user with no admin prompt, create a desktop shortcut and launch the game? | Verified on a clean Windows PC. |
| Q5 | Can `UpdateManager` with a GitHub source detect, download and apply a newer release and restart? | v0.0.1 updates itself to v0.0.2 from inside the game. |
| Q6 | Does uninstall remove the game and its shortcuts? | Nothing left except user data we choose to keep. |

## Method

1. Install the .NET SDK on the development machine. Pin `vpk` as a local .NET tool (`.config/dotnet-tools.json`) so every machine uses the same version.
2. Add the Velopack package to Unity through NuGetForUnity, so NuGet dependencies are declared and versioned rather than hand-copied DLLs.
3. Build a minimal scene showing the version number and an update button. Mono scripting backend for the spike.
4. Pack v0.0.1, publish to the releases repository, install on a clean PC.
5. Pack and publish v0.0.2, update from inside v0.0.1.
6. Record the result for each question in this document.

Spike code lives on branch `spike/velopack` and is not merged. Production code is written afterwards to the [Coding Standards](../CodingStandards.md), behind an update-service interface.

## Fallback

If Q1 or Q2 fails, use Velopack for packaging only and add a small .NET launcher executable as the main executable. The launcher handles Velopack hooks and updates, then starts the Unity player. If Velopack fails Q3 to Q5, re-evaluate alternatives in ADR-0004.

## Results

Not yet run.
