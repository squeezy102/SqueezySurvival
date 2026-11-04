using System;
using SqueezySurvival.Core.Content;

namespace SqueezySurvival.Core.Zones
{
    /// <summary>
    /// Which zone a run starts in (Docs/Design/Systems/Zones.md, MVP zones: the Beach is the starting zone). The
    /// choice is a rule held in Core, not made by Presentation (Docs/Decisions/ADR-0009-Data-Driven-Content.md).
    /// </summary>
    public sealed class StartingZoneRules
    {
        /// <summary>The wireframe stage's rule: runs start on the Beach.</summary>
        public static readonly StartingZoneRules Wireframe = new StartingZoneRules("beach");

        /// <summary>Creates a rule that starts runs in the zone with the given id.</summary>
        public StartingZoneRules(string startingZoneId)
        {
            if (string.IsNullOrWhiteSpace(startingZoneId))
            {
                throw new ArgumentException("Cannot be empty.", nameof(startingZoneId));
            }

            StartingZoneId = startingZoneId;
        }

        /// <summary>The id of the zone a run starts in.</summary>
        public string StartingZoneId { get; }

        /// <summary>Finds the starting zone in the content. Throws when the content has no such zone.</summary>
        public ZoneDefinition SelectStartingZone(GameContentCatalog catalog)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException(nameof(catalog));
            }

            if (!catalog.TryGetZone(StartingZoneId, out ZoneDefinition startingZone))
            {
                throw new InvalidOperationException($"The content has no zone '{StartingZoneId}', which runs start in.");
            }

            return startingZone;
        }
    }
}
