# Options

**Status:** Draft

The Options screen holds the player's settings.

## Where settings come from

Every setting has up to three values:

| Value | Belongs to | Set by |
|---|---|---|
| **Run setting** | One save file | Changing the setting during that run |
| **Preference** | The player, across every run | "Capture these settings as my global preference" |
| **Default** | The game | The design documents |

Settings are local to a run: changing one during a run changes it for that save file only. A preference may be the same as the default or different.

**When a game loads,** each setting takes the first of these that exists: the save file's run setting, then the player's preference, then the default. A new run, which has no run settings yet, therefore starts from the preferences.

## Buttons in every settings menu

Every settings menu in the game has two buttons that act on the settings that menu shows:

| Button | Does |
|---|---|
| **Return to default settings** | Sets every setting in the menu back to its default, for this run. |
| **Capture these settings as my global preference** | Saves the menu's current settings as the player's preferences, used by every run that has no run setting of its own. |

**From the main menu**, with no run loaded, the Options screen edits the global preference itself (resolves OQ-070), and it has only **Return to default settings**. A note at the top of every Options screen says which settings it changes:

| Opened from | Note |
|---|---|
| Main menu | "These settings are your global preference. Every game uses them unless that game has its own settings. Order of precedence: the game's own settings, then your global preference, then the defaults." |
| During a run | "These settings apply to this game only. To use them in every new game, capture them as your global preference." |

## Opening it

| From | How |
|---|---|
| Main menu | **Options** |
| During a run | **Options** in the pause menu (Escape), or **Options** in the nav bar |

**Back** or Escape closes it and returns to where it was opened. There is no Apply button: every change applies at once and is kept automatically ([where settings come from](#where-settings-come-from)).

## Wireframe stage

The Options screen opens from the main menu, the pause menu and the nav bar, with its note. It has no settings yet: the grid layout settings it first held were removed when the grids became fixed ([#49](https://github.com/squeezy102/SqueezySurvival/issues/49)). Run settings, the global preference and the two buttons are built with the first settings, alongside saving ([#46](https://github.com/squeezy102/SqueezySurvival/issues/46)).

## Sections

| Section | Settings | Status |
|---|---|---|
| Display | Temperature unit, real-world clock, event log auto-hide ([HUD](HUD.md)). | Planned ([#7](https://github.com/squeezy102/SqueezySurvival/issues/7)) |
| Gameplay | End of day summary ([Day Summary](DaySummary.md)). | Planned (#7) |
| Controls | Key remapping ([Controls](Controls.md)). | Planned (#7) |
