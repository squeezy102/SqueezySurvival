# HUD

**Status:** Concept

Always-visible status shown on every gameplay screen. Follows [Pillar 4: clarity](../GameDesignDocument.md#pillars).

## Static status area (top of screen)

| Element | Form | Notes |
|---|---|---|
| Body temperature | "Thermometer and body" icon | The survivor's own temperature. Shown separately from the survival stat bars because both extremes are bad. How the icon communicates its state: [OQ-020](../OpenQuestions.md). |
| Outside temperature | Number with unit, for example `84°F` | Ambient air temperature. Unit is a player setting: Fahrenheit (default) or Celsius. |
| Weather | Icon | Current weather state. |

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

## Survival stat bars

Shown as bars, separate from the static status area. Conventions: [Survival Stats](../Systems/SurvivalStats.md#conventions).
