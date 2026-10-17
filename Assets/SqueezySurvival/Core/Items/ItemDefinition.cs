using System;


namespace SqueezySurvival.Core.Items
{
    /// <summary>A kind of item, such as a stone. Every stone in the game shares one definition. Immutable.</summary>
    public sealed class ItemDefinition : ICountableThingDefinition
    {
        /// <summary>Creates an item definition.</summary>
        /// <param name="id">Stable identifier, never shown.</param>
        /// <param name="name">Name shown in lists and tooltips.</param>
        /// <param name="singularPhrase">One of it in a sentence: "a stone".</param>
        /// <param name="pluralName">Several after a number: "stones".</param>
        /// <param name="description">A sentence or two for its tooltip.</param>
        /// <param name="weightPounds">How much one weighs, in pounds (Docs/Design/Systems/Inventory.md). Must be more than 0.</param>
        public ItemDefinition(string id, string name, string singularPhrase, string pluralName, string description, decimal weightPounds)
        {
            Id = CountableThingDefinitionText.Require(id, nameof(id));
            Name = CountableThingDefinitionText.Require(name, nameof(name));
            SingularPhrase = CountableThingDefinitionText.Require(singularPhrase, nameof(singularPhrase));
            PluralName = CountableThingDefinitionText.Require(pluralName, nameof(pluralName));
            Description = CountableThingDefinitionText.Require(description, nameof(description));
            if (weightPounds <= 0m)
            {
                throw new ArgumentOutOfRangeException(nameof(weightPounds), weightPounds, "Must be more than 0.");
            }

            WeightPounds = weightPounds;
        }

        /// <inheritdoc />
        public string Id { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public string SingularPhrase { get; }

        /// <inheritdoc />
        public string PluralName { get; }

        /// <inheritdoc />
        public string Description { get; }

        /// <summary>How much one weighs, in pounds.</summary>
        public decimal WeightPounds { get; }
    }
}
