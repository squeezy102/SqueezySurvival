# Playtest Log

**Status:** Approved

An index of playtest sessions. Every finding from playing a build is filed as a [GitHub issue](WorkTracking.md) labeled `playtest`, which records its resolution and the commit that fixed it. This page lists each session's findings and links to their issues. Newest session first.

## 2026-10-16: startup flow and game screen wireframe

Played in the Unity Editor and as a local build. Findings were filed as issues after the session.

| # | Finding | Type | Issue |
|---|---|---|---|
| 1 | Main menu cut off at the edges in the Game view. | Not a bug | [#14](https://github.com/squeezy102/SqueezySurvival/issues/14) |
| 2 | Black strip along the shoreline in the background. | Bug | [#15](https://github.com/squeezy102/SqueezySurvival/issues/15) |
| 3 | Reasons written under greyed-out menu options felt cluttered. | Design change | [#16](https://github.com/squeezy102/SqueezySurvival/issues/16) |
| 4 | Splash logos did not appear when pressing Play in the editor. | Not a bug | [#17](https://github.com/squeezy102/SqueezySurvival/issues/17) |
| 5 | Unity 6.4's Build Profiles window offered no Build And Run without creating a build profile. | Tooling | [#18](https://github.com/squeezy102/SqueezySurvival/issues/18) |
| 6 | Console exception during builds: "Exception occurred attempting to connect to Unity services. Native symbols will not be uploaded". | Environment | [#19](https://github.com/squeezy102/SqueezySurvival/issues/19) |
| 7 | Dominant hand defaulted to Right. | Design change | [#20](https://github.com/squeezy102/SqueezySurvival/issues/20) |
| 8 | Leaving and reopening the New Game screen kept earlier choices. | Bug | [#21](https://github.com/squeezy102/SqueezySurvival/issues/21) |
| 9 | Run option rules were hard to read from the checkbox labels. | Design change | [#22](https://github.com/squeezy102/SqueezySurvival/issues/22) |
| 10 | Checkbox labels turned grey when clicked. | Bug | [#23](https://github.com/squeezy102/SqueezySurvival/issues/23) |
| 11 | Hard mode could not be clicked while Casual or Easy was ticked. | Design change | [#24](https://github.com/squeezy102/SqueezySurvival/issues/24) |
| 12 | Unclear whether a run option had to be chosen. | Clarification | [#25](https://github.com/squeezy102/SqueezySurvival/issues/25) |
| 13 | With Casual and Easy both ticked, Easy still claimed reduced points. | Bug | [#26](https://github.com/squeezy102/SqueezySurvival/issues/26) |
| 14 | Starting a run needed a last chance to review. | Design change | [#27](https://github.com/squeezy102/SqueezySurvival/issues/27) |
| 15 | The menu's choices had to drive real game data. | Design change | [#28](https://github.com/squeezy102/SqueezySurvival/issues/28) |
| 16 | The game screen had no nav bar. | Bug | [#29](https://github.com/squeezy102/SqueezySurvival/issues/29) |
| 17 | No way to save and quit a run. | Bug | [#30](https://github.com/squeezy102/SqueezySurvival/issues/30) |
| 18 | Empty space below the ground panel, and uneven gaps between panels. | Bug | [#31](https://github.com/squeezy102/SqueezySurvival/issues/31) |
| 19 | The event log did not scroll to new entries. | Bug | [#32](https://github.com/squeezy102/SqueezySurvival/issues/32) |
| 20 | A local build left changes in `ProjectSettings` (Unity services switch and Input System preloaded asset). | Tooling | [#10](https://github.com/squeezy102/SqueezySurvival/issues/10) |
| 21 | The cloud session cannot upload Git LFS files (images). | Tooling | [#11](https://github.com/squeezy102/SqueezySurvival/issues/11) |
| 22 | Reaching the game screen took several clicks every time. | Tooling | [#33](https://github.com/squeezy102/SqueezySurvival/issues/33) |
| 23 | Panels resized as the event log grew. | Bug | [#34](https://github.com/squeezy102/SqueezySurvival/issues/34) |
| 24 | Nothing marks the end of a day; play felt monotonous. | Design change | [#4](https://github.com/squeezy102/SqueezySurvival/issues/4) |
