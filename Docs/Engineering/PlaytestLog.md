# Playtest Log

**Status:** Approved

Every finding from playing a build: bugs, confusions and design changes, with what was decided and what changed. Design decisions live in their owning documents; this log records how they were found. Newest session first.

## 2026-10-16: startup flow and game screen wireframe

Played in the Unity Editor and as a local build.

| # | Finding | Type | Resolution |
|---|---|---|---|
| 1 | Main menu cut off at the edges in the Game view. | Not a bug | The Game view was previewing 2560 x 1440 in a smaller window. Use the **16:9 Aspect** setting; the UI scales to any screen. |
| 2 | Black strip along the shoreline in the background. | Bug | An outdated copy of the generated background reached the project. Replaced with the corrected image. |
| 3 | Reasons written under greyed-out menu options felt cluttered. | Design change | A greyed-out option shows its reason in place of its label ([Startup Flow](../Design/UI/StartupFlow.md#menu-options)). |
| 4 | Splash logos did not appear when pressing Play in the editor. | Not a bug | Unity shows the splash only in a built player. Added **SqueezySurvival > Build > Build And Run Locally** ([Project Setup](ProjectSetup.md#setup-commands)). |
| 5 | Unity 6.4's Build Profiles window offered no Build And Run without creating a build profile. | Tooling | Covered by the same local build command; no build profile asset is needed. |
| 6 | Console exception during builds: "Exception occurred attempting to connect to Unity services. Native symbols will not be uploaded". | Environment | The developer's DNS filter (NextDNS) blocked Unity's crash reporting host. Fixed by allowlisting `*.unity3d.com`. The game does not use Unity crash reporting. |
| 7 | Dominant hand defaulted to Right. | Design change | No default; the choice is required, and Start reads "Choose a dominant hand" until it is made ([Equipment: handedness](../Design/Systems/Equipment.md#handedness)). |
| 8 | Leaving and reopening the New Game screen kept earlier choices. | Bug | The screen opens fresh every time ([Startup Flow](../Design/UI/StartupFlow.md#new-game-screen)). |
| 9 | Run option rules were hard to read from the checkbox labels. | Design change | Run options became a table: option, effect on play, effect on points. |
| 10 | Checkbox labels turned grey when clicked. | Bug | Unity's default theme recolors a focused field's label. Overridden so labels change only when an option conflicts. |
| 11 | Hard mode could not be clicked while Casual or Easy was ticked. | Design change | Every option is always clickable; conflicting options are dimmed, and choosing one clears the conflict ([Startup Flow](../Design/UI/StartupFlow.md#new-game-screen)). |
| 12 | Unclear whether a run option had to be chosen. | Clarification | No option is required: none ticked is the default, intended game. The screen now says so. |
| 13 | With Casual and Easy both ticked, Easy still claimed reduced points. | Bug | Easy's effect reads "No points (Casual mode)" while Casual is ticked. |
| 14 | Starting a run needed a last chance to review. | Design change | Added the start confirmation, stating the game type and what it earns ([Startup Flow](../Design/UI/StartupFlow.md#start-confirmation)). |
| 15 | The menu's choices had to drive real game data. | Design change | Choices are recorded only on confirmed Start, as a `NewRunConfiguration` handed to the game, and written once when the run's save is created ([Startup Flow](../Design/UI/StartupFlow.md#starting-the-run)). |
| 16 | The game screen had no nav bar. | Bug | Added, with placeholder windows ([Screen Layout](../Design/UI/ScreenLayout.md#wireframe-stage)). |
| 17 | No way to save and quit a run. | Bug | Escape opens a pause menu holding Save and exit; until saving is built it exits without keeping the run. |
| 18 | Empty space below the ground panel, and uneven gaps between panels. | Bug | Rebuilt the layout so panels stretch to fill the screen with a uniform 16 px margin and gap. |
| 19 | The event log did not scroll to new entries. | Bug | The log scrolls to the newest entry whenever its content grows. |
| 20 | A local build left changes in `ProjectSettings` (Unity services switch and Input System preloaded asset). | Tooling | Reverted after the build; recorded as open issues in [Release Pipeline](ReleasePipeline.md#open-issues). |
| 21 | The cloud session cannot upload Git LFS files (images). | Tooling | Commits that add binary files are pushed from the developer's PC ([Source Control](SourceControl.md)). |
| 22 | Reaching the game screen took several clicks every time. | Tooling | In the editor and development builds, a placeholder save makes Continue start a brand-new default run ([Startup Flow](../Design/UI/StartupFlow.md#wireframe-stage)). Release builds never include it. |
| 23 | Panels resized as the event log grew. | Bug | Panel sizes now come from the screen only; content scrolls inside its panel ([Screen Layout](../Design/UI/ScreenLayout.md#wireframe-stage)). |
| 24 | Nothing marks the end of a day; play felt monotonous. | Design change | Concept for an end-of-day summary screen ([Day Summary](../Design/UI/DaySummary.md), [OQ-064](../Design/OpenQuestions.md)). Not built yet. |
