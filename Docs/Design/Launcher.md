# Launcher

**Status:** Draft

The launcher is what players open from the desktop shortcut. It shows the installed version, starts the game, and installs updates. Technology: [ADR-0005](../Decisions/ADR-0005-Launcher-Technology.md). Distribution: [ADR-0004](../Decisions/ADR-0004-Distribution-And-Self-Update.md).

## Layout

```
+--------------------------------------------------------------+
|                                                              |
|                     Background artwork                       |
|                                                              |
|                                                              |
|  [ Play ]  [ Check for update (!) ]                 v0.0.1   |
+--------------------------------------------------------------+
```

| Element | Position | Notes |
|---|---|---|
| Background artwork | Fills the window | Placeholder art during development. Source and license recorded when added. |
| Play button | Bottom left | Primary action. |
| Check for update button | Bottom left, right of Play | Shows an update indicator when a newer version is known. |
| Installed version | Bottom right | Format `v{MAJOR}.{MINOR}.{PATCH}`. |

Window: fixed size, not resizable, centered on screen, titled "SqueezySurvival".

## Behavior

### On open

1. Show the installed version.
2. Check for updates in the background. The window is usable immediately; the check never blocks Play.
3. If a newer version exists, show the update indicator.

### Update indicator

- An exclamation mark icon on the Check for update button.
- Tooltip on hover: "A newer version is available."
- Static, not flashing. Flashing UI is an accessibility risk (WCAG 2.3.1) and adds no information over a persistent badge.

### Play

1. Start the installed game executable.
2. Close the launcher. See [OQ-014](OpenQuestions.md).

If the game executable is missing or fails to start, show an error message and keep the launcher open.

### Check for update

| Result | Response |
|---|---|
| Newer version available | Prompt: "Version {new} is available. You have {current}. Update now?" with **Update** and **Not now**. |
| Already up to date | Message: "You have the latest version ({current})." Indicator cleared. |
| Check failed (offline, GitHub unavailable) | Message: "Couldn't check for updates. Check your internet connection and try again." |

On **Update**:

1. Disable Play and Check for update.
2. Show download progress as a percentage.
3. When the download completes, apply the update and restart the launcher on the new version.
4. If the download fails, show an error, re-enable the buttons, and leave the installed version untouched.

## States

| State | Play | Check for update | Indicator |
|---|---|---|---|
| Idle, up to date | Enabled | Enabled | Hidden |
| Idle, update known | Enabled | Enabled | Shown |
| Checking | Enabled | Disabled | Unchanged |
| Downloading | Disabled | Disabled | Shown |
| Applying | Disabled | Disabled | Shown |
| Not installed (development run) | Enabled | Disabled | Hidden |

## Out of scope

- News, patch notes, settings, accounts.
- Animation.
