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
| Hosting | GitHub Releases. Build output is never committed to the source repository. |
| Packaging and updates | [Velopack](https://docs.velopack.io/) (open source). Produces a per-user `Setup.exe` that installs without admin rights and creates a desktop shortcut, plus a portable zip. Provides update checks and delta updates. |
| In-game update | On launch the game checks for a newer release and shows an update button. Updating downloads, applies and restarts. |
| Build trigger | An editor menu command that builds, versions, packages and publishes. Automated builds on GitHub Actions are deferred; see Consequences. |
| Versioning | Semantic Versioning, shown on the main menu. |

## Acceptance test

1. Publish v0.0.1: an empty screen showing its version number and an update control.
2. On a clean Windows PC with no development tools, download from GitHub, install and launch from the desktop icon.
3. Publish v0.0.2. Launch v0.0.1, update from inside the game, confirm v0.0.2 is running.

The ADR is accepted only when this test passes.

## Open items

- **Repository visibility ([OQ-012](../Design/OpenQuestions.md)).** The source repository is private, so its releases are not downloadable by others. Options: a public releases-only repository, or a public source repository.
- **Platforms ([OQ-013](../Design/OpenQuestions.md)).** Windows only, or also macOS.
- **Velopack with Unity.** Velopack targets .NET applications. Using it with a Unity player executable must be proven by a spike before this ADR is accepted.

## Alternatives considered

- **Zip download with no updater.** Rejected: fails the in-game update requirement.
- **itch.io with the itch app.** Viable fallback. Rejected as primary: the requirement names GitHub, and the itch app is an extra install for players.
- **Custom launcher and updater.** Rejected unless Velopack fails the spike: more code to own and test.

## Consequences

- **Windows SmartScreen.** Without a paid code-signing certificate, first launch shows "Windows protected your PC". Players click More info, then Run anyway, once. Updates are not affected.
- **Automated builds deferred.** Unity Personal license activation in CI is currently unreliable ([Unity Discussions](https://discussions.unity.com/t/unity-no-longer-supports-manual-activation-of-personal-licenses/926760)). Builds run locally until that is resolved.
