# Testing and Quality Standards

**Status:** Approved

How SqueezySurvival is tested, and what "done" means for quality. These standards apply to every contributor and to every AI assistant working in this repository ([AGENTS.md](../../AGENTS.md)).

## Sources

The standards follow recognized industry syllabi. Terms below are used as these sources define them.

| Source | Used for |
|---|---|
| [ISTQB Certified Tester Foundation Level (CTFL) v4.0](https://istqb.org/certifications/certified-tester-foundation-level-ctfl-v4-0/) (2023) | Testing principles, test levels and types, test design techniques, the test pyramid, defect reports |
| [ISTQB Certified Tester Game Testing (CT-GaMe) v1.0.1](https://istqb.org/certifications/certified-tester-game-testing-ct-game/) ([syllabus](https://istqb.org/wp-content/uploads/2024/11/ISTQB_CT_GaMe_Syllabus_v1.0.1_LtrKuyi.pdf)) | Game-specific testing: mechanics, game state and saves, graphics, sound, levels, controllers, localization, playtesting |
| [Game Accessibility Guidelines](https://gameaccessibilityguidelines.com/full-list/) | Accessibility checks (Basic tier is the baseline; see [Controls: accessibility](../Design/UI/Controls.md#accessibility)) |

## Principles

The seven testing principles of CTFL v4.0, and what each means here:

| Principle | In practice |
|---|---|
| Testing shows the presence, not the absence, of defects | Passing tests are evidence, not proof. Playtesting continues alongside them. |
| Exhaustive testing is impossible | Test cases are chosen with the techniques below, not by trying everything. |
| Early testing saves time and money | Designs are reviewed before code ([documentation policy](../Home.md#documentation-policy)); acceptance criteria are written with the issue. |
| Defects cluster together | A system that produced a defect gets extra tests and exploratory attention. |
| Tests wear out | Tests are reviewed when the code they cover changes; repeated playtests vary their approach. |
| Testing is context dependent | A single-player PC game: no server, anti-cheat or multiplayer testing; saves, mechanics and UI matter most. |
| Absence-of-defects fallacy | Bug-free is not enough: the game must also match its design and be fun. Playtests judge that. |

## Test levels

| Level | Here | Who | How |
|---|---|---|---|
| Component | One Core class | Author | Edit Mode unit tests |
| Component integration | Core systems working together, such as `ZoneExplorationService` exploring a zone through a run built by `RunSessionFactory` | Author | Edit Mode unit tests |
| System | A build or Play mode session | squeezy102, contributors | Playtests, logged in the [Playtest Log](PlaytestLog.md) |
| Acceptance | An issue's acceptance criteria | squeezy102 | Checked before the pull request into `main` is merged |

## The test pyramid

Most tests are fast Edit Mode unit tests of Core, which has no Unity references ([ADR-0007](../Decisions/ADR-0007-Runtime-UI-And-Assembly-Layout.md)). Fewer tests cover systems together. Manual playtesting sits on top. Play Mode tests are written only where Unity runtime behavior itself is under test. Presentation code is kept thin so little of it needs testing.

## Designing test cases

Test cases are derived with CTFL techniques, so each one has a reason to exist:

| Technique | Use it for | Example |
|---|---|---|
| Equivalence partitioning | Inputs that behave alike | One case for "any dominant hand chosen", not one per hand. |
| Boundary value analysis (two-value) | Limits and thresholds | Each boundary and its nearest neighbor: 23 and 24 explores of a 24-explore zone. |
| Decision table testing | Combinations of conditions | Run options: casual, easy and hard together. |
| State transition testing | Behavior that depends on what came before | Menus, drag and drop, the pause menu. |
| Branch coverage (white-box) | Core logic | Every branch of Core logic is reached by at least one test. |
| Error guessing and exploratory testing | Playtests | Try to break it: drag outside the window, click rapidly, resize, press Escape mid-action. |

### Every test must be able to fail

- Every test can fail for a real defect. No tests of constants, of .NET or Unity themselves, or of argument-null guard clauses.
- No test cases that exercise the same partition or branch as another; each boundary and each distinct branch gets one case.
- Arrange-Act-Assert structure; one behavior per test. Test names describe the behavior: `Explore_FullyExploredZone_StillFindsItemsAndFeatures`.
- Randomness and time are injected, so tests are repeatable ([Coding Standards: architecture](CodingStandards.md#architecture)).

## Confirmation and regression testing

- **Confirmation testing:** every fixed bug gets a test that fails without the fix, where the defect is in testable code; otherwise its issue records how the fix was checked.
- **Regression testing:** the full test suite runs before every commit and in Unity before every pull request. Every changed system is replayed in the next playtest.

## Reading results without Unity

Many contributors work through an AI assistant and cannot read Unity's windows for it. So after every compile and every test run, the editor writes what happened to the `Diagnostics/` folder at the project root (git-ignored), and assistants read it from there:

| File | Written | Holds |
|---|---|---|
| `Diagnostics/latest-compilation.md` | After every script compile | Succeeded or failed; every error and warning with its file, line and assembly |
| `Diagnostics/latest-test-summary.md` | After every test run, from the Test Runner window or anywhere else | Passed or failed; counts; every test that did not pass, with its message, stack trace and output. If tests could not start, for example because scripts do not compile, it says so and why |
| `Diagnostics/latest-test-results.xml` | After every test run | The full results in NUnit XML |

- Check the timestamp in each file: it is the last compile or run, not necessarily the one just asked for.
- Compile errors stop tests from running, so read `latest-compilation.md` first.
- Written by `SqueezySurvival.Editor.Diagnostics`, using the Test Framework's run callbacks and `TestRunnerApi.SaveResultToFile` ([Unity Manual: retrieve test results](https://docs.unity3d.com/Manual/test-framework/extension-get-test-results.html), [TestRunnerApi](https://docs.unity3d.com/Packages/com.unity.test-framework@1.6/api/UnityEditor.TestTools.TestRunner.Api.TestRunnerApi.html)) and Unity's `CompilationPipeline` events ([Unity Scripting API: CompilationPipeline.assemblyCompilationFinished](https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Compilation.CompilationPipeline-assemblyCompilationFinished.html)).

## Static testing

Reviews find defects before they are built: design documents are reviewed through pull requests before code is written, and every code change is reviewed by squeezy102 before it reaches `main` ([Source Control](SourceControl.md)).

## Game testing

From CT-GaMe, applied to this game:

| Area | What to check |
|---|---|
| Game mechanics | Behavior matches its design document. Values shown to the player match the values the game uses (for example, a tooltip's weight matches the item's real weight). Mechanics combine without breaking each other. |
| Game state and saves | Saving and loading restore every visible and hidden value. Each save point is loaded and checked. |
| Graphics and UI | Layout follows [Screen Layout](../Design/UI/ScreenLayout.md); nothing overlaps, clips or resizes unexpectedly; icons are readable at every grid size. |
| Sound | Once sound exists: effects match actions, and the mix keeps feedback audible. |
| Zones (levels) | Every zone is reachable and its content matches its design. |
| Controls | Every input works with mouse and keyboard; remapped controls work everywhere. |
| Localization | Not planned. Text lives in Core formatters so it can be localized later. |
| Negative testing | Try to skip costs or exploit rules: actions without their requirements, dragging to invalid places. |
| Performance | Once content grows: frame rate, freezes and memory on low-end hardware. |
| Risk-based priority | The first session (startup, New Game, the first day) gets the most attention, because it decides whether a player continues. |
| Playtesting | Playtests judge clarity and fun, not only correctness. Findings are logged and filed as issues. |

## Defect reports

Every defect is filed as an issue before it is worked on ([Work Tracking: lifecycle](WorkTracking.md#lifecycle)). The bug form collects what CTFL v4.0 lists for a defect report: a title, steps to reproduce, expected and actual results, the version, the environment, severity, and references to the design. squeezy102 sets priority through milestones.

## Definition of done

A change is done when:

1. Its acceptance criteria are met.
2. Core logic has tests designed as above, and every test passes in Unity's Test Runner.
3. There are zero compiler and Console warnings.
4. Every affected document is updated in the same pull request.
5. It has been played in the editor or a build, and new findings are filed as issues.
