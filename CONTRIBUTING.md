# Contributing

Thanks for helping build SqueezySurvival. New to git or coding? Read [Getting Started](GETTING_STARTED.md) first. The project owner, squeezy102, has the final say on every change. Everyone taking part follows the [Code of Conduct](CODE_OF_CONDUCT.md).

## How changes are made

1. **Branch.** Create a branch from `main`, named for the change: `design/weather-effects`, `feature/time-system`, `fix/launcher-version-label`.
2. **Commit.** Follow [Source Control: commits](Docs/Engineering/SourceControl.md#commits).
3. **Open a pull request** into `main` and fill in the template.
4. **Review.** squeezy102 reviews every pull request. Nothing merges without that approval.
5. **Merge.** Once approved, the pull request is squash-merged and the branch is deleted.

Nobody pushes directly to `main`. A ruleset on GitHub enforces this ([Source Control: protecting main](Docs/Engineering/SourceControl.md#protecting-main)).

## Share your ideas

Have an idea for the game, a question, or something you made? Start a thread in [Discussions](https://github.com/squeezy102/SqueezySurvival/discussions). Pitch it in **Ideas**, ask in **Q&A**, or put options to a vote in **Polls**. Every idea is welcome. Good ideas move from Discussions into the design docs once squeezy102 approves them, and then into issues for building ([Work Tracking: from idea to work](Docs/Engineering/WorkTracking.md#from-idea-to-work)).

## Finding and reporting work

Work is tracked in [GitHub Issues](https://github.com/squeezy102/SqueezySurvival/issues): user stories, bugs and tasks, each opened with a form. Pull requests reference the issue they finish (`Closes #3`). See [Work Tracking](Docs/Engineering/WorkTracking.md).

## Before you start

- Using an AI assistant? Have it read [AGENTS.md](AGENTS.md) first; it is the baseline for LLM-assisted contributions.

- Read [Docs/Home.md](Docs/Home.md), especially the documentation policy: **docs come first**, and a change is not done until every affected document is updated in the same pull request.
- The project is in the **design phase**: no gameplay code is written until the core designs are concrete. Design ideas are proposed as pull requests to the design documents, or as new entries in [Open Questions](Docs/Design/OpenQuestions.md).
- Code follows [Coding Standards](Docs/Engineering/CodingStandards.md): testable, warning-free, descriptive names.

## Rights to contributions

SqueezySurvival is not open source; all rights are reserved ([LICENSE](LICENSE)). By submitting a contribution, you confirm you have the right to submit it, and you grant squeezy102 the right to use, modify and distribute it as part of SqueezySurvival.
