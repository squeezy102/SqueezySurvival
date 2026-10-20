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

The cloud session that Claude works in cannot upload Git LFS objects. A commit that adds or changes a binary file is pushed from squeezy102's PC (`git push`); commits with only text files are pushed by Claude, to a feature branch.

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

### squeezy102's integration branch

`PjDev` is squeezy102's personal integration branch, created from `main`.

1. squeezy102's feature branches (and Claude's) are created from `PjDev` and squash-merged back into it.
   Claude never pushes to `PjDev` itself: it pushes a branch such as `docs/...`, opens a pull request into `PjDev`, and merges it once squeezy102 approves the change in chat.
2. When `PjDev` holds a set of changes ready for `main`, a pull request from `PjDev` into `main` brings them in. squeezy102 merges it with a **merge commit**, never a squash: a merge commit keeps `main` an ancestor of `PjDev`, so the two never drift apart and the next pull request shows only new work. Squash-merging `PjDev` into `main` gave `main` a copy of the same changes under a different history, so every later pull request conflicted until `main` was merged back into `PjDev` (2026-10-20).
3. Claude handles git for squeezy102: it commits, pushes its branches, and keeps squeezy102's local repository in sync (checking out and pulling the branch to test) through the desktop app's link to their PC. squeezy102 runs git commands only for pushes that need their credentials, such as Git LFS files or `PjDev` after a merge from `main`. On that PC, Windows Git converts line endings (`core.autocrlf`), so git commands Claude runs there pass `-c core.autocrlf=true` to see the same state Windows does.

Contributors branch from `main` as described above.

`PjDev` belongs to squeezy102 alone. A second ruleset, `Protect PjDev`, restricts updates, deletions and force pushes on it, with only the repository admin (squeezy102) able to bypass, always. Nobody else can push to it, merge into it, or delete it.

A third ruleset, `Keep PjDev`, blocks deleting and force-pushing `PjDev` with **no** bypass at all. It exists because the repository deletes head branches after a merge: without it, merging `PjDev` into `main` deleted `PjDev` through squeezy102's bypass (2026-10-16).

If `main` ever gets ahead of `PjDev` (for example, after a squash merge), `PjDev` is brought back in line by merging `main` into it, never by force-pushing (which the rulesets block). Claude prepares the merge on a branch and squeezy102 pushes it to `PjDev`.

### Before merging any pull request

Check that the pull request's latest commit is the one that was tested: its head commit must match the tip of the tested branch, and its commit count must match. Pull request #66 merged one commit of four because its head had not caught up with the branch (2026-10-20); the missing commits had to be brought in by #70.

## Protecting main

A GitHub ruleset on `main` ([GitHub Docs: available rules for rulesets](https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-rulesets/available-rules-for-rulesets)):

| Rule | Setting |
|---|---|
| Restrict deletions | On |
| Block force pushes | On |
| Require a pull request before merging | On: 1 approval, dismiss stale approvals when new commits are pushed, require review from Code Owners, require approval of the most recent reviewable push |
| Allowed merge methods | Squash, for every pull request except `PjDev` into `main`; merge commit, for `PjDev` into `main` ([squeezy102's integration branch](#squeezy102s-integration-branch)) |
| Bypass list | Repository admin (squeezy102), for pull requests only |

`.github/CODEOWNERS` makes squeezy102 the owner of every file, so every pull request needs that approval. Because GitHub does not let authors approve their own pull requests, squeezy102 merges their own (and Claude's) pull requests through the bypass, which still requires a pull request.

## Versioning

Releases follow [Semantic Versioning](https://semver.org/spec/v2.0.0.html) and are tagged `vMAJOR.MINOR.PATCH`. The current version is the `VERSION` file at the repository root. Pre-1.0 versions (`0.x.y`) signal that save compatibility and features may change.
