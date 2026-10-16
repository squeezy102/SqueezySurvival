# Source Control

**Status:** Approved

## Repository

| Item | Value |
|---|---|
| Host | GitHub, `squeezy102/SqueezySurvival` |
| Visibility | Public ([ADR-0006](../Decisions/ADR-0006-Public-Source-And-Contribution-Workflow.md)) |
| Default branch | `main` |

Player builds are published to a separate public repository, `squeezy102/SqueezySurvival-Releases`. See [ADR-0004](../Decisions/ADR-0004-Distribution-And-Self-Update.md).

## Large files

Binary assets (images, audio, fonts, models) are stored with Git LFS. Rules are defined in `.gitattributes`. Unity YAML files (`.unity`, `.prefab`, `.asset`, `.meta`) are text and merge with UnityYAMLMerge.

## Ignored files

`.gitignore` follows the [GitHub Unity template](https://github.com/github/gitignore/blob/main/Unity.gitignore). Generated folders and IDE files are never committed.

## Commits

- **Commit at milestones, not every exchange.** Ideas change during a design discussion, so work is committed only once a substantial change has settled. At a natural stopping point, Claude asks Pj whether to commit, and commits only on a yes.
- One logical change per commit.
- Subject line in the imperative mood, 72 characters or fewer: `Add survival stat decay calculator`.
- The body explains why when it is not obvious.
- Code, tests and documentation for a change are committed together.
- Every commit leaves the project compiling with zero warnings.

## Branching

Nobody pushes directly to `main`. Every change, including the owner's, goes through a pull request ([CONTRIBUTING](../../CONTRIBUTING.md)).

1. Create a branch from `main`: `design/...`, `feature/...`, `fix/...` or `docs/...`.
2. Open a pull request into `main`.
3. squeezy102 reviews and approves it.
4. Squash merge; the branch is deleted automatically.

Claude follows the same flow: it pushes a branch and opens the pull request, and squeezy102 merges it.

## Protecting main

A GitHub ruleset on `main` ([GitHub Docs: available rules for rulesets](https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/available-rules-for-rulesets)):

| Rule | Setting |
|---|---|
| Restrict deletions | On |
| Block force pushes | On |
| Require a pull request before merging | On: 1 approval, dismiss stale approvals when new commits are pushed, require review from Code Owners, require approval of the most recent reviewable push |
| Allowed merge method | Squash |
| Bypass list | Repository admin (squeezy102), for pull requests only |

`.github/CODEOWNERS` makes squeezy102 the owner of every file, so every pull request needs that approval. Because GitHub does not let authors approve their own pull requests, squeezy102 merges their own (and Claude's) pull requests through the bypass, which still requires a pull request.

## Versioning

Releases follow [Semantic Versioning](https://semver.org/spec/v2.0.0.html) and are tagged `vMAJOR.MINOR.PATCH`. The current version is the `VERSION` file at the repository root. Pre-1.0 versions (`0.x.y`) signal that save compatibility and features may change.
