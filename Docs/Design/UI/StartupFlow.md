# Startup Flow

**Status:** Draft

What the player sees from launching the game to starting play.

## Sequence

1. **Splash logos**, shown by Unity's built-in splash screen ([Unity Manual: splash screen](https://docs.unity3d.com/Manual/class-PlayerSettingsSplashScreen.html)), one after another:
   1. "Made with Unity"
   2. "Pj's Gameroom" (placeholder studio logo)
2. **Main menu.**

## Main menu

| Element | Position | Notes |
|---|---|---|
| Background | Full screen | Placeholder beach scene ([Asset Credits](../../Engineering/AssetCredits.md)). |
| Title | Top | "SqueezySurvival" (working title). |
| Menu | Left, below the title | New Game, Continue, Load Game, Options, Quit. |
| Version | Bottom right | For example `v0.0.1`. In the Unity Editor it reads `(editor)` after the number, because the version is only stamped by a release build ([Release Pipeline](../../Engineering/ReleasePipeline.md)). |

### Menu options

| Option | Does | Unavailable when |
|---|---|---|
| New Game | Starts a new run, after choosing [run options](../Systems/Runs.md#run-options) and [handedness](../Systems/Equipment.md#handedness). | Never. |
| Continue | Resumes the most recent run in progress. | No run is in progress. |
| Load Game | Lists runs in progress and [casual saves](../Systems/Runs.md#casual-runs) to choose from. | Nothing can be loaded. |
| Options | Opens settings (controls, audio, display, accessibility, units). | Never. |
| Quit | Closes the game. | Never. |

An unavailable option stays visible but disabled, and shows its reason in place of its label (for example "No game in progress" in place of Continue), so state is never shown by color alone ([Pillar 4](../GameDesignDocument.md#pillars)).

### Keyboard and controller

The first available option has focus when the menu opens, so the menu works without a mouse. Escape closes any open panel.

## New Game screen

Opened by New Game. Sets up the run before it starts. **The screen opens fresh every time:** nothing chosen on a previous visit is remembered.

```
NEW GAME
Dominant hand:   ( ) Left   ( ) Right
Run options (optional)
                 Leave these unticked to play the game in its default, intended mode.
                 Option          Effect on play                              Points
                 [ ] Casual mode Save and load any time; death is not final  No points
                 [ ] Easy mode   Slower hunger, gentler penalties,           1/5 survival points,
                                 slower decay                                no challenge points
                 [ ] Hard mode   Faster hunger, things break more often,     Double points
                                 harsher weather
This run earns:  1 survival point per day, 1 challenge point per 30 days
                                          [ Back ]  [ Start ]
```

| Element | Behavior |
|---|---|
| Dominant hand | Left or Right ([Equipment: handedness](../Systems/Equipment.md#handedness)). **Required, with no default:** nothing is selected until the player chooses. |
| Run options | A table: each [run option](../Systems/Runs.md#run-options) checkbox, its effect on play, and its effect on points. Run options are optional: with none ticked, the run is an unmodified default game, and the screen says so beneath the heading. Every option is always clickable. The effects column updates with the choices: while Casual is ticked, Easy's effect reads "No points (Casual mode)", because Casual's no points overrides Easy's reduced points. While Casual or Easy is ticked, Hard is shown dimmed; ticking Hard anyway clears Casual and Easy. While Hard is ticked, Casual and Easy are shown dimmed; ticking either clears Hard. |
| This run earns | The points the chosen options earn, updated as boxes are ticked, so the cost of each option is visible before starting ([Runs: points earned](../Systems/Runs.md#points-earned)). |
| Back | Returns to the main menu. Escape does the same. |
| Start | Unavailable until a dominant hand is chosen; until then it reads "Choose a dominant hand". Opens the [start confirmation](#start-confirmation); confirming starts the run: Day 1, 6:00 AM, on the Beach ([Time: start of a run](../Systems/Time.md#start-of-a-run)). |

| Options | This run earns |
|---|---|
| None | 1 survival point per day, 1 challenge point per 30 days |
| Easy | 1 survival point per 5 days, no challenge points |
| Hard | 2 survival points per day, 2 challenge points per 30 days |
| Casual (with or without Easy) | No points |

### Start confirmation

A last chance to confirm, stating the game type and what it earns, with **Go back** and **Start** buttons. Go back and Escape return to the New Game screen.

| Options | Message |
|---|---|
| None | "You are starting an unmodified default game. You will earn 1 survival point per day survived, and 1 challenge point per 30 days survived. Are you sure?" |
| Easy | "You are starting an Easy mode game. You will earn 1 survival point per 5 days survived, and no challenge points. Are you sure?" |
| Hard | "You are starting a Hard mode game. You will earn 2 survival points per day survived, and 2 challenge points per 30 days survived. Are you sure?" |
| Casual, or Casual and Easy | "You are starting a Casual mode game (or Casual and Easy mode game). You will be awarded no points for this adventure. Are you sure?" |

## Starting the run

Every choice on the New Game screen is data, not decoration.

1. While the screen is open, `NewGameSetup` holds only what the checkboxes currently show and applies their combination rules. It is screen state: nothing is recorded, and it is discarded on Back.
2. The run's choices are recorded only when Start is pressed **and** the confirmation is confirmed. That creates the `NewRunConfiguration`: the dominant hand, the run options, the point rates they imply, and the start of the run (Day 1, 6:00 AM, Beach). It cannot be created without a dominant hand.
3. The configuration is placed in the `RunLaunchHandoff`, a service that outlives scene changes, and the game scene takes it when it starts the run. Taking it empties the handoff, so one confirmation starts exactly one run.
4. The configuration is written **once**, when the run's save is created, and only read after that ([Runs: saving](../Systems/Runs.md#saving)). It is never rewritten by later saves. Protecting saves from editing outside the game is designed with the save system.

## Wireframe stage

The main menu and New Game screen are built. Choosing an option whose screen does not exist yet opens a short notice: "This isn't built yet." Confirming Start hands off the run configuration for real; because the game scene does not exist yet, the notice then shows what was handed off, for example "Run configured: Left-handed, Hard mode. Day 1, 6:00 AM, Beach." Saving does not exist yet, so Continue and Load Game are always unavailable for now.
