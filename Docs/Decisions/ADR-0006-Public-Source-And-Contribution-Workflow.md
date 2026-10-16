# ADR-0006: Public source and contribution workflow

**Status:** Accepted
**Date:** 2026-10-16

## Context

The source repository was private ([ADR-0004](ADR-0004-Distribution-And-Self-Update.md)). The owner has made it public and invited friends as contributors. The owner must keep the final say on every change, and changes must follow a pull request process.

On a personal-account repository, collaborators can only be given write access: they can push and merge pull requests, and GitHub offers no role in between ([GitHub Docs: permission levels for a personal account repository](https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/repository-access-and-collaboration/permission-levels-for-a-personal-account-repository)). Control therefore has to come from rules on `main`, not from roles.

## Decision

| Concern | Choice |
|---|---|
| Source visibility | Public. Supersedes the "private" source repository in ADR-0004. |
| License | None: all rights reserved ([LICENSE](../../LICENSE)). Contributors grant the owner rights to their contributions ([CONTRIBUTING](../../CONTRIBUTING.md)). |
| Contributors | Invited collaborators with write access. |
| Protection | A GitHub ruleset on `main` requires a pull request, and approval from the code owner, before merging ([Source Control: protecting main](../Engineering/SourceControl.md#protecting-main)). |
| Code owner | `squeezy102` owns every file through `.github/CODEOWNERS` ([GitHub Docs: about code owners](https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/customizing-your-repository/about-code-owners)). |
| Owner's own changes | Also go through pull requests. GitHub does not let an author approve their own pull request, so the owner merges them through a bypass that applies to pull requests only. |
| Merge method | Squash merge; head branches deleted automatically. |

## Alternatives considered

- **Fork-only contributions.** Friends fork and open pull requests without write access, so only the owner can merge. Most locked down, but more friction for friends; rejected in favor of collaborators plus a ruleset.
- **Transfer to an organization.** Allows finer roles (Triage, Maintain). More administration than a small group of friends needs.
- **Open-source license (MIT, GPL-3.0).** Rejected for now: the owner keeps the game.

## Consequences

- Claude no longer pushes to `main`. Changes go to a branch, a pull request is opened, and the owner merges it.
- Anything committed is public. Secrets must never be committed; the release token stays in the owner's user environment ([Release Pipeline](../Engineering/ReleasePipeline.md)). History was scanned for tokens and keys when the repository was made public; none were found.
- The branching model is now defined ([Source Control: branching](../Engineering/SourceControl.md#branching)).
