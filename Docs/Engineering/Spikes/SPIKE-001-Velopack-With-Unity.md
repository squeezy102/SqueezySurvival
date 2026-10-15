# SPIKE-001: Velopack with a Unity player

**Status:** Revised (launcher approach)
**Decides:** [ADR-0004](../../Decisions/ADR-0004-Distribution-And-Self-Update.md)

## Goal

Prove that Velopack can install, launch and self-update a Unity Windows game, end to end, from the public releases repository.

## Research findings

| Finding | Source |
|---|---|
| Velopack maintainers have no working Unity sample, with or without a launcher, and are not familiar with Unity. | [velopack#164](https://github.com/velopack/velopack/issues/164) |
| The shipping precedent (a commercial Unity card game in beta) uses a small C# launcher as the Velopack-managed process, with the Unity player as payload. It works on Windows. | [velopack#164](https://github.com/velopack/velopack/issues/164) |
| The maintainer endorses "a sample Unity game that is just the payload", with the launcher built from the standard Velopack quick start. | [velopack#164](https://github.com/velopack/velopack/issues/164) |
| `System.Diagnostics.Process` does not work under IL2CPP, which Velopack's in-process update path relies on unless a custom implementation is supplied. | [velopack#164](https://github.com/velopack/velopack/issues/164) |
| `VelopackApp.Build().Run()` must be the first code in `Main()`. Unity players have no `Main()`; the earliest game hook runs after the engine starts. | [Velopack: Integrating](https://docs.velopack.io/integrating/overview) |
| Velopack NuGet 1.2.161 targets .NET Standard 2.0, .NET Framework 4.7.2 and .NET 8 to 10. `vpk` 1.2.161 targets .NET 8. | [NuGet: Velopack](https://www.nuget.org/packages/velopack), [NuGet: vpk](https://www.nuget.org/packages/vpk) |
| `vpk pack` creates `Setup.exe`, a portable zip and a release feed. Shortcuts default to Desktop and Start menu. | [vpk CLI (Windows)](https://docs.velopack.io/reference/cli/content/vpk-windows) |
| `vpk download github` and `vpk upload github` (with `--repoUrl`, `--token`, `--publish`) fetch the previous release for delta generation and publish the new one. | [vpk CLI (Windows)](https://docs.velopack.io/reference/cli/content/vpk-windows) |

## Decision

Embedding Velopack in the Unity player has no precedent and conflicts with Velopack's documented startup contract. The spike uses the precedented launcher architecture instead.

```
SqueezySurvival (installed by Velopack)
  SqueezySurvival.Launcher.exe   Velopack main executable: hooks, update check, apply
  Game/
    SqueezySurvival.exe          Unity player, no Velopack dependency
```

| Component | Responsibility |
|---|---|
| Launcher (.NET console or minimal window app) | Runs `VelopackApp.Build().Run()` first in `Main()`. Checks GitHub Releases for updates, downloads and applies them, then starts the Unity player and exits. |
| Unity game | Knows nothing about Velopack. Shows its version. Reads the release feed to detect a newer version and, when the player chooses to update, starts the launcher in update mode and quits. |

Benefits: Unity code stays engine-portable and IL2CPP-compatible; the update path is ordinary, documented Velopack usage; a future macOS or Linux port reuses the launcher pattern.

The game-to-launcher update request is designed in a follow-up document before implementation.

## Questions

| ID | Question | Pass criterion |
|---|---|---|
| Q1 | Can `vpk pack` package the launcher plus the Unity player folder, with the launcher as the main executable? | `Setup.exe` and feed produced without errors or warnings. |
| Q2 | Does `Setup.exe` install per-user with no admin prompt, create a desktop shortcut and launch the game? | Verified on a clean Windows PC. |
| Q3 | Does the launcher detect, download and apply a newer release and then start the new game version? | v0.0.1 updates to v0.0.2. |
| Q4 | Can the game trigger an update through the launcher? | Update started from inside v0.0.1 results in v0.0.2 running. |
| Q5 | Does uninstall remove the game and its shortcuts? | Nothing left except user data we choose to keep. |

## Open research before implementation

- Launcher publish mode: self-contained .NET versus Native AOT, judged on output size, startup time and Velopack compatibility.
- Launcher user interface during download (console, minimal window, or none).

## Superseded approach

The first plan called `VelopackApp.Build().Run()` from `[RuntimeInitializeOnLoadMethod(BeforeSceneLoad)]` inside the Unity player, with Velopack added through NuGetForUnity. It was abandoned before running because it had no precedent and conflicts with the documented requirement that `Run()` be the first code in `Main()`.

## Results

Not yet run.
