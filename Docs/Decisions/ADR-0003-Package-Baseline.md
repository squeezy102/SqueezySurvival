# ADR-0003: Package baseline

**Status:** Accepted
**Date:** 2026-10-15

## Context

The Universal 2D template installs packages for workflows this game does not use. Unused packages increase compile time, import time and the surface for warnings.

## Decision

Keep only packages the MVP needs or that the architecture expects soon.

| Kept | Reason |
|---|---|
| Universal RP, 2D Sprite | Rendering |
| uGUI (includes TextMeshPro), UI Toolkit module | Windows, text, progress bars |
| Input System | Player input |
| Test Framework | Unit testing |
| Visual Studio integration | IDE support |
| Audio and Animation modules | Planned features; kept to avoid later rework |

| Removed | Reason |
|---|---|
| Unity Version Control (collab-proxy) | Git is the source control system |
| Visual Scripting | All logic is C# |
| Multiplayer Center | Single-player |
| Timeline | No cutscenes planned |
| Tilemap, Tilemap Extras, Sprite Shape | No tile or spline-based worlds |
| PSD and Aseprite importers, 2D Tooling, 2D Animation | Not needed for placeholder art; re-add with animation work |
| Rider integration | Visual Studio is the IDE |
| `com.unity.pipeline` (experimental) | Added by Unity Hub's "Use Unity CLI" option. Removed 2026-10-15: logged a warning in every player build and is not used. |
| 3D physics, terrain, cloth, vehicles, wind, VR, XR, video, analytics modules | Not applicable to a 2D single-player game |

## Consequences

- Re-adding a package is a one-line change in `Packages/manifest.json` and requires updating this ADR.
- Packages required as dependencies of kept packages are resolved automatically.
