# Getting Started

**New to GitHub, git or game code? Start here.** This guide assumes you have never worked on a project like this. It explains the words you will see, how to get the game onto your computer, how to set up an AI assistant to help you, and how to find something to work on. No step needs the command line.

You do not have to read everything else in this repository first. When this guide points somewhere, follow that link when you need it.

## Ways to help, no code needed

You can help without installing anything:

- **Share ideas** in [Discussions](https://github.com/squeezy102/SqueezySurvival/discussions): pitch a feature in **Ideas**, ask a question in **Q&A**, or vote in **Polls**.
- **Play the game and report problems** as issues (see [Reporting a problem](#reporting-a-problem)).

Everyone follows the [Code of Conduct](CODE_OF_CONDUCT.md): be kind, stay on topic, and credit others' work.

## Words you will see

| Word | What it means |
|---|---|
| **Repository** (repo) | The project's folder on GitHub: every file, plus the history of every change. "Think of a repository as a project folder that tracks changes and stores history" ([GitHub Docs](https://docs.github.com/en/get-started/learning-to-code/getting-started-with-git)). |
| **Clone** | Download a copy of the repository to your computer, still connected to GitHub so you can send changes back. |
| **Commit** | A saved snapshot of your changes, with a short note saying what you changed. |
| **Branch** | Your own copy of the project where you work on one change without affecting anyone else. When it is ready, it is merged back. |
| **`main`** | The official branch: the real game. Nobody changes it directly. |
| **Pull request** (PR) | A request to merge your branch into `main`. squeezy102, the project owner, reviews every one and has the final say. |
| **Issue** | A to-do item on GitHub: a feature to build, a bug to fix, or a task. Each has a number, like **#3**, shown in small gray text under its title. Typing `#3` anywhere on GitHub links to it. |
| **Discussion** | A conversation thread for ideas and questions. Nothing is decided there; decisions go into the docs. |
| **Docs** | The `Docs/` folder: the game's design and rules, written down. The docs are the single source of truth: if the game and the docs disagree, the docs win. |

## Get the project onto your computer

You need three free programs. Install them once.

### 1. A GitHub account

Sign up at [github.com](https://github.com). Then ask squeezy102 to invite you to the repository as a collaborator, and accept the invitation from your email or your GitHub notifications.

### 2. GitHub Desktop

GitHub Desktop is a friendly app for git: no commands to type.

1. Download and install it from [desktop.github.com](https://desktop.github.com).
2. Sign in: **File > Options > Accounts**, then **Sign in to GitHub.com**, and follow the prompts ([GitHub Docs](https://docs.github.com/en/desktop/overview/getting-started-with-github-desktop)).

### 3. Clone the repository

1. In GitHub Desktop, choose **File > Clone Repository**.
2. Click the **URL** tab and enter `squeezy102/SqueezySurvival`.
3. Next to **Local Path**, click **Choose...** and pick a folder, for example `Documents\SqueezySurvival`.
4. Click **Clone** ([GitHub Docs](https://docs.github.com/en/desktop/adding-and-cloning-repositories/cloning-and-forking-repositories-from-github-desktop)).

The project includes images stored with Git LFS (Large File Storage), which GitHub Desktop installs for you ([GitHub Docs](https://docs.github.com/en/desktop/configuring-and-customizing-github-desktop/about-git-large-file-storage-and-github-desktop)). If GitHub Desktop asks to initialize Git LFS, accept.

### 4. Unity, to open and play the game

1. Install [Unity Hub](https://unity.com/download).
2. In Unity Hub, install the editor version **6000.4.2f1** exactly. A different version can change project files.
3. In Unity Hub, add the folder you cloned as a project, then open it. The first opening takes a few minutes.
4. To play, press **Play** at the top of the editor (**Ctrl+P**).

More detail: [Project Setup](Docs/Engineering/ProjectSetup.md).

## Set up your AI assistant

You can do this project with an AI assistant doing most of the technical work, as squeezy102 does. The repository tells assistants how to work here, in [AGENTS.md](AGENTS.md).

- **Claude Code** reads the project's instructions automatically when it is opened in the folder you cloned (through [CLAUDE.md](CLAUDE.md)).
- **Any other assistant** (ChatGPT, Gemini, Copilot, Cursor): tell it to read `AGENTS.md` first, and point its project instructions at that file if it has such a setting.

A good first message to your assistant:

> Read AGENTS.md and the documents it lists. I'm new to git and coding, so explain each step simply and tell me exactly what to click. I'd like to work on issue #N.

Your assistant should follow the project's rules: design is written in the docs before code, every bug is filed as an issue first, and nothing reaches `main` without squeezy102's approval. If it suggests skipping those, point it back to `AGENTS.md`.

## Find something to work on

1. Open the [Issues](https://github.com/squeezy102/SqueezySurvival/issues) tab.
2. Filter by label: click **Labels**, then **good first issue** (small and well described) or **help wanted**.
3. Open an issue to read it. Each one says what is wanted, links to its design, and lists **acceptance criteria**: the checklist that says when it is done.
4. Leave a comment saying you would like to take it, so two people do not work on the same thing.

Not sure where to start? Ask in [Discussions: Q&A](https://github.com/squeezy102/SqueezySurvival/discussions).

## Make a change

All of this happens in GitHub Desktop ([GitHub Docs](https://docs.github.com/en/get-started/learning-to-code/getting-started-with-git)).

1. **Start from the latest `main`.** In **Current Branch**, choose `main`, then click **Fetch origin** (and **Pull origin** if it appears).
2. **Make a branch.** In **Current Branch**, click **New Branch**, name it after the issue, such as `fix/32-event-log-scroll` or `feature/3-explore-beach`, and click **Create Branch**.
3. **Make your changes**, yourself or with your assistant.
4. **Commit.** In the bottom left, type a short note saying what you changed, then click **Commit to** *your branch*. Commit as often as you like.
5. **Publish.** Click **Publish branch** to send your branch to GitHub.
6. **Open a pull request.** Click **Preview Pull Request**, then **Create Pull Request**. GitHub opens in your browser: fill in the template, including `Closes #N` for the issue you finished, and click **Create pull request**.
7. **Review.** squeezy102 reviews it and may ask for changes. Make them on the same branch, commit and push again; the pull request updates by itself. When approved, squeezy102 merges it.

Before you open the pull request, run the tests in Unity (**Window > General > Test Runner > EditMode > Run All**) and make sure they all pass.

## Reporting a problem

1. Go to [Issues](https://github.com/squeezy102/SqueezySurvival/issues) and click **New issue**.
2. Choose **Bug**, and fill in the form: what you did, what you expected, what happened instead, the version (bottom right of the main menu), and how bad it is.
3. Screenshots help.

## Where to go next

| When you want to | Read |
|---|---|
| Understand the game | [Game Design Document](Docs/Design/GameDesignDocument.md) |
| See every document | [Docs/Home.md](Docs/Home.md) |
| Know how changes are reviewed | [CONTRIBUTING.md](CONTRIBUTING.md) |
| Know how issues move from open to done | [Work Tracking](Docs/Engineering/WorkTracking.md) |
| Ask anything | [Discussions: Q&A](https://github.com/squeezy102/SqueezySurvival/discussions) |
