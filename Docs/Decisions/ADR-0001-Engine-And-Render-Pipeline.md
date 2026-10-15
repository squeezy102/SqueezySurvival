# ADR-0001: Engine and render pipeline

**Status:** Accepted
**Date:** 2026-10-15

## Context

The game is 2D and UI-driven: sprites, text, windows and progress bars, with animation added after the MVP.

## Decision

Unity 6 (6000.4.2f1), Universal 2D template: Universal Render Pipeline with the 2D Renderer.

## Alternatives considered

- **Universal 3D template.** Rejected: 3D defaults (lighting, camera, packages) add weight and settings that do not apply to a 2D game.
- **Built-in Render Pipeline.** Rejected: legacy pipeline; URP is Unity's supported path forward.

## Consequences

- 2D lighting, sprite sorting and post-processing are available if wanted later.
- Shaders and materials must be URP-compatible.
