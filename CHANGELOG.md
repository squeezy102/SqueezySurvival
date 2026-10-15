# Changelog

All notable changes to this project are documented here.

Format: [Keep a Changelog](https://keepachangelog.com/en/1.1.0/). Versioning: [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Added
- Unity 6 (6000.4.2f1) Universal 2D project with URP 2D Renderer.
- Unity `.gitignore` and `.gitattributes` with Git LFS for binary assets.
- Project documentation structure: README, Game Design Document, Open Questions, Coding Standards, Source Control, Project Setup, Glossary, and Architecture Decision Records.

- Launcher (Avalonia, Velopack) with unit tests. See [Launcher](Docs/Design/Launcher.md).
- Release pipeline: `Tools/Publish-Release.cmd`. See [Release Pipeline](Docs/Engineering/ReleasePipeline.md).
- Developer scripts in `Tools/`.

### Removed
- Template packages outside MVP scope. See [ADR-0003](Docs/Decisions/ADR-0003-Package-Baseline.md).
