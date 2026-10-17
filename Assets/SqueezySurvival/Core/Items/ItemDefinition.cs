
namespace SqueezySurvival.Core.Items
{
    /// <summary>A kind of item, such as a stone. Every stone in the game shares one definition. Immutable.</summary>
    public sealed class ItemDefinition : ICountableThingDefinition
    {
        /// <summary>Creates an item definition.</summary>
        public ItemDefinition(string id, string name, string singularPhrase, string pluralName)
        {
            Id = CountableThingDefinitionText.Require(id, nameof(id));
            Name = CountableThingDefinitionText.Require(name, nameof(name));
            SingularPhrase = CountableThingDefinitionText.Require(singularPhrase, nameof(singularPhrase));
            PluralName = CountableThingDefinitionText.Require(pluralName, nameof(pluralName));
        }

        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string SingularPhrase { get; }

        /// <inheritdoc />
        public string PluralName { get; }
    }
}
