# Architecture

**Status:** Draft, for squeezy102's approval ([#116](https://github.com/squeezy102/SqueezySurvival/issues/116))

How the game's runtime code is organized: what owns what, how time and changes flow, how a scene is put together, and how each part is tested. The [Coding Standards](CodingStandards.md) state the principles; this document is how they apply to this codebase. The assembly layout is defined in [ADR-0007](../Decisions/ADR-0007-Runtime-UI-And-Assembly-Layout.md).

Code that does not yet match this document is listed in [Known gaps](#known-gaps), each with its issue. New code follows this document, not the gaps.

## The shape of the game

```
 Player input                                   What the player sees
      |                                                  ^
      v                                                  |
 Presentation  ---- commands ---->  Core  ---- change events (what changed) ----+
 (scene, views,                     (the run: rules, state, time)
  composition root)  <---- reads current state -----------------------------------
```

- **Core** decides everything: what an action does, how long it takes, what it changes, and the words that describe it. It has no Unity references; the compiler enforces that.
- **Presentation** shows Core's state and turns player input into commands. It makes no game decisions.
- Core never calls Presentation. Presentation learns about changes only through Core's events.

## Core

### The run

A run is a small object that owns a set of focused services and wires them together. Each service has one job, its own tests, and receives its collaborators through its constructor as interfaces ([Coding Standards: architecture](CodingStandards.md#architecture)).

| Service | Owns | Examples of what it does |
|---|---|---|
| Game time | The clock and the one path that advances it | Advance by an action's duration; tell every time-dependent system |
| Survivor's belongings | Inventory, hands, and the load they make | Report load and its state; refuse nothing on weight ([Inventory](../Design/Systems/Inventory.md#weight)) |
| Item moves | Moving things between panels | Preview a move; carry it out; edge holds |
| Exploration | Exploring zones | One explore: finds, features, ground overflow |
| Zones | Each zone's state | Ground pool, features, exploration progress |
| Event log | What happened, in order | Add entries; unread warnings |

Services are added the same way as systems are designed: survival stats, weather and research each become a service with its own rules, not more methods on the run.

### Time: one path

Every action advances time through **one entry point**, as [Time: implementation notes](../Design/Systems/Time.md#implementation-notes) requires:

1. The action is validated and its duration worked out as an `ActionDuration` (`FromMinutes` or `FromSeconds`; never less than 1 second).
2. The action's own effects are applied (things move, finds are added), and its event log entries are written, stamped with the time the action ends. Writing them first keeps an action's own entries ahead of anything a time-dependent system logs about the time that passed.
3. `GameTimeAdvancer.Advance` moves the clock by the duration and tells each **time-dependent system** (`ITimeDependentSystem`) in a fixed order. Each receives a `GameTimeAdvance`: the clock before, the clock after and the exact duration.
4. `TimeAdvanced` is raised for Presentation, carrying the same `GameTimeAdvance`.

**Order of time-dependent systems.** The game scene's composition root passes them to `GameTimeAdvancer`'s constructor as an ordered list, so the order is set in one place and changed only deliberately. Order, as they arrive: survival stats, then weather, then research. None exist yet.

**Rules.**

- No other code changes the clock. `GameClock` is immutable; `GameTimeAdvancer` is its only owner. The run's commands hold `IGameTimeAdvancer`; everything else, Presentation included, sees the read-only `IGameTime`.
- A time-dependent system never reads the clock to work out how much time passed; it uses the duration it is given. This keeps the simulation deterministic and each system testable on its own.
- Time cannot advance while an advance is being applied: `Advance` called from a time-dependent system or a `TimeAdvanced` handler throws. A reaction to time passing belongs to the action that caused it.

### Commands, state and change events

- **Commands** are ordinary methods on services (`MoveItems`, `Explore`, `PassTime`). A command checks it is allowed, changes state, and raises events. A command that is not allowed throws; Presentation asks first (`CanExplore`, `PreviewItemMove`) so the player is told why before acting.
- **Change events say what changed**: which slots of a collection, which entry was added, which stat moved. A listener never has to compare the whole state to find out ([#121](https://github.com/squeezy102/SqueezySurvival/issues/121)).
- Events are raised **after** state is consistent, synchronously, on the thread that issued the command. A handler must not issue a command from inside an event; if a reaction needs one, Core does it as part of the original command.
- Events follow .NET's event pattern: a typed payload describing the change ([Microsoft: handling and raising events](https://learn.microsoft.com/en-us/dotnet/standard/events/)).

### Rules, content and layout

- **Rules** are numbers and decisions the design documents own: inventory slots, durations, weight limits, hold timings. Each lives in one Core rules class that names the document it comes from. Rules never depend on screen layout; grid shapes are derived from rules, not the other way round ([#123](https://github.com/squeezy102/SqueezySurvival/issues/123)).
- **Content** (items, zones, find pools, features) is data, not code ([Coding Standards](CodingStandards.md#architecture)). Its format and how it reaches Core are decided in an ADR ([#124](https://github.com/squeezy102/SqueezySurvival/issues/124)). Core sees content only as definitions it is given.
- **Words** shown to the player come from Core formatters next to the rules they describe, so wording is tested with the rules.

## Presentation

### One composition root per scene

Each scene has one composition root (a MonoBehaviour) that builds everything the scene needs, Core services and views alike, and hands each its collaborators. Nothing else in the scene creates services. The main menu works this way already; the game scene is being brought in line ([#119](https://github.com/squeezy102/SqueezySurvival/issues/119)).

Cross-scene services live in `GameSessionHost`, the one static access point in the game, kept only because Unity scenes cannot pass objects to each other directly. It holds hand-offs between scenes, never game state.

### Presenters and views

- **One presenter per panel or screen**: the HUD clock, the event log, each inventory grid, the pause menu, the nav bar. A presenter listens to Core's events, updates its elements, and turns input into commands.
- **Screen state is Core logic.** Which overlay is open and what Escape closes first is decided by a Core class and unit tested; the presenter only shows the result ([#119](https://github.com/squeezy102/SqueezySurvival/issues/119)).
- Layout is UXML, style is USS, behavior is C# ([ADR-0007](../Decisions/ADR-0007-Runtime-UI-And-Assembly-Layout.md)). Scenes and UI assets are generated by setup commands ([Project Setup](ProjectSetup.md#setup-commands)).

### Element lifetime

Creating elements and adding them to the hierarchy is slow; Unity advises building elements once and reusing them rather than creating and discarding them ([Unity Manual: best practices for managing elements](https://docs.unity3d.com/2022.1/Documentation/Manual/UIE-best-practices-for-managing-elements.html), [Unity 6: optimizing UI performance](https://docs.unity.com/en-us/engine/6000.3/manual/best-practice-guides/bpg-uiad-index/optimizing-performance)).

- **Fixed-size things are built once** and updated in place: grid cells ([#114](https://github.com/squeezy102/SqueezySurvival/issues/114)), tooltips ([#122](https://github.com/squeezy102/SqueezySurvival/issues/122)), HUD fields.
- **Long or growing lists are virtualized** with `ListView`, which pools and recycles elements as the player scrolls (same sources). The event log is the first: a `ListView` with dynamic row heights (entries wrap), bound to the log's entries through a read-only items source, so the view never copies or changes Core's list. Whether the log follows new entries (`EventLogFollowState`) and when a warning counts as in view (`ScrolledIntoViewCalculator`) are Core logic.
- **A change updates only what changed**, using the detail in Core's change events.
- **Textures are cached**, made once per icon and destroyed with their cache (`PixelArtTextureCache`).

### Work over time

- Nothing runs every frame or on a timer unless the player is in the middle of something that needs it: a drag, an edge hold, a pulse. Scheduled work starts with the interaction and stops when it ends.
- While it runs, it recomputes only when an input changes (the pointer moved, a second passed), not on every tick.

## Testing

- **Core** is covered by Edit Mode unit tests designed to the [Testing and Quality Standards](TestingStandards.md). Every rule, command and formatter.
- **Presentation** holds no decisions that can live in Core. What must stay (binding, element updates, input routing) is covered by a Presentation Edit Mode test assembly ([#126](https://github.com/squeezy102/SqueezySurvival/issues/126)).
- A design document change that alters a rule changes its rules class and that class's tests in the same pull request.

## Known gaps

Found in the architecture review of 2026-10-20 ([Architecture pass milestone](https://github.com/squeezy102/SqueezySurvival/milestone/5)). Fixed in this order; each fix updates this document and removes its row.

| Gap | Issue |
|---|---|
| `GameScreenController` composes the scene and presents everything | [#119](https://github.com/squeezy102/SqueezySurvival/issues/119) |
| `RunSession` holds every service's job | [#120](https://github.com/squeezy102/SqueezySurvival/issues/120) |
| Change events don't say what changed | [#121](https://github.com/squeezy102/SqueezySurvival/issues/121) |
| The tooltip recreates its elements on every show | [#122](https://github.com/squeezy102/SqueezySurvival/issues/122) |
| Capacities come from grid layouts | [#123](https://github.com/squeezy102/SqueezySurvival/issues/123) |
| Content is hard-coded | [#124](https://github.com/squeezy102/SqueezySurvival/issues/124) |
| The game scene's collaborators aren't injected | [#125](https://github.com/squeezy102/SqueezySurvival/issues/125) |
| Screen-side logic isn't tested | [#126](https://github.com/squeezy102/SqueezySurvival/issues/126) |
| Moves assume two panels | [#127](https://github.com/squeezy102/SqueezySurvival/issues/127) |
| Smaller cleanups | [#128](https://github.com/squeezy102/SqueezySurvival/issues/128) |
