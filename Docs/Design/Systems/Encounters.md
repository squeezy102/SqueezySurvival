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

## Fight or flight

Every encounter except a pure discovery presents a fight-or-flight choice with risk and reward.

- **Fight:** opens a [combat](Combat.md) encounter.
- **Flight:** avoid or leave. Risks of fleeing an encounter (for example the thief escapes with the item, or the animal pursues) are defined per encounter.

The choice, its risks and rewards are shown before the player commits ([Pillar 4: clarity](../GameDesignDocument.md#pillars)).
