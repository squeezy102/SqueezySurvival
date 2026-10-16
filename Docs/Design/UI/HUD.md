# HUD

**Status:** Concept

Always-visible status shown on every gameplay screen. Follows [Pillar 4: clarity](../GameDesignDocument.md#pillars).

## Static status area (top of screen)

| Element | Form | Notes |
|---|---|---|
| Body temperature | "Thermometer and body" icon | The survivor's own temperature. Shown separately from the survival stat bars because both extremes are bad. See [Body temperature icon](#body-temperature-icon). |
| Outside temperature | Number with unit, for example `84°F` | Ambient air temperature. Unit is a player setting: Fahrenheit (default) or Celsius. |
| Weather | Icon | Current weather state. |

## Body temperature icon

| State | Thermometer fill | Color | Extra marking (MVP, static) | Future animation |
|---|---|---|---|---|
| Too cold | Lower third | Blue | Frost or shiver lines | Shivering |
| Comfortable | Middle third | Green | None | None |
| Too hot | Upper third | Red | Sweat drops | Sweating |

- Color is never the only signal: fill height and markings carry the same meaning, so the icon stays readable with red-green color blindness.
- Hovering shows the exact body temperature and its band.
- MVP uses static icon variants; animation replaces them later without changing game logic (resolves OQ-020).

## Weather icons

| Weather | Category |
|---|---|
| Sunny | Normal |
| Cloudy | Normal |
| Light rain | Normal |
| Heavy rain | Normal |
| Storm | Normal |
| Hurricane | Disaster event |
| Tornado | Disaster event |
| Hail | Disaster event |

The weather states and their effects are specified in Weather (planned, [OQ-009](../OpenQuestions.md)).

## Event log

A running ticker on screen that records everything that happens, in plain language.

```
You discovered 5 sticks.
You picked up 5 sticks.
You put a seashell on the ground.
You cut the stick into an arrow using your flint knife.
You felt strain in your muscles...
You cut the stick into an arrow using your flint knife.
Your muscles ache...
You cut the stick into an arrow using your flint knife.
You pulled a muscle!
```

- Every game event that changes state is logged: discoveries, item movements, actions and their outcomes, stat warnings, injuries, weather changes.
- **Every sound cue has a matching log entry**, so sound is never the only signal ([Pillar 4](../GameDesignDocument.md#pillars)).
- Warnings and serious events are marked with an icon as well as color.
- May auto-hide when idle. Default behavior and access to the full history: [OQ-031](../OpenQuestions.md).

## Survival stat bars

Shown as bars, separate from the static status area. Conventions: [Survival Stats](../Systems/SurvivalStats.md#conventions).
