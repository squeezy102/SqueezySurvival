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

## Wireframe stage

Only the menu itself is built so far. Choosing an option whose screen does not exist yet opens a short notice: "This isn't built yet." Saving does not exist yet, so Continue and Load Game are always unavailable for now.
