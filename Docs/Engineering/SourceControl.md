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

## Working on the owner's PC

squeezy102 works and tests in his local repository, and never runs git. **Claude works in that repository, not in a cloud copy.** The conversation is linked to the repository folder on his PC, and Claude edits the files there and runs git there through the PC-side shell (`device_bash`, in the connected repository folder). Changes stay **uncommitted** while squeezy102 tests them in Unity; Claude commits only after he has tested and says yes ([Commits](#commits)). If the conversation is not linked to the folder, Claude says so and asks him to add it before changing any code; it does not start in a cloud copy. The cloud workspace is used only to push, because it has the repository attached with push access ([issue #152](https://github.com/squeezy102/SqueezySurvival/issues/152)).

At the start of a session:

1. Ask for delete permission once, for the repository folder (`device_request_delete_permission`). Without it git cannot remove its own `.lock` files or replace files, so `commit`, `pull`, `checkout` and `merge` fail or leave locks behind.
2. Run every git command there with the settings below.
3. Check the branch and `git status` before changing anything.

Settings, passed to every git command (`G` here stands for the whole prefix):

```
G='git -c core.autocrlf=true -c filter.lfs.process= -c filter.lfs.smudge=cat -c filter.lfs.clean=cat -c filter.lfs.required=false'
```

- `core.autocrlf=true`: Windows checks files out with CRLF while the shell sees LF, so without it unchanged files look modified ([git-config: core.autocrlf](https://git-scm.com/docs/git-config#Documentation/git-config.txt-coreautocrlf)).
- The four `filter.lfs.*` settings: the shell has no `git-lfs`, so without them git stops on the missing filter ([git-config: filter](https://git-scm.com/docs/gitattributes#_filter)). Binary files (fonts, images) still show as modified under these settings; never stage them, and never commit a change to one from here.
- The `git-lfs` hook warnings that `checkout`, `merge` and `commit` print can be ignored.

| Task | How |
|---|---|
| Pull a branch | `$G fetch origin <branch>`, `$G checkout <branch>`, `$G merge --ff-only FETCH_HEAD`. Fast-forward only: if it cannot, stop and tell squeezy102. |
| Start a branch | `$G checkout -b <branch> PjDev` on the PC and in the cloud workspace, from the same commit. |
| Make a change | Edit the files in the PC repository, on a branch started as above. Leave them uncommitted for squeezy102 to test in Unity. A check that needs tools the PC lacks (the .NET compile of Core) runs on a copy in the cloud workspace and never replaces the PC's files. |
| Commit and push | Only after squeezy102 says yes: commit on the PC with the settings above, then push the commit through a bundle ([committing from a sandbox](#committing-from-a-sandbox)). |
| After a merge | Pull `PjDev` as above, then `$G branch -d <merged branch>`. |

The shell has no GitHub credentials, which is why pushes go through the cloud workspace.

A `.lock` file under `.git` is removed only when an earlier command of this session left it, never one that a running git, squeezy102's included, may hold. With delete permission, `rm` it. Without it, `mv` it to a new name directly under `.git` (never under `.git/refs`).

## Committing from a sandbox

Work for squeezy102 is done in his local repository, not in a sandbox copy ([working on the owner's PC](#working-on-the-owners-pc)); this section is how the commits made there reach GitHub, and how a contributor's assistant that works in a sandbox commits. An assistant that works in a sandbox, such as Claude's desktop link to squeezy102's PC, has a git with no GitHub credentials and no Git LFS, so it can commit but not push or fetch LFS files. Text-only commits still reach GitHub without squeezy102 running anything:

1. Commit in the checkout, staging only the files changed. Use the [settings for the owner's PC](#working-on-the-owners-pc), so the sandbox sees the same state Windows does and does not stop on the missing `git-lfs`; never stage binary files.
2. Bundle the new commits: `git bundle create Temp/<name>.bundle origin/<branch>..<branch>` ([git-bundle](https://git-scm.com/docs/git-bundle)). `Temp/` is git-ignored.
3. Copy the bundle to an environment that is signed in to GitHub (Claude's cloud workspace has the repository attached with push access), fetch from it and push the branch there: the same commits, with the same ids, so squeezy102's `git pull` is a plain fast-forward.
4. Delete the bundle.

Commits that add or change binary files still need squeezy102's `git push` from the PC.

## Ignored files

`.gitignore` follows the [GitHub Unity template](https://github.com/github/gitignore/blob/main/Unity.gitignore). Generated folders and IDE files are never committed.

## Commits

- **Commit at milestones, not every exchange.** Ideas change during a design discussion, so work is committed only once a substantial change has settled. Pj tests code in Unity first. Claude leaves it uncommitted until he has tested it, then asks whether to commit, and commits only on a yes.
- One logical change per commit.
- Subject line in the imperative mood, 72 characters or fewer: `Add survival stat decay calculator`.
- The body explains why when it is not obvious.
- Code, tests and documentation for a change are committed together.
- Every commit leaves the project compiling with zero warnings.

## Branching

Nobody works on `main`, and nobody pushes directly to it. This section is the one home of the branching rules; [AGENTS.md](../../AGENTS.md#workflow), [CONTRIBUTING](../../CONTRIBUTING.md#how-changes-are-made) and [Getting Started](../../GETTING_STARTED.md) summarize the steps and link here.

| Branch | What it is | Cut from | Merged into | Deleted |
|---|---|---|---|---|
| `main` | The line that releases are made from | - | - | Never |
| Stage branch | One per collaborator, named as they like, treated as their own `main`. squeezy102's is `PjDev`. | `main`, once | `main`, by a pull request squeezy102 approves | Never |
| Working branch (`design/`, `feature/`, `fix/` or `docs/`) | One change, named for its issue | The collaborator's own stage branch | The same stage branch | After it is merged |

1. **Once**, create your stage branch from `main`. It is the only branch you ever cut from `main`.
2. Cut every working branch from your own stage branch, never from `main` and never from another collaborator's stage branch. Merge finished work back into the same stage branch.
3. A stage branch reaches `main` only through a pull request from it into `main`, opened when its work is tested and ready. squeezy102 reviews and approves it, then merges it ([CONTRIBUTING](../../CONTRIBUTING.md)).
4. `main` is pushed to only by those pull requests, including squeezy102's own.
5. After a merge into `main`, bring your stage branch up to date with it.

### squeezy102's integration branch

`PjDev` is squeezy102's stage branch, and the branch his work is integrated in.

1. squeezy102's working branches (and Claude's) are cut from `PjDev` and merged back into it with a merge commit ("Create a merge commit" on GitHub), never a squash.
   Claude never pushes to `PjDev` itself: it pushes a branch such as `docs/...`, opens a pull request into `PjDev`, and merges it. squeezy102 has asked Claude to handle all git work unless their input is specifically required, such as merging into `main` or pushing what only their credentials can.
2. When `PjDev` holds a set of changes ready for `main`, a pull request from `PjDev` into `main` brings them in. squeezy102 merges it with a **merge commit**, never a squash: a merge commit keeps `main` an ancestor of `PjDev`, so the two never drift apart and the next pull request shows only new work. Squash-merging `PjDev` into `main` gave `main` a copy of the same changes under a different history, so every later pull request conflicted until `main` was merged back into `PjDev` (2026-10-20).
3. Claude handles git for squeezy102: it commits, pushes its branches, and keeps squeezy102's local repository in sync (checking out and pulling the branch to test) through the desktop app's link to their PC. squeezy102 runs git commands only for pushes that need their credentials, such as Git LFS files or `PjDev` after a merge from `main`. On that PC, Windows Git converts line endings (`core.autocrlf`), so git commands Claude runs there pass `-c core.autocrlf=true` to see the same state Windows does.

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
