using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Randomness;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Exploration
{
    /// <summary>Explores a zone by its definition's scripted finds, then by drawing from its pools at random.</summary>
    public sealed class ZoneExplorer : IZoneExplorer
    {
        private readonly ExplorationRules _rules;
        private readonly IRandomNumberSource _randomNumberSource;

        /// <summary>Creates an explorer that follows the given rules and draws from the given random numbers.</summary>
        public ZoneExplorer(ExplorationRules rules, IRandomNumberSource randomNumberSource)
        {
            _rules = rules ?? throw new ArgumentNullException(nameof(rules));
            _randomNumberSource = randomNumberSource ?? throw new ArgumentNullException(nameof(randomNumberSource));
        }

        /// <inheritdoc />
        public ExplorationRules Rules => _rules;

        /// <inheritdoc />
        public ExplorationOutcome Explore(Zone zone)
        {
            if (zone == null)
            {
                throw new ArgumentNullException(nameof(zone));
            }

            ExplorationProgress progressBefore = zone.ExplorationProgress;
            ZoneDefinition definition = zone.Definition;
            int exploreIndex = progressBefore.ExploresCompleted;

            IReadOnlyList<ItemDefinition> itemsFound;
            var featuresFound = new List<ZoneFeatureDefinition>();
            if (exploreIndex < definition.ScriptedFinds.Count)
            {
                itemsFound = definition.ScriptedFinds[exploreIndex];
            }
            else
            {
                itemsFound = DrawItems(definition);
                if (RollFeature())
                {
                    // A feature with no room in its grid is not revealed (Docs/Design/UI/InventoryGrids.md).
                    ZoneFeatureDefinition feature = definition.FeaturePool.Pick(_randomNumberSource);
                    if (zone.Features.HasRoomFor(feature))
                    {
                        featuresFound.Add(feature);
                    }
                }
            }

            zone.RecordExplore();
            IReadOnlyList<CountedThing<ItemDefinition>> groundOverflow = zone.GroundPool.AddRange(itemsFound);
            zone.Features.AddRange(featuresFound);
            return new ExplorationOutcome(itemsFound, featuresFound, groundOverflow, progressBefore, zone.ExplorationProgress);
        }

        private List<ItemDefinition> DrawItems(ZoneDefinition definition)
        {
            int count = _randomNumberSource.NextInt(_rules.MinimumFindsPerExplore, _rules.MaximumFindsPerExplore + 1);
            var items = new List<ItemDefinition>(count);
            for (int index = 0; index < count; index++)
            {
                items.Add(definition.FindPool.Pick(_randomNumberSource));
            }

            return items;
        }

        private bool RollFeature() => _randomNumberSource.NextInt(0, 100) < _rules.FeatureChancePercent;
    }
}
