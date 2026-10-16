# Combat

**Status:** Concept

Turn-based combat with wildlife, entered from an [encounter](Encounters.md). Rough design; expected to be refined.

## Presentation

Like a classic Final Fantasy overworld battle: a new window opens with the survivor on the left and the animal on the right. The player always acts first.

## Actions

Each turn both sides choose one of three actions:

| Action | Meaning |
|---|---|
| Fight | Attack. |
| Defend | Guard against an attack. |
| Flee | Try to leave the fight. |

The player chooses freely each turn. The player's Flee succeeds with a chance equal to their Health percentage ([Injuries and Health](Injuries.md)).

## Animal decks

Every animal has a hidden 10-card deck made of Fight, Defend and Flee cards. Each turn it draws a card at random and commits to that action. The player never sees the deck.

| Animal | Fight | Defend | Flee | Personality |
|---|---|---|---|---|
| Bird | 1 | 1 | 8 | Skittish |
| Goat | 2 | 2 | 6 | Wary |
| Mountain lion | 6 | 3 | 1 | Predator |

## Weapons reshape the deck

A successful attack turns one of the animal's cards into a **dud**, depending on the weapon's class. A dud stays in the deck; if the animal draws it, it does nothing and the player gets a free turn. The event log explains why.

| Weapon class | Examples | Card turned into a dud | Effect | Example dud text |
|---|---|---|---|---|
| Melee | Knife, sword | Defend | Breaks the animal's guard. | "The goat stumbled, unable to brace itself." |
| Reach | Spear, axe | Fight | Keeps the animal at bay. | "The tiger couldn't get around your spear to attack you!" |
| Ranged | Stone, sling, bow | Flee | Stops the animal escaping. | "The gull tried to take off, but your stone had clipped its wing." |

Duds rather than removal: removing a card shrinks the deck and makes every other card more likely (removing a Fight card raises the chance to Flee). A dud keeps the deck at 10 cards, so a hit only ever makes the targeted action rarer.

**Duds last for the whole fight**; they never revert. Fights are expected to be short, and losing hard-won progress would feel bad and make no sense.

There are three weapon classes: **Melee, Reach and Ranged.** Every weapon shows its class as an icon, and its class is named on hover.

Weapon choice is therefore tactical: a ranged weapon against a skittish bird, a spear against a predator.

## Proposals to complete the design (not approved)

### Turn resolution ([OQ-038](../OpenQuestions.md))

| Player \ Animal | Fight | Defend | Flee |
|---|---|---|---|
| **Fight** | Both attack. The player's attack resolves first. | Player's attack is blocked; no dud is made. | Player's attack resolves first; unless it defeats the animal, the animal escapes. |
| **Defend** | Animal's attack is blocked. | Nothing happens. | The animal escapes. |
| **Flee** | Flee roll; on failure the animal's attack lands. | Flee roll. | Both separate; the encounter ends. |

### Hit chance

An attack's hit chance comes from the fighting skill for the weapon's class, lowered by injuries (an injured tool-side arm lowers it). Values are set during balancing.

| Weapon class | Skill |
|---|---|
| Melee | Melee fighting |
| Reach | Reach fighting |
| Ranged | Ranged fighting |

See [Skills](Skills.md).

## Deck draws

Cards are drawn without replacement: a drawn card cannot be drawn again until every card has been drawn, then the whole deck is reshuffled and reused (resolves OQ-039). Duds remain duds through reshuffles. Attentive players can infer what remains in the deck, which rewards skill without ever showing it.

### Damage ([OQ-037](../OpenQuestions.md))

Hits lower current Health and can cause injuries ([Injuries and Health](Injuries.md)). Damage values for weapons and animals, animal health, and which injuries each animal inflicts are not yet designed.
