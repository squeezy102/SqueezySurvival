namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// Something that can be found and counted, such as an item or a zone feature, with the names needed to show it
    /// in lists and write about it in the event log.
    /// </summary>
    public interface ICountableThingDefinition
    {
        /// <summary>Stable identifier, used by saves and data. Never shown to the player.</summary>
        string Id { get; }

        /// <summary>Name shown in lists: "Stone".</summary>
        string Name { get; }

        /// <summary>One of it, as written in a sentence: "a stone".</summary>
        string SingularPhrase { get; }

        /// <summary>More than one, as written after a number: "stones" in "2 stones".</summary>
        string PluralName { get; }

        /// <summary>A sentence or two describing it, shown in its tooltip.</summary>
        string Description { get; }
    }
}
