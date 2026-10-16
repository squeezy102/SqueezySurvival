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

The player chooses freely each turn. The player's Flee succeeds with a chance equal to their health percentage ([OQ-037](../OpenQuestions.md)).

## Animal decks

Every animal has a hidden 10-card deck made of Fight, Defend and Flee cards. Each turn it draws a card at random and commits to that action. The player never sees the deck.

| Animal | Fight | Defend | Flee | Personality |
|---|---|---|---|---|
| Bird | 1 | 1 | 8 | Skittish |
| Goat | 2 | 2 | 6 | Wary |
| Mountain lion | 6 | 3 | 1 | Predator |

## Weapons reshape the deck

A successful attack also removes one card from the animal's deck, depending on the weapon's range.

| Weapon class | Examples | Card removed on a successful hit | Effect |
|---|---|---|---|
| Close melee | Knife, sword | Defend | Breaks the animal's guard. |
| Reach melee | Spear, axe | Fight | Keeps the animal at bay. |
| Ranged | Stone, sling, bow | Flee | Stops the animal escaping. |

Weapon choice is therefore tactical: a ranged weapon against a skittish bird, a spear against a predator.

## Proposals to complete the design (not approved)

### Turn resolution ([OQ-038](../OpenQuestions.md))

| Player \ Animal | Fight | Defend | Flee |
|---|---|---|---|
| **Fight** | Both hit. The player's attack lands first. | Player's attack is blocked; no card removed. | Player hits; the animal escapes unless the hit removes its last Flee card or defeats it. |
| **Defend** | Animal's attack is blocked. | Nothing happens. | The animal escapes. |
| **Flee** | Flee roll; on failure the animal's attack lands. | Flee roll. | Both separate; the encounter ends. |

### Deck draws ([OQ-039](../OpenQuestions.md))

Cards are drawn without replacement and the discard pile is reshuffled when the deck runs out. Cards removed by weapons are gone for the rest of the fight. Attentive players can therefore infer what remains in the deck, which rewards skill without ever showing it.

### Health and damage ([OQ-037](../OpenQuestions.md))

Combat needs a survivor health value and damage values for weapons and animals. These are not yet designed.
