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

### Success, skill and information

There is no hit roll: the matchup of actions alone decides what succeeds (resolves OQ-042). Randomness lives only in the animal's hidden draw, before the player decides, so a correct read is never undone by a dice roll ([Game Developer: the 2 types of randomness](https://www.gamedeveloper.com/game-platforms/the-2-types-of-randomness-and-how-to-apply-them-to-your-board-game)).

| Progress | Gives | Grows by |
|---|---|---|
| Weapon skill (Melee, Reach or Ranged fighting) | Damage dealt by a successful attack | Fighting with that weapon class |
| Defense skill | Damage blocked by Defend | Successful defends |
| Creature familiarity | Tells about that species | Encounters with that species |

Injuries to the tool-side arm lower damage dealt. See [Skills](Skills.md).

### Tells

A tell is a hint shown before the player chooses. Tells are always truthful.

- **Rule-out tell:** rules out exactly one of the animal's three options. Example: "The gull isn't looking for a fight." The player still guesses, between two options.
- **Explicit tell (mastery only):** states the animal's action. Example: "The tiger crouches low, ready to pounce!" Available only once the species is mastered, and only on some turns.
- **Explicit tell presentation:** the text is accompanied by an icon for the action - sword (Fight), shield (Defend), boot (Flee). Later, the creature's sprite changes pose instead (for example a crouching tiger). Icon plus text keeps the tell readable without relying on color.

Tells come from creature familiarity (proposed progression):

| Encounters with the species | Tells |
|---|---|
| First | None: pure guessing |
| About 3 | Occasional |
| About 10 | Frequent; the bestiary records its temperament ("Skittish: usually flees") |
| Many | The bestiary shows its rough card makeup |
| Mastery | About half of turns give an explicit tell; the rest give rule-out tells or none |

### Bestiary

Records each species the survivor has encountered. Entries appear only after a first encounter and fill in as familiarity grows ([You only know what you know](../GameDesignDocument.md#pillars)).

## Defense

- Defense is a [skill](Skills.md), raised by successful defends.
- A shield in the offhand raises defense further, but only alongside a one-handed weapon. Shields range from improvised (driftwood, a pot lid) to crafted shields.
- Weapons are one-handed or two-handed. Which weapons are two-handed: [OQ-041](../OpenQuestions.md).

## Deck draws

Cards are drawn without replacement: a drawn card cannot be drawn again until every card has been drawn, then the whole deck is reshuffled and reused (resolves OQ-039). Duds remain duds through reshuffles. Attentive players can infer what remains in the deck, which rewards skill without ever showing it.

### Damage ([OQ-037](../OpenQuestions.md))

Hits lower current Health and can cause injuries ([Injuries and Health](Injuries.md)). Damage values for weapons and animals, animal health, and which injuries each animal inflicts are not yet designed.
