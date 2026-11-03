# ADR-0009: Data-driven content

**Status:** Accepted
**Date:** 2026-11-03

## Context

Items, zone features and zones, with their find pools and scripted first explores, are written in C# in `PlaceholderBeachZoneContent`. The [Coding Standards](../Engineering/CodingStandards.md#architecture) require content to be data, not code, and the [architecture review](../Engineering/Architecture.md#known-gaps) lists this as a gap ([#124](https://github.com/squeezy102/SqueezySurvival/issues/124)).

The forces:

- **Core has no Unity references** ([ADR-0007](ADR-0007-Runtime-UI-And-Assembly-Layout.md)). It cannot load assets, so whatever reads the content in Presentation must hand Core something plain.
- **Contributors work through AI assistants and pull requests** ([ADR-0006](ADR-0006-Public-Source-And-Contribution-Workflow.md)). Content must be readable and editable as text, and its changes reviewable in a diff.
- **Core is compile-checked outside Unity** ([Testing and Quality Standards: checking Core without Unity](../Engineering/TestingStandards.md#checking-core-without-unity)). That sandbox reaches the operating system's package mirror but not NuGet or Unity's package registry, so Core cannot depend on a library it can only get from either.
- **No package without an ADR** ([ADR-0003](ADR-0003-Package-Baseline.md)).
- Content will grow: more zones, recipes, resource nodes. Content mistakes (a misspelled key or id, a weight of 0) must be caught by tests, not found in play.

## Decision

### Content is JSON, imported as text assets

Content lives in `Assets/SqueezySurvival/Content/` as JSON files. Unity imports `.json` files as text assets ([Unity Manual: text assets](https://docs.unity3d.com/6000.4/Documentation/Manual/class-TextAsset.html)). One file per kind of content, plus one per zone:

| File | Holds |
|---|---|
| `Items.json` | Every item definition |
| `ZoneFeatures.json` | Every zone feature definition |
| `Zones/<Zone>.json` | One zone: its name, explores to map, find pool, feature pool and scripted explores, naming items and features by id |

Keys match the record properties below, in PascalCase, following the project's C# naming ([Coding Standards: naming](../Engineering/CodingStandards.md#naming)). An example of one item and part of a zone:

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
      "WeightPounds": 1.0
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
  "ScriptedExplores": [ [ "stone", "stone", "small-stick" ] ]
}
```

### Presentation parses with Newtonsoft Json; Core builds and checks

Reading content takes two steps, each in the layer that can do it:

1. **Parse (Presentation).** A Presentation reader turns each text asset into a record with Newtonsoft Json, Unity's package of Json.NET (`com.unity.nuget.newtonsoft-json`, [package manual](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.2/manual/index.html)). The records are plain Core classes with public properties, typed as the definitions need them: `decimal` weights, `ItemCategory` and `ZoneFeaturePermanence` enums, lists of lists for scripted explores ([Json.NET: serialization guide](https://www.newtonsoft.com/json/help/html/SerializationGuide.htm)). Core never references Newtonsoft; only the reader does.
2. **Build (Core).** A Core builder takes the records and returns a content catalog of the existing immutable definitions (`ItemDefinition`, `ZoneFeatureDefinition`, `ZoneDefinition`). It checks every record, resolves every id a zone names, and reports **every** problem in one exception, each naming the file and id ("ZoneFeatures.json, feature 'puddle': description is empty"), so one fix pass covers them all.

The reader's settings make the files strict:

- **Unknown keys are errors** (`MissingMemberHandling.Error`, [Json.NET](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_MissingMemberHandling.htm)), so a misspelled key fails instead of being ignored.
- **Enums are written as names, never numbers** (`StringEnumConverter` with `AllowIntegerValues` off, [Json.NET](https://www.newtonsoft.com/json/help/html/T_Newtonsoft_Json_Converters_StringEnumConverter.htm)), so a diff reads "StoneAndMinerals", not "2", and an unknown name fails.

A parse error names the file; Json.NET's message gives the path and line. Records are a file format, not a model: nothing outside the builder and the reader uses them.

The records are created by reflection, which the build's code stripping cannot always see, so a `link.xml` preserves the record namespace ([Unity Manual: preserving code](https://docs.unity3d.com/6000.4/Documentation/Manual/managed-code-stripping-preserving.html), [link.xml format](https://docs.unity3d.com/6000.4/Documentation/Manual/managed-code-stripping-xml-formatting.html)).

### How the content reaches the game

The game scene's composition root holds the text assets as serialized references, set by **SqueezySurvival > Setup > Game Screen** like the rest of the generated scene ([Project Setup: setup commands](../Engineering/ProjectSetup.md#setup-commands); [Unity Scripting API: SerializedObject](https://docs.unity3d.com/6000.4/Documentation/ScriptReference/SerializedObject.html)). It reads and builds the catalog once, then passes definitions into the run as it does today. Which zone a run starts in is a rule ([Zones](../Design/Systems/Zones.md): the Beach is the starting zone), held in Core, not chosen by Presentation.

The `Resources` folder is not used: Unity advises against it beyond prototyping because it slows startup and grows the build ([Unity Manual: Resources folder best practices](https://docs.unity.cn/2023.3/Documentation/Manual/BestPracticeUnderstandingPerformanceInUnity6.html)).

### Content values have one home

The content files are the home of each thing's values: its names, description, weight, category, its weights in a pool, a zone's explores to map. Design documents keep intent, targets and ranges (what an item is for, how long zones take to map) and link to the files instead of restating values ([AGENTS.md: one home per fact](../../AGENTS.md#non-negotiable-rules)).

### Tests

- **Core:** the builder is unit tested: each kind of problem (empty text, weight at and below its lower bound, duplicate id, unknown id in a pool or scripted explore, empty scripted explore) and a valid build.
- **Presentation:** the reader is tested with small JSON strings (a valid file, an unknown key, an enum by number or unknown name, malformed JSON), and one test loads the shipped content files and builds the catalog, so a content mistake fails a test before it reaches a build. The check that every item and feature has a placeholder icon moves there.
- **No other test depends on shipped content.** Tests build their own definitions, so balancing a weight never breaks an unrelated test.

## Alternatives considered

- **Unity's built-in JsonUtility.** Needs no package. Not chosen: it cannot read `decimal`, enums by name, nested lists or dictionaries ([Unity Manual: JSON serialization](https://docs.unity3d.com/6000.4/Documentation/Manual/json-serialization.html), [serialization rules](https://docs.unity3d.com/6000.4/Documentation/Manual/script-serialization-rules.html)), so records would carry stand-in types for Core to convert, and it silently ignores keys that match no field, so a misspelled optional key would go unnoticed.
- **Newtonsoft Json referenced by Core.** Not chosen: Core would carry a precompiled library the sandbox that compile-checks Core cannot download. Parsing in Presentation gives the same strictness and keeps Core dependency-free.
- **ScriptableObjects mapped to Core definitions in Presentation.** Unity's own data-container asset, edited in the Inspector ([Unity Manual: ScriptableObject](https://docs.unity3d.com/6000.4/Documentation/Manual/class-ScriptableObject.html)). Not chosen: assets reference each other by GUID in YAML, which is hard to review in a diff or edit outside Unity, and every content change needs the editor. Still needs the same mapping into Core.
- **A hand-written JSON reader.** Not chosen: reimplements a solved problem without precedent.
- **Keep content in C#.** Not chosen: content is data ([Coding Standards](../Engineering/CodingStandards.md#architecture)), and code edits are a poor way to balance.

## Consequences

- `Packages/manifest.json` gains `com.unity.nuget.newtonsoft-json` 3.2.1, the latest version in its [changelog](https://docs.unity3d.com/Packages/com.unity.nuget.newtonsoft-json@3.2/changelog/CHANGELOG.html), added by name: it is not listed among the editor's released packages ([Unity Manual: released packages](https://docs.unity3d.com/6000.4/Documentation/Manual/pack-safe.html)). [ADR-0003](ADR-0003-Package-Baseline.md) lists it. Presentation and its tests reference its assembly; Core does not.
- `PlaceholderBeachZoneContent` is removed; the Beach's content moves to the JSON files unchanged.
- Adding an item, feature or zone is a data change plus, until real icon art exists, a placeholder icon entry ([Art Direction](../Design/ArtDirection.md)). A new zone file is also added to the composition root by the setup command.
- Record types are added only under the preserved namespace, or a build strips them.
- Documents updated with the change: [Architecture](../Engineering/Architecture.md) (content, known gaps), [Coding Standards](../Engineering/CodingStandards.md#architecture), [Project Setup](../Engineering/ProjectSetup.md), [Beach Finds](../Design/Content/BeachFinds.md), [Exploration](../Design/Systems/Exploration.md), and [AGENTS.md](../../AGENTS.md).
