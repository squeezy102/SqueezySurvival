# Encounters

**Status:** Draft

Random encounters with wildlife while the survivor is outside.

## Triggers

- Any action can trigger an encounter while the survivor is outside.
- Each zone defines its own encounter table.
- An encounter is only possible when its animal's existence conditions are met (for example day phase: diurnal, nocturnal or crepuscular; weather; zone).
- Encounter chance scales with the action's duration, so long actions are not safer per minute than short ones ([OQ-036](../OpenQuestions.md)).

## Encounter types

| Type | Examples | What happens |
|---|---|---|
| Thief | A seagull or monkey comes for your things. | It tries to steal an item. Fight or flight decides whether you keep it. |
| Sighting | You see a snake, a scorpion, a beehive. | You notice it first and choose whether to engage or avoid. |
| Accident | You step on a snake or scorpion; you bump into a beehive. | An immediate consequence (for example a sting), then fight or flight. |
| Discovery | You happen across an ant mound or termite hill. | A feature is found; may become a [resource node](ResourceNodes.md). |

## Ambushes

Encounters are opt-in. In almost every encounter the survivor sees the animal first and can flee. An ambush is the exception: a forced [combat](Combat.md) encounter with no option to flee at the start.

- **Only ambush predators ambush.** Some aggressive predators (for example the mountain lion) are tagged as ambush predators. No other animal can force combat.
- **The first one's free.** An ambush predator cannot ambush the survivor in a zone until the survivor has had a sighting of that species in that zone. A first meeting is always a sighting.
- **Known threats are shown.** Once the survivor has seen an ambush predator in a zone, the zone shows that this animal is there and when it was seen (for example "Mountain lion - seen at Dusk"). An ambush can only happen when the player ignores that information.
- **Darkness invites ambushes.** When an encounter with a known ambush predator comes up, the [light level](Actions.md#light-level) decides how likely it is to be an ambush rather than a sighting (resolves OQ-045, draft values):

| Light level | Ambush share |
|---|---|
| Bright | 5% (1 in 20) |
| Dim | 15% (about 1 in 7) |
| Dark | 50% (a coin flip) |

A light source such as a torch raises the light level, and so lowers the share. Nocturnal predators are also most active at Night ([Time: what phases affect](Time.md#what-phases-affect)), so wandering in the dark where predators are known to live is asking for trouble.

In daylight, exploring a dangerous zone should feel like "probably not, but what if?" The danger never comes out of nowhere ([Pillar 4: clarity](../GameDesignDocument.md#pillars)).

## Fight or flight

Every encounter except a pure discovery or an ambush presents a fight-or-flight choice with risk and reward.

- **Fight:** opens a [combat](Combat.md) encounter.
- **Flight:** avoid or leave. Risks of fleeing an encounter (for example the thief escapes with the item, or the animal pursues) are defined per encounter.

The choice, its risks and rewards are shown before the player commits ([Pillar 4: clarity](../GameDesignDocument.md#pillars)).
