# HUD

**Status:** Concept

Always-visible status shown on every gameplay screen. Follows [Pillar 4: clarity](../GameDesignDocument.md#pillars). Positions are defined in [Screen Layout](ScreenLayout.md).

## Static status area (top of screen)

| Element | Form | Notes |
|---|---|---|
| Body temperature | "Thermometer and body" icon | The survivor's own temperature. Shown separately from the survival stat bars because both extremes are bad. See [Body temperature icon](#body-temperature-icon). |
| Outside temperature | Number with unit, for example `84°F` | Ambient air temperature. Unit is a player setting: Fahrenheit (default) or Celsius. |
| Weather | Icon | Current weather state. |
| Game clock | Day, time and day phase on three rows, for example `Day 3`, `7:45:12 AM`, `Morning` | See [Time](../Systems/Time.md). |
| Real-world clock | The player's local time | Optional; player setting, off by default. |
| Idle research | Atom with "zzz" icon | Shown only while nothing is researching. See [Research: idle research](../Systems/Research.md#idle-research). |

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

One icon per [weather state](../Systems/Weather.md#weather-states). Severe and disaster states also carry a warning marker, so their severity is never shown by color alone.

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
- Warnings and serious events are marked with an icon as well as color: a warning entry is red, starts with a red warning triangle, and says in words why it is a warning (for example, a cone snail find adds "You've heard cone snails are very, very dangerous. Best to leave them alone.").
- Positioned lower left.
- Hides automatically when idle and reappears when a new event arrives. Auto-hide is a player setting, on by default.
- **Important events are highlighted** for a couple of seconds, both the entry and the log window: bad (red), good (green), notification (yellow). Each also carries an icon, so color is never the only signal. The MVP uses a static highlight (colored border, icon and bold text for a few seconds); pulsing and glowing arrive with animation, together with a reduce-motion setting (resolves OQ-034).
- **Unread warnings:** every new warning starts unread. While any warning is unread, the log's border pulses red and a red **!** sits beside the log's heading, with the number unread beside it when there is more than one ("! 3"). The pulse is not a flash, so the reduced-flashing option does not change it.
  - Clicking the **!** scrolls to the oldest unread warning and marks it read. Each click takes the player to the next one; when none are left, the **!** disappears and the pulse stops.
  - A warning also counts as read once the player scrolls the log (mouse wheel, scroll bar or the jump-to-newest arrow) and the warning is fully in view. A warning that arrives on screen is still unread, so it always alerts.
- **Following the newest entry:** while the log is scrolled to the bottom, new entries scroll into view as they arrive. Once the player scrolls up, the log stays where they left it.
- **Jump to newest:** hovering the bottom edge of the log shows a down arrow; clicking it scrolls to the newest entry and follows new entries again.
- **Header controls:** clicking the top of the log offers pin, hide, configure, and open the full history.
- **Full history view:** the whole run, searchable and filterable by type (actions, finds, warnings) (resolves OQ-031).

### Voice

Entries are written in second person, past tense: **what happened, then how it felt.** The second sentence conveys the consequence in human terms, hinting at which stat was affected without numbers.

| Event | Entry |
|---|---|
| Ate an earthworm | "You ate an earthworm. It was revolting." |
| Broke a leg | "You broke your leg. You can feel yourself beginning to panic." |
| Arms tiring | "Your arms ache." |
| Picked up items | "You picked up 5 sticks." |

- Routine events (picking up, putting down) need only the first sentence.
- Events that change Sanity, Contentment or health always get the second sentence.
- Exact values are available on hover, never in the sentence itself.

## Survival stat bars

Shown as bars, separate from the static status area. Conventions: [Survival Stats](../Systems/SurvivalStats.md#conventions).
