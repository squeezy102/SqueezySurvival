# ADR-0004: Distribution and self-update

**Status:** Proposed
**Date:** 2026-10-15

## Context

A shareable build is a primary, non-negotiable deliverable from the first build onward. Requirements:

1. Friends and family download the game from GitHub.
2. They double-click a desktop icon and the game opens, with no other installation steps or prerequisites.
3. After the first download, the game updates itself from inside the game ("update to latest version").

## Proposed decision

| Concern | Choice |
|---|---|
| Source repository | `squeezy102/SqueezySurvival`, private. |
| Releases repository | `squeezy102/SqueezySurvival-Releases`, public. Contains only a player-facing README and GitHub Releases with build artifacts. No source code. |
| Hosting | GitHub Releases on the releases repository. Build output is never committed to either repository. |
| Platforms | Windows (x64) only for now. Other platforms are not excluded; see Portability. |
| Packaging and updates | [Velopack](https://docs.velopack.io/) (open source). Produces a per-user `Setup.exe` that installs without admin rights and creates a desktop shortcut, plus a portable zip. Provides update checks and delta updates. |
| Launcher | A small .NET launcher is Velopack's main executable. It handles Velopack hooks and updates, then starts the Unity player. The Unity game has no Velopack dependency. Rationale and precedent: [SPIKE-001](../Engineering/Spikes/SPIKE-001-Velopack-With-Unity.md). |
| In-game update | The game detects a newer release and shows an update button. Updating hands off to the launcher, which downloads, applies and restarts the game. |
| Build trigger | An editor menu command that builds, versions, packages and publishes. Automated builds on GitHub Actions are deferred; see Consequences. |
| Versioning | Semantic Versioning, shown on the main menu. |

## Acceptance test

1. Publish v0.0.1: an empty screen showing its version number and an update control.
2. On a clean Windows PC with no development tools, download from GitHub, install and launch from the desktop icon.
3. Publish v0.0.2. Launch v0.0.1, update from inside the game, confirm v0.0.2 is running.

The ADR is accepted only when this test passes.

## Portability

Windows is the only shipped platform, but nothing in the design is Windows-specific outside a replaceable boundary:

- Game code depends on an update-service interface, never on Velopack or Windows APIs directly. Each platform supplies its own implementation.
- The build pipeline takes the target platform as a parameter rather than hard-coding Windows.
- Velopack also supports macOS and Linux, so a port would reuse the same release flow. macOS additionally requires a paid Apple developer account for signing and notarization.

## Open items

- **Velopack with Unity.** Velopack targets .NET applications. Using it with a Unity player executable must be proven by [SPIKE-001](../Engineering/Spikes/SPIKE-001-Velopack-With-Unity.md) before this ADR is accepted.
- **Publishing credentials.** Publishing to the releases repository requires a GitHub token on the build machine, scoped to that repository only, and never committed.

## Resolved

- [OQ-012](../Design/OpenQuestions.md): source private, releases public in a separate repository.
- [OQ-013](../Design/OpenQuestions.md): Windows only for now, designed for portability.

## Alternatives considered

- **Zip download with no updater.** Rejected: fails the in-game update requirement.
- **itch.io with the itch app.** Viable fallback. Rejected as primary: the requirement names GitHub, and the itch app is an extra install for players.
- **Custom launcher and updater.** Rejected unless Velopack fails the spike: more code to own and test.

## Consequences

- **Windows SmartScreen.** Without a paid code-signing certificate, first launch shows "Windows protected your PC". Players click More info, then Run anyway, once. Updates are not affected.
- **Automated builds deferred.** Unity Personal license activation in CI is currently unreliable ([Unity Discussions](https://discussions.unity.com/t/unity-no-longer-supports-manual-activation-of-personal-licenses/926760)). Builds run locally until that is resolved.
