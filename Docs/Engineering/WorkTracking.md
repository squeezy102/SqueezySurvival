# Work Tracking

**Status:** Approved

Work is tracked in [GitHub Issues](https://github.com/squeezy102/SqueezySurvival/issues). Design is not.

## What goes where

| Kind | Home |
|---|---|
| Rules, values, decisions, design | Docs: the owning design document, [ADRs](../Decisions/README.md) |
| Unanswered design questions | [Open Questions](../Design/OpenQuestions.md) |
| Findings from playing a build | GitHub Issues labeled `playtest`; sessions indexed in the [Playtest Log](PlaytestLog.md) |
| Work to be done: stories, bugs, tasks | GitHub Issues |

An issue links to the design it implements; it never restates the design. One home per fact ([documentation policy](../Home.md#documentation-policy)).

## Issue types

New issues use a form ([`.github/ISSUE_TEMPLATE/`](../../.github/ISSUE_TEMPLATE/)); blank issues are disabled.

| Type | Label | Use for |
|---|---|---|
| User story | `story` | A player-facing capability: "As a player, I want ..., so that ...", with acceptance criteria. |
| Bug | `bug` | Something works differently than designed: steps, expected, actual, version. |
| Task | `task` | Engineering, tooling or documentation work. |

## Labels and milestones

- **Type:** `story`, `bug`, `task`; `question` for reports that turn out not to need work.
- **Area:** `area: ui`, `area: time`, `area: saving`, `area: exploration`, `area: survival`, `area: tooling`. The existing `accessibility` and `documentation` labels are used as they read.
- **Source:** `playtest` for anything found by playing a build.
- **Status:** `status: in progress` and `status: in development branch` (see the lifecycle below).
- **Milestones:** `Wireframe` (a clickable wireframe of the core loop) and `MVP` (the game described in the [Game Design Document](../Design/GameDesignDocument.md)).

## Lifecycle

1. **Report first.** Every problem or request is filed as an issue before anyone works on it, even if the fix takes minutes. The issue is the record that it happened and how it was resolved.
2. **Todo.** Open, unassigned. Anyone can pick it up.
3. **In progress.** The person working on it assigns themselves and adds `status: in progress`. Work happens on a branch named for the issue, such as `fix/32-event-log-scroll` or `feature/3-explore-beach`.
4. **In development branch.** The fix is merged into a development branch (for squeezy102, `PjDev`) but not yet into `main`. The issue stays open, labeled `status: in development branch`, with a link to the fixing commit.
5. **Done.** Merged into `main` and verified. GitHub closes issues automatically only when closing keywords reach the default branch ([GitHub Docs: linking a pull request to an issue](https://docs.github.com/en/issues/tracking-your-work-with-issues/using-issues/linking-a-pull-request-to-an-issue)), so the pull request into `main` lists every issue it finishes: `Closes #15, Closes #16`.

A report that needs no work is closed as "not planned" with the reason.

## Linking work to issues

- Commits reference their issue: `Refs #3`.
- The pull request into `main` closes its issues with `Closes #N`.
- Playtest sessions are indexed in the [Playtest Log](PlaytestLog.md), which links each finding to its issue.

## Who maintains issues

Claude opens, updates and closes issues as part of its work, with squeezy102's standing approval, and reports what it changed. Contributors open issues with the forms. squeezy102 decides priority and milestones.
