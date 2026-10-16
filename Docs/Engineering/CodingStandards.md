# Coding Standards

**Status:** Approved

## Non-negotiables

1. **All code is unit testable.** Game logic lives in plain C# classes with no dependency on `MonoBehaviour`, scenes or the Unity runtime, so it can be tested in Edit Mode without entering Play Mode.
2. **Warnings are defects.** Compiler, analyzer and Unity Console warnings are fixed when they appear. A warning that cannot be fixed is suppressed at the narrowest scope with a comment stating why, and recorded in the changelog.
3. **Documentation is part of done.** See the [documentation policy](../Home.md#documentation-policy).

## Architecture

- **Simulation and presentation are separate.** The simulation (time, stats, inventory, recipes) never references UI or Unity components. The presentation layer observes simulation state and sends player commands. This is what allows animation to be added later without changing game rules.
- **Dependency injection.** Classes receive their dependencies through constructors, typed as interfaces. Composition happens at a single composition root per scene.
- **Data-driven content.** Items, resources, recipes and biomes are defined as data (ScriptableObjects), not hard-coded.
- **Deterministic simulation.** Randomness and time come from injected interfaces (for example `IRandomNumberSource`, `IGameClock`) so tests are repeatable.
- **Patterns are tools, not goals.** Use factories, builders, strategies, observers and similar patterns where they reduce coupling or simplify testing. Avoid global singletons for game state.

## Assemblies

Code is split into assembly definitions (`.asmdef`) so that dependencies are explicit and enforced by the compiler. The specific layout is defined when the first code is written and recorded in an ADR.

## Naming

- Names describe both what a thing is and its purpose: `SurvivalStatDecayCalculator`, not `StatHelper`.
- No abbreviations except universally standard ones (`UI`, `Id`).
- Interfaces are prefixed `I`. Test classes are named `<ClassUnderTest>Tests`.
- Follow Microsoft C# naming conventions: PascalCase for types and members, camelCase for locals and parameters, `_camelCase` for private fields.

## Comments

- XML doc comments on public types and members state purpose and contract.
- Inline comments explain why, never what.
- No commented-out code. No TODO, FIXME or HACK comments; open work is tracked in documentation or issues.

## Testing

- Unity Test Framework, Edit Mode tests for simulation logic.
- Arrange-Act-Assert structure; one behavior per test.
- Test names describe the behavior: `DrinkCoconutWater_WhenWaterIsLow_RaisesWater`.
- Play Mode tests only where Unity runtime behavior is the subject under test.
