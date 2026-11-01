using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Exploration
{
    /// <summary>
    /// Resolves one explore of a zone: advances its progress, adds what was found and sometimes reveals a feature
    /// (Docs/Design/Systems/Exploration.md). Time is the caller's concern.
    /// </summary>
    public interface IZoneExplorer
    {
        /// <summary>The rules this explorer follows.</summary>
        ExplorationRules Rules { get; }

        /// <summary>
        /// Explores the zone once. The zone's first explores give its scripted finds and never a feature; after them,
        /// finds are drawn from its find pool and may come with a new feature. Neither ever runs out, even in a fully
        /// explored zone.
        /// </summary>
        ExplorationOutcome Explore(Zone zone);
    }
}
