using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Randomness;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Exploration
{
    /// <summary>
    /// Resolves one explore of a zone: advances its progress, draws loose items into its ground pool and sometimes
    /// reveals a feature (Docs/Design/Systems/Exploration.md). Time is the caller's concern.
    /// </summary>
    public sealed class ZoneExplorer
    {
        private readonly ExplorationRules _rules;
        private readonly IRandomNumberSource _randomNumberSource;

        /// <summary>Creates an explorer that follows the given rules and draws from the given random numbers.</summary>
        public ZoneExplorer(ExplorationRules rules, IRandomNumberSource randomNumberSource)
        {
            _rules = rules ?? throw new ArgumentNullException(nameof(rules));
            _randomNumberSource = randomNumberSource ?? throw new ArgumentNullException(nameof(randomNumberSource));
        }

        /// <summary>The rules this explorer follows.</summary>
        public ExplorationRules Rules => _rules;

        /// <summary>
        /// Explores the zone once. The zone's first explores give its scripted finds and never a feature; after them,
        /// finds are drawn from its find pool, and a zone not yet fully explored may also reveal a feature. Finds
        /// never run out, even in a fully explored zone.
        /// </summary>
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
                if (!progressBefore.IsFullyExplored && RollFeature())
                {
                    featuresFound.Add(definition.FeaturePool.Pick(_randomNumberSource));
                }
            }

            zone.RecordExplore();
            zone.GroundPool.AddRange(itemsFound);
            zone.Features.AddRange(featuresFound);
            return new ExplorationOutcome(itemsFound, featuresFound, progressBefore, zone.ExplorationProgress);
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
