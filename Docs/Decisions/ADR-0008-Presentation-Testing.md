# ADR-0008: Presentation testing

**Status:** Accepted
**Date:** 2026-10-21

## Context

Every decision lives in Core and is unit tested, but Presentation still binds events, enables and disables controls, sends commands and routes keys. That code can break, and it has no tests: there is no Presentation test assembly ([#126](https://github.com/squeezy102/SqueezySurvival/issues/126)). All submitted code is tested ([Testing and Quality Standards](../Engineering/TestingStandards.md)).

Presentation tests must drive UI Toolkit elements and the Input System the way a player would, without a scene, in Edit Mode, so they run with every other test.

## Decision

### A Presentation test assembly

| Assembly | Folder | References |
|---|---|---|
| `SqueezySurvival.Presentation.Tests` | `Assets/SqueezySurvival/Tests/Presentation/` | Core, Presentation, test runners, UI Test Framework, Input System test framework |

Edit Mode only, like the other test assemblies ([ADR-0007](ADR-0007-Runtime-UI-And-Assembly-Layout.md#assemblies)).

### UI: Unity's UI Test Framework

Add `com.unity.ui.test-framework` 6.4.0, the version that matches the editor ([Unity Manual: UI Test Framework](https://docs.unity3d.com/6000.4/Documentation/Manual/com.unity.ui.test-framework.html)).

- Tests derive from `UITestFixture`, which provides an empty panel to populate in the test; presenters take a root element, so a test loads the real `GameScreen.uxml` into that panel and hands its root to the presenter ([UI Test Framework: test fixtures](https://docs.unity3d.com/Packages/com.unity.ui.test-framework@6.4/manual/test-fixtures/introduction-to-test-fixtures.html), [testing without an Editor window](https://docs.unity3d.com/Packages/com.unity.ui.test-framework@6.4/manual/test-fixtures/test-ui-without-editor-window-dependencies.html)).
- Input is simulated with the fixture's `simulate` API: clicks, key presses, drags and scrolls, then `simulate.FrameUpdate()` ([UI Test Framework: simulate UI interactions](https://docs.unity3d.com/Packages/com.unity.ui.test-framework@6.4/manual/simulate/simulate-ui-interaction-landing.html)).

### Keys read through the Input System: its test framework

Escape is read with an Input System action ([#48](https://github.com/squeezy102/SqueezySurvival/issues/48)). Tests for it derive from the Input System's `InputTestFixture`, which adds a test keyboard and presses keys on it. This needs `com.unity.inputsystem` in the manifest's `testables`; no new package ([Input System: testing](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.19/manual/Testing.html)).

### What is tested

- Every presenter and view: it shows Core's state, refreshes on Core's events, sends the right command, and stops listening when disposed.
- Adapters with no Unity dependency, such as `ReadOnlyListViewItemsSource`, are tested directly.
- The composition root is covered by one test that composes the real game screen and checks every presenter is wired.
- Decisions are still made in Core and tested there; a Presentation test checks the decision reaches the screen, not the decision itself.

## Alternatives considered

- **Hand-built input events** (sending `MouseDownEvent` and `MouseUpEvent` to an element). Works in some cases, but depends on undocumented event order and panel state; forum threads show it failing silently ([Unity Discussions: click a Button from a unit test](https://discussions.unity.com/t/click-uielements-button-from-unit-test/883473)). Rejected in favor of Unity's supported API.
- **Play Mode tests of the game scene.** Closest to a real session, but slow, need a scene and a run handoff, and are harder to isolate. Kept for later end-to-end checks, not for each presenter.
- **No Presentation tests; keep Presentation thin and review it.** Rejected: thin is not empty, and the project tests all submitted code.

## Consequences

- `Packages/manifest.json` gains `com.unity.ui.test-framework` and a `testables` entry; [ADR-0003](ADR-0003-Package-Baseline.md) lists the package.
- Presentation changes come with Presentation tests; the [definition of done](../Engineering/TestingStandards.md#definition-of-done) says so.
- Presentation code must be constructible in a test: built from a root element and injected collaborators, never by finding objects in a scene.
