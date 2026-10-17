using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Zones
{
    /// <summary>How long a zone feature lasts (Docs/Design/UI/ScreenLayout.md, Regions: zone features).</summary>
    public enum ZoneFeaturePermanence
    {
        /// <summary>Stays for the whole run, such as a large tree.</summary>
        Permanent,

        /// <summary>Lasts a while, then can change or disappear, such as a puddle.</summary>
        SemiPermanent,
    }

    /// <summary>
    /// A kind of zone feature: a resource node or other fixture of a zone, such as a palm tree or a tide pool
    /// (Docs/Design/Systems/Exploration.md, What exploring can reveal). Immutable.
    /// </summary>
    public sealed class ZoneFeatureDefinition : ICountableThingDefinition
    {
        /// <summary>Creates a zone feature definition.</summary>
        public ZoneFeatureDefinition(string id, string name, string singularPhrase, string pluralName, ZoneFeaturePermanence permanence)
        {
            Id = CountableThingDefinitionText.Require(id, nameof(id));
            Name = CountableThingDefinitionText.Require(name, nameof(name));
            SingularPhrase = CountableThingDefinitionText.Require(singularPhrase, nameof(singularPhrase));
            PluralName = CountableThingDefinitionText.Require(pluralName, nameof(pluralName));
            Permanence = permanence;
        }

        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string SingularPhrase { get; }

        /// <inheritdoc />
        public string PluralName { get; }

        /// <summary>How long the feature lasts.</summary>
        public ZoneFeaturePermanence Permanence { get; }
    }
}
