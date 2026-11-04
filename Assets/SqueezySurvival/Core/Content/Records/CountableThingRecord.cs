namespace SqueezySurvival.Core.Content.Records
{
    /// <summary>
    /// What an item and a zone feature share in their files: the text of a countable thing. A file format, not a
    /// model: only the content reader and <see cref="GameContentCatalogBuilder"/> use records. Keys match the property
    /// names. Keep records in this namespace, which link.xml preserves from code stripping
    /// (Docs/Decisions/ADR-0009-Data-Driven-Content.md).
    /// </summary>
    public abstract class CountableThingRecord
    {
        /// <summary>Stable identifier, never shown.</summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>Name shown in lists and tooltips.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>One of it in a sentence: "a stone".</summary>
        public string SingularPhrase { get; set; } = string.Empty;

        /// <summary>Several after a number: "stones".</summary>
        public string PluralName { get; set; } = string.Empty;

        /// <summary>A sentence or two for its tooltip.</summary>
        public string Description { get; set; } = string.Empty;
    }
}
