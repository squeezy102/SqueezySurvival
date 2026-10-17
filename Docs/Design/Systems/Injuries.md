# Injuries and Health

**Status:** Draft

Health and injuries are modeled on Haven and Hearth's wound system ([Ring of Brodgar wiki: Wounds](https://ringofbrodgar.com/wiki/Wounds)), adapted to this game. Expected to be adjusted as development proceeds.

## Health

Health is an ordinary health bar: a red bar with the numbers beside it, current health over maximum health, for example **85/90** (resolves OQ-067).

The bar's full length is the survivor's uninjured maximum. From left to right it shows:

| Part | Meaning |
|---|---|
| Red | Current health. |
| Empty | Health lost to damage, which regenerates over time. |
| Darkened | Maximum health taken by injuries, which returns only as they heal. |

At 85/90 out of an uninjured 100, the bar is 85% red, 5% empty and 10% darkened.

| Value | Lowered by | Restored by | At zero |
|---|---|---|---|
| Current health | Damage: blows, falls, combat hits | Regeneration and eating (see [Recovering health](#recovering-health)) | [Knocked out](#knocked-out) |
| Maximum health | Injuries, each by its severity, for as long as it lasts | Healing the injury, over time or with treatment | Death |

**Order:** a hit's damage lowers current health first; then any injury it caused lowers maximum health. If maximum health falls below current health, current health drops to match.

Example, starting from 100/100:

| Event | Bar |
|---|---|
| Hit for 5 damage | 95/100 |
| Hit for 10 damage, causing a laceration that lowers maximum health by 10 | 85/90 |

Game text always says "health". Haven and Hearth equivalents: current health is Soft HP (SHP); maximum health is Hard HP (HHP), lowered by injuries.

### Recovering health

Current health regenerates only while Food and Water are both above Dangerous ([Survival Stats: bands](SurvivalStats.md#bands-draft)). How fast depends on what the survivor is doing ([Time: passing time deliberately](Time.md#passing-time-deliberately), [sheltered variants](Time.md#sheltered-variants)):

| Doing | Regeneration |
|---|---|
| Anything | About 1 per in-game hour |
| Wait or Rest, in the open | Barely faster |
| Wait or Rest, sheltered | A little faster |
| Nap or Sleep, sheltered, on a bed | Much faster |

Eating also helps: every food item eaten, whatever it is, restores 1 current health ([Food](Food.md)).

Exact rates: [OQ-069](../OpenQuestions.md). Napping or sleeping in the open or without a bed: [OQ-068](../OpenQuestions.md).

### Knocked out

As in Haven and Hearth, when current health reaches zero the survivor is **knocked out**, and falls wherever they are.

- **For at least 1 hour,** long enough to regenerate 1 current health. The bigger the overkill (damage beyond zero), the longer it lasts.
- **The survivor suffers a concussion**, an injury like any other.
- **If it happened in battle,** the battle ends there, and the injuries suffered in it take effect as well ([Combat: injuries in battle](Combat.md#injuries-in-battle)).
- Time passes while the survivor is out, and wildlife may take advantage.

How overkill sets the length: [OQ-069](../OpenQuestions.md).

## Injuries

Examples: bruises, sprains, abrasions, lacerations, concussions, goring, broken bones, hemorrhaging, stings and bites, sunburn ([Weather](Weather.md#clear-and-fair-weather)).

Each injury has:

| Property | Description |
|---|---|
| Type | What it is (sprain, laceration, ...). |
| Severity | How much it lowers maximum health. |
| Location | One of six body locations, or the whole body (hemorrhaging, venom). See [Body locations](#body-locations). |
| Effects | Consequences beyond health, for example a broken leg slows or blocks movement; a lacerated arm makes crafting harder. |
| Course | How it heals over time. Most improve steadily; some worsen before improving (venom); some only heal with treatment. |
| Treatments | What can treat it. |

### Body locations

| Location | Injuries there affect |
|---|---|
| Head | The injury's own effects (for example a concussion slows complex actions) |
| Torso | The injury's own effects (for example a goring) |
| Left arm, right arm | The hand on that side: the tool hand or the offhand ([Equipment](Equipment.md)) |
| Left leg, right leg | Movement and fleeing |
| Whole body | The injury's own effects (hemorrhaging, venom, burns) |

The body silhouette shows each injury and its treatment at its location.

### Healing

- Each injury heals independently, at its own pace.
- Healing an injury gives back the maximum health it took.
- Some healing requires conditions (for example being well fed and rested).

### Escalation

Neglected minor injuries can combine into worse ones. Example chain: abrasion, then laceration, then deep laceration. Treatment reduces the chance of escalation.

### Treatment

First aid is a core skill. Treatments speed healing, stop escalation or cure specific injuries. A treatment attaches to the injury at its location (a bandage goes on the laceration on the right arm). Many carry a trade-off: some block equipment at that location ([Equipment](Equipment.md#treatments-and-equipment)); a crude poultice stings, costing sanity or contentment.

## Combat

The player's Flee chance equals their Health percentage ([Combat](Combat.md)). A battered survivor struggles to escape.

Injuries suffered in battle are announced as they happen but take effect only after the battle ends ([Combat: injuries in battle](Combat.md#injuries-in-battle)).
