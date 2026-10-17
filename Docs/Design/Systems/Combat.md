# Combat

**Status:** Draft

Turn-based combat with wildlife, entered from an [encounter](Encounters.md). Expected to be refined.

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

There is no hit roll: the matchup of actions alone decides what succeeds (resolves OQ-042). Randomness lives only in the animal's hidden draw, before the player decides, so a correct read is never undone by a dice roll ([Game Developer: the 2 types of randomness](https://www.gamedeveloper.com/game-platforms/the-2-types-of-randomness-and-how-to-apply-them-to-your-board-game)).

## Animal decks

Every animal has a hidden 10-card deck made of Fight, Defend and Flee cards. Each turn it draws a card and commits to that action. The player never sees the deck.

| Animal | Fight | Defend | Flee | Personality |
|---|---|---|---|---|
| Bird | 1 | 1 | 8 | Skittish |
| Goat | 2 | 2 | 6 | Wary |
| Mountain lion | 6 | 3 | 1 | Predator |

### Deck draws

Cards are drawn without replacement: a drawn card cannot be drawn again until every card has been drawn, then the whole deck is reshuffled and reused (resolves OQ-039). Attentive players can infer what remains in the deck, which rewards skill without ever showing it.

### Duds

A successful attack turns one of the animal's cards into a **dud**, depending on the weapon's class. If the animal draws a dud, it does nothing and the player gets a free turn. The event log explains why.

- Duds rather than removal: removing a card shrinks the deck and makes every other card more likely (removing a Fight card raises the chance to Flee). A dud keeps the deck at 10 cards, so a hit only ever makes the targeted action rarer.
- **Duds last for the whole fight**, through reshuffles. Fights are expected to be short, and losing hard-won progress would feel bad and make no sense.

## Weapons

There are three weapon classes. Every weapon shows its class as an icon, named on hover. A weapon's class decides its handedness (resolves OQ-041) and which card a hit turns into a dud.

| Class | Handedness | Weapons | Offhand | Card turned into a dud | Effect | Example dud text |
|---|---|---|---|---|---|---|
| Melee | One-handed | Knives, short swords, hatchets, hand axes, clubs | Free: a shield if desired | Defend | Breaks the animal's guard. | "The goat stumbled, unable to brace itself." |
| Reach | Two-handed | Spears, long swords, axes, mauls | Unavailable | Fight | Keeps the animal at bay. | "The tiger couldn't get around your spear to attack you!" |
| Ranged | Two-handed | Slings, bows | Holds the ammunition: a quiver or ammo pouch | Flee | Stops the animal escaping. | "The gull tried to take off, but your stone had clipped its wing." |

Weapon choice is tactical: a ranged weapon against a skittish bird, a spear against a predator.

### Throwing stones

A survivor fighting **empty-handed** with stones in their inventory chooses how to Fight each turn (resolves OQ-044):

| Choice | Class |
|---|---|
| Unarmed | Melee |
| Throw a stone | Ranged |

- This is the only situation in which the player chooses between Melee and Ranged. Holding anything in the tool hand means fighting with it.
- A thrown stone is a **very weak** attack. Enough to bring down a seagull with several hits before it flies off, but little else.
- Each throw uses one stone from the inventory. There is a decent chance it can be recovered afterwards: it lands in the zone's [ground pool](Zones.md#ground-pool); otherwise it is lost.

Throwing stones outside combat: [Resource Nodes: throwing stones](ResourceNodes.md#throwing-stones).

**Gear is locked for the fight.** Whatever the survivor wears when an encounter starts is what they fight in, for the whole fight. Choosing between gathering gear and combat gear is covered in [Equipment: loadouts](Equipment.md#loadouts).

## Skill and information

| Progress | Gives | Grows by |
|---|---|---|
| Weapon skill (Melee, Reach or Ranged fighting) | Damage dealt by a successful attack | Fighting with that weapon class |
| Defense skill | Damage blocked by Defend | Successful defends |
| Creature familiarity | Tells about that species | Encounters with that species |

Injuries to the tool-side arm lower damage dealt. See [Skills](Skills.md).

### Defense

- Defense is a [skill](Skills.md), raised by successful defends.
- A shield in the offhand raises defense further, but only alongside a one-handed (Melee) weapon. Shields range from improvised (driftwood, a pot lid) to crafted shields.

### Tells

A tell is a hint shown before the player chooses. Tells are always truthful.

- **Rule-out tell:** rules out exactly one of the animal's three options. Example: "The gull isn't looking for a fight." The player still guesses, between two options.
- **Explicit tell (mastery only):** states the animal's action. Example: "The tiger crouches low, ready to pounce!" Available only once the species is mastered, and only on some turns. Shown with an icon for the action - sword (Fight), shield (Defend), boot (Flee) - so it is readable without relying on color. Later, the creature's sprite changes pose instead (for example a crouching tiger).

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

## Damage

Hits lower current Health and can cause injuries ([Injuries and Health](Injuries.md)). Damage values for weapons and animals, animal health, and which injuries each animal inflicts: [OQ-057](../OpenQuestions.md).

## Turn resolution

Resolves OQ-038.

| Player \ Animal | Fight | Defend | Flee |
|---|---|---|---|
| **Fight** | Both attack. The player's attack resolves first. | Player's attack is blocked; no dud is made. | Player's attack resolves first; unless it defeats the animal, the animal escapes. |
| **Defend** | Animal's attack is blocked. | Nothing happens. | The animal escapes. |
| **Flee** | Flee roll; on failure the animal's attack lands. | Flee roll. | Both separate; the encounter ends. |

- **A dud is a free turn:** the player's attack lands, or their Flee succeeds automatically.
- **A Ranged hit stops a fleeing animal escaping**, as its dud rule describes ([Weapons](#weapons)).

## Tracking

When an animal escapes, the player can try to **track** it.

- Success is more likely the more injured the animal is, and the higher the survivor's Tracking [skill](Skills.md).
- Tracking is trained by tracking.
- **Each attempt takes 15 minutes**, longer under action penalties such as darkness, storms, fog or injury ([Actions: availability](Actions.md#availability)).
- **When found, the fight resumes where it left off**: the animal's injuries and duds carry over.
- **Badly hurt animals may already have collapsed** when found. No fight is needed: a sharp tool turns a collapsed animal into an animal carcass.

| Animal health when found | Chance it has collapsed |
|---|---|
| Below 50% | 50% |
| Below 25% | 75% |

- **An animal can't be chased forever.** Each time an animal escapes and survives, it has a chance to get away for good, and that chance grows with every escape. An animal that gets away is gone from the game, and the player is told: "You see no sign of the animal. It seems to have gotten away."

How injury and Tracking skill combine into the chance of finding the animal: [OQ-062](../OpenQuestions.md). How likely an escaping animal is to get away for good, and how fast that grows: [OQ-065](../OpenQuestions.md).
