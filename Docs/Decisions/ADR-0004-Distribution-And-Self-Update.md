# ADR-0004: Distribution and self-update

**Status:** Proposed
**Date:** 2026-10-15

## Context

A shareable build is a primary, non-negotiable deliverable from the first build onward. Requirements:

1. Friends and family download the game from GitHub.
2. They double-click a desktop icon and the game opens, with no other installation steps or prerequisites.
3. After the first download, updates are installed from the launcher with one click. No update control exists inside the game (OQ-015).

## Proposed decision

| Concern | Choice |
|---|---|
| Source repository | `squeezy102/SqueezySurvival`, private. *Superseded: the source repository is now public ([ADR-0006](ADR-0006-Public-Source-And-Contribution-Workflow.md)).* |
| Releases repository | `squeezy102/SqueezySurvival-Releases`, public. Contains only a player-facing README and GitHub Releases with build artifacts. No source code. |
| Hosting | GitHub Releases on the releases repository. Build output is never committed to either repository. |
| Platforms | Windows (x64) only for now. Other platforms are not excluded; see Portability. |
| Packaging and updates | [Velopack](https://docs.velopack.io/) (open source). Produces a per-user `Setup.exe` that installs without admin rights and creates a desktop shortcut, plus a portable zip. Provides update checks and delta updates. |
| Launcher | A small .NET launcher is Velopack's main executable. It handles Velopack hooks and updates, then starts the Unity player. The Unity game has no Velopack dependency. Rationale and precedent: [SPIKE-001](../Engineering/Spikes/SPIKE-001-Velopack-With-Unity.md). |
| Update flow | The launcher shows an indicator when a newer release exists and installs it on request. Specification: [Launcher](../Design/Launcher.md). |
| Build trigger | An editor menu command that builds, versions, packages and publishes. Automated builds on GitHub Actions are deferred; see Consequences. |
| Versioning | Semantic Versioning, shown on the main menu. |

## Acceptance test

1. Publish v0.0.1: the launcher and an empty game scene showing its version number.
2. On a clean Windows PC with no development tools, download from GitHub, install and launch from the desktop icon.
3. Publish v0.0.2. Open the v0.0.1 launcher, confirm the update indicator, update, and confirm v0.0.2 is running.

The ADR is accepted only when this test passes.

**Status of the test (2026-10-15):** v0.0.1 published successfully with launcher and Unity tests passing. Steps 2 and 3 (clean-PC install, in-launcher update to v0.0.2) are deferred and not yet run.

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

- **Zip download with no updater.** Rejected: fails the one-click update requirement.
- **itch.io with the itch app.** Viable fallback. Rejected as primary: the requirement names GitHub, and the itch app is an extra install for players.
- **Custom launcher and updater.** Rejected unless Velopack fails the spike: more code to own and test.

## Consequences

- **Windows SmartScreen.** Without a paid code-signing certificate, first launch shows "Windows protected your PC". Players click More info, then Run anyway, once. Updates are not affected.
- **Automated builds deferred.** Unity Personal license activation in CI is currently unreliable ([Unity Discussions](https://discussions.unity.com/t/unity-no-longer-supports-manual-activation-of-personal-licenses/926760)). Builds run locally until that is resolved.
