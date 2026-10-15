# Source Control

**Status:** Approved

## Repository

| Item | Value |
|---|---|
| Host | GitHub, `squeezy102/SqueezySurvival` |
| Visibility | Private |
| Default branch | `main` |

## Large files

Binary assets (images, audio, fonts, models) are stored with Git LFS. Rules are defined in `.gitattributes`. Unity YAML files (`.unity`, `.prefab`, `.asset`, `.meta`) are text and merge with UnityYAMLMerge.

## Ignored files

`.gitignore` follows the [GitHub Unity template](https://github.com/github/gitignore/blob/main/Unity.gitignore). Generated folders and IDE files are never committed.

## Commits

- One logical change per commit.
- Subject line in the imperative mood, 72 characters or fewer: `Add survival stat decay calculator`.
- The body explains why when it is not obvious.
- Code, tests and documentation for a change are committed together.
- Every commit leaves the project compiling with zero warnings.

## Branching

Work is committed to `main` while the project is in pre-production. A branching model is adopted when automated builds or collaborators are introduced, and recorded in an ADR.

## Versioning

Releases follow [Semantic Versioning](https://semver.org/spec/v2.0.0.html) and are tagged `vMAJOR.MINOR.PATCH`. Pre-1.0 versions (`0.x.y`) signal that save compatibility and features may change.
