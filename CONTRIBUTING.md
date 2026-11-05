# Contributing

Thanks for helping build SqueezySurvival. New to git or coding? Read [Getting Started](GETTING_STARTED.md) first. The project owner, squeezy102, has the final say on every change. Everyone taking part follows the [Code of Conduct](CODE_OF_CONDUCT.md).

## How changes are made

0. **Stage branch, once.** Create your own stage branch from `main`, named as you like, and treat it as your main ([Source Control: branching](Docs/Engineering/SourceControl.md#branching)). It is the only branch you ever cut from `main`. Nobody works on `main` itself.
1. **Branch.** Create a branch from your own stage branch, never from `main` or another collaborator's stage branch, named for the change: `design/weather-effects`, `feature/time-system`, `fix/launcher-version-label`.
2. **Commit.** Follow [Source Control: commits](Docs/Engineering/SourceControl.md#commits).
3. **Merge it back into your stage branch** when it is finished.
4. **Open a pull request** from your stage branch into `main` when it holds tested work that is ready, and fill in the template.
5. **Review.** squeezy102 reviews every pull request into `main`. Nothing merges without that approval.
6. **Merge.** squeezy102 merges the approved pull request ([Source Control: branching](Docs/Engineering/SourceControl.md#branching)).

Nobody pushes directly to `main`. A ruleset on GitHub enforces this ([Source Control: protecting main](Docs/Engineering/SourceControl.md#protecting-main)).

## Share your ideas

Have an idea for the game, a question, or something you made? Start a thread in [Discussions](https://github.com/squeezy102/SqueezySurvival/discussions). Pitch it in **Ideas**, ask in **Q&A**, or put options to a vote in **Polls**. Every idea is welcome. Good ideas move from Discussions into the design docs once squeezy102 approves them, and then into issues for building ([Work Tracking: from idea to work](Docs/Engineering/WorkTracking.md#from-idea-to-work)).

## Finding and reporting work

Work is tracked in [GitHub Issues](https://github.com/squeezy102/SqueezySurvival/issues): user stories, bugs and tasks, each opened with a form. See [Work Tracking](Docs/Engineering/WorkTracking.md).

**Issue numbers.** Every issue, pull request and discussion has a number, such as **#3**. In the issue list it appears in small gray text under the title (`squeezy102/SqueezySurvival#3`); on the issue's own page it follows the title. Refer to work by its number: typing `#3` in any comment, commit or pull request links to that issue, and pull requests list the issues they finish (`Closes #3`).

## Before you start

- Using an AI assistant? Have it read [AGENTS.md](AGENTS.md) first; it is the baseline for LLM-assisted contributions.
- Read [Docs/Home.md](Docs/Home.md), especially the documentation policy: **docs come first**, and a change is not done until every affected document is updated in the same pull request.
- The project is a **playable wireframe**: code is written only for systems that are already designed. Design ideas are proposed as pull requests to the design documents, or as new entries in [Open Questions](Docs/Design/OpenQuestions.md).
- Code follows [Coding Standards](Docs/Engineering/CodingStandards.md): testable, warning-free, descriptive names.

## Rights to contributions

SqueezySurvival is not open source; all rights are reserved ([LICENSE](LICENSE)). By submitting a contribution, you confirm you have the right to submit it, and you grant squeezy102 the right to use, modify and distribute it as part of SqueezySurvival.
