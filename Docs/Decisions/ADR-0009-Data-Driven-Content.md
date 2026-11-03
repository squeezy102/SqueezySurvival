# ADR-0009: Data-driven content

**Status:** Proposed
**Date:** 2026-11-03

## Context

Items, zone features and zones, with their find pools and scripted first explores, are written in C# in `PlaceholderBeachZoneContent`. The [Coding Standards](../Engineering/CodingStandards.md#architecture) require content to be data, not code, and the [architecture review](../Engineering/Architecture.md#known-gaps) lists this as a gap ([#124](https://github.com/squeezy102/SqueezySurvival/issues/124)).

The forces:

- **Core has no Unity references** ([ADR-0007](ADR-0007-Runtime-UI-And-Assembly-Layout.md)). It cannot load assets or call Unity's serializer, so whatever reads the content in Presentation must hand Core something plain.
- **Contributors work through AI assistants and pull requests** ([ADR-0006](ADR-0006-Public-Source-And-Contribution-Workflow.md)). Content must be readable and editable as text, and its changes reviewable in a diff.
- **Core is compile-checked outside Unity** ([Testing and Quality Standards: checking Core without Unity](../Engineering/TestingStandards.md#checking-core-without-unity)). That sandbox reaches the operating system's package mirror but not NuGet or Unity's package registry, so Core cannot depend on a library it can only get from either.
- **No package without an ADR** ([ADR-0003](ADR-0003-Package-Baseline.md)).
- Content will grow: more zones, recipes, resource nodes. Content mistakes (a misspelled id, a weight of 0) must be caught by tests, not found in play.

## Decision

### Content is JSON, imported as text assets

Content lives in `Assets/SqueezySurvival/Content/` as JSON files. Unity imports `.json` files as text assets ([Unity Manual: text assets](https://docs.unity3d.com/6000.4/Documentation/Manual/class-TextAsset.html)). One file per kind of content, plus one per zone:

| File | Holds |
|---|---|
| `Items.json` | Every item definition |
| `ZoneFeatures.json` | Every zone feature definition |
| `Zones/<Zone>.json` | One zone: its name, explores to map, find pool, feature pool and scripted explores, naming items and features by id |

Keys match the record fields below, in PascalCase, following the project's C# naming ([Coding Standards: naming](../Engineering/CodingStandards.md#naming)). An example of one item and part of a zone:

```json
{
  "Items": [
    {
      "Id": "stone",
      "Name": "Stone",
      "SingularPhrase": "a stone",
      "PluralName": "stones",
      "Description": "A small, ordinary stone. Good for knapping, campfires and much else; it may break when knapped.",
      "Category": "StoneAndMinerals",
      "WeightPounds": 1.0,
      "FoundWarning": ""
    }
  ]
}
```

```json
{
  "Id": "beach",
  "Name": "Beach",
  "ExploresToMap": 24,
  "FindPool": [ { "Id": "stone", "Weight": 30 } ],
  "FeaturePool": [ { "Id": "small-palm-tree", "Weight": 3 } ],
  "ScriptedExplores": [ { "Finds": [ "stone", "stone", "small-stick" ] } ]
}
```

### Presentation parses with Unity's JsonUtility; Core builds and checks

Reading content takes two steps, each in the layer that can do it:

1. **Parse (Presentation).** A Presentation reader turns each text asset into a plain record with `JsonUtility.FromJson` ([Unity Manual: JSON serialization](https://docs.unity3d.com/6000.4/Documentation/Manual/json-serialization.html)). The record classes live in Core: `[Serializable]` classes with public fields only. `System.SerializableAttribute` is .NET's, not Unity's, so Core stays free of Unity references.
2. **Build (Core).** A Core builder takes the records and returns a content catalog of the existing immutable definitions (`ItemDefinition`, `ZoneFeatureDefinition`, `ZoneDefinition`). It checks every record, resolves every id a zone names, and reports **every** problem in one exception, each naming the file and id ("Items.json, item 'stone': unknown category 'Stones'"), so one fix pass covers them all.

The record shapes keep to what Unity's serializer supports ([Unity Manual: serialization rules](https://docs.unity3d.com/6000.4/Documentation/Manual/script-serialization-rules.html)):

- No `decimal`: it is not among the supported types, so `WeightPounds` is a `double` that the builder converts with `Convert.ToDecimal` ([Microsoft: Convert.ToDecimal](https://learn.microsoft.com/en-us/dotnet/api/system.convert.todecimal)). A test checks that the shipped weights convert to exactly the values written.
- Enums are written as names, not numbers, so a diff reads "StoneAndMinerals", not "2". The record holds a string; the builder parses it and rejects unknown names.
- No nested lists: each scripted explore is a record holding its list of finds.
- No dictionaries: pools are lists of id and weight.

Records are a file format, not a model: nothing outside the builder and the reader uses them.

### How the content reaches the game

The game scene's composition root holds the text assets as serialized references, set by **SqueezySurvival > Setup > Game Screen** like the rest of the generated scene ([Project Setup: setup commands](../Engineering/ProjectSetup.md#setup-commands); [Unity Scripting API: SerializedObject](https://docs.unity3d.com/6000.4/Documentation/ScriptReference/SerializedObject.html)). It reads and builds the catalog once, then passes definitions into the run as it does today. Which zone a run starts in is a rule ([Zones](../Design/Systems/Zones.md): the Beach is the starting zone), held in Core, not chosen by Presentation.

The `Resources` folder is not used: Unity advises against it beyond prototyping because it slows startup and grows the build ([Unity Manual: Resources folder best practices](https://docs.unity.cn/2023.3/Documentation/Manual/BestPracticeUnderstandingPerformanceInUnity6.html)).

### Content values have one home

The content files are the home of each thing's values: its names, description, weight, category, its weights in a pool, a zone's explores to map. Design documents keep intent, targets and ranges (what an item is for, how long zones take to map) and link to the files instead of restating values ([AGENTS.md: one home per fact](../../AGENTS.md#non-negotiable-rules)).

### Tests

- **Core:** the builder is unit tested: each kind of problem (empty text, unknown category or permanence, weight at and below its lower bound, duplicate id, unknown id in a pool or scripted explore, empty scripted explore) and a valid build.
- **Presentation:** the reader is tested with small JSON strings, and one test loads the shipped content files and builds the catalog, so a content mistake fails a test before it reaches a build. The check that every item and feature has a placeholder icon moves there.
- **No other test depends on shipped content.** Tests build their own definitions, so balancing a weight never breaks an unrelated test.

## Alternatives considered

- **ScriptableObjects mapped to Core definitions in Presentation.** Unity's own data-container asset, edited in the Inspector, with asset references checked by the editor ([Unity Manual: ScriptableObject](https://docs.unity3d.com/6000.4/Documentation/Manual/class-ScriptableObject.html)). Not chosen: assets reference each other by GUID in YAML, which is hard to review in a diff or edit outside Unity, and every content change needs the editor. Still needs the same mapping into Core.
- **JSON parsed in Core with Newtonsoft Json** (`com.unity.nuget.newtonsoft-json`, [package manual](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.2/manual/index.html)). Handles `decimal`, enum names and nested lists directly, and can reject unknown keys. Not chosen now: it adds a package and a precompiled reference to Core, and the sandbox that compile-checks Core cannot download it. Revisit if content outgrows JsonUtility's limits (see Consequences).
- **A hand-written JSON reader in Core.** Not chosen: reimplements a solved problem without precedent.
- **Keep content in C#.** Not chosen: content is data ([Coding Standards](../Engineering/CodingStandards.md#architecture)), and code edits are a poor way to balance.

## Consequences

- `PlaceholderBeachZoneContent` is removed; the Beach's content moves to the JSON files unchanged.
- Adding an item, feature or zone is a data change plus, until real icon art exists, a placeholder icon entry ([Art Direction](../Design/ArtDirection.md)). A new zone file is also added to the composition root by the setup command.
- `JsonUtility` ignores keys that match no field ([Unity Manual: JSON serialization](https://docs.unity3d.com/6000.4/Documentation/Manual/json-serialization.html)). A misspelled required key leaves the value empty, which the builder rejects; a misspelled optional key, such as `FoundWarning`, goes unnoticed. If that becomes a real problem, or content needs shapes JsonUtility cannot read (polymorphic recipes, for example), a later ADR supersedes this one with Newtonsoft Json.
- Documents updated with the change: [Architecture](../Engineering/Architecture.md) (content, known gaps), [Coding Standards](../Engineering/CodingStandards.md#architecture), [Project Setup](../Engineering/ProjectSetup.md), [Beach Finds](../Design/Content/BeachFinds.md), [Exploration](../Design/Systems/Exploration.md), and [AGENTS.md](../../AGENTS.md).
