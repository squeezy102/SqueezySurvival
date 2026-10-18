using System;
using System.Globalization;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Runs
{
    /// <summary>
    /// The words for a find with no room on the ground: the question, its choices and the log lines
    /// (Docs/Design/UI/InventoryGrids.md, When the ground is full). Every choice says that what goes is gone for good.
    /// </summary>
    public static class GroundOverflowTextFormatter
    {
        /// <summary>"There's no room on the ground for the conch."</summary>
        public static string FormatQuestion(CountedThing<ItemDefinition> incoming) =>
            $"There's no room on the ground for {DefinitePhrase(Require(incoming).Definition, incoming.Count)}.";

        /// <summary>"Leave the conch behind"</summary>
        public static string FormatLeaveBehindChoice(CountedThing<ItemDefinition> incoming) =>
            $"Leave {DefinitePhrase(Require(incoming).Definition, incoming.Count)} behind";

        /// <summary>"It will be gone for good." or "They will be gone for good."</summary>
        public static string FormatLeaveBehindWarning(CountedThing<ItemDefinition> incoming) =>
            $"{Pronoun(Require(incoming).Count)} will be gone for good.";

        /// <summary>"Pick something on the ground to throw away; it will be gone for good, and the conch takes its place."</summary>
        public static string FormatDiscardChoiceWarning(CountedThing<ItemDefinition> incoming)
        {
            Require(incoming);
            string takes = incoming.Count == 1 ? "takes" : "take";
            return $"Pick something on the ground to throw away; it will be gone for good, and {DefinitePhrase(incoming.Definition, incoming.Count)} {takes} its place.";
        }

        /// <summary>"Click something on the ground to discard for the conch. Esc: go back."</summary>
        public static string FormatDiscardHint(CountedThing<ItemDefinition> incoming) =>
            $"Click something on the ground to discard for {DefinitePhrase(Require(incoming).Definition, incoming.Count)}. Esc: go back.";

        /// <summary>"Discard the stone (3) to make room for the conch. It will be gone for good."</summary>
        public static string FormatDiscardPreview(CountedEntry<ItemDefinition> discarded, CountedThing<ItemDefinition> incoming)
        {
            if (discarded == null)
            {
                throw new ArgumentNullException(nameof(discarded));
            }

            Require(incoming);
            string count = discarded.Count == 1 ? string.Empty : $" ({discarded.Count.ToString(CultureInfo.InvariantCulture)})";
            string pronoun = discarded.Count == 1 ? "It" : "They";
            return $"Discard the {discarded.Definition.Name.ToLowerInvariant()}{count} to make room for {DefinitePhrase(incoming.Definition, incoming.Count)}. {pronoun} will be gone for good.";
        }

        /// <summary>"You left the conch behind. It's gone for good."</summary>
        public static string FormatLeftBehindLog(CountedThing<ItemDefinition> incoming) =>
            $"You left {DefinitePhrase(Require(incoming).Definition, incoming.Count)} behind. {PronounIs(incoming.Count)} gone for good.";

        /// <summary>"You threw away the 3 stones to make room for the conch. They're gone for good."</summary>
        public static string FormatDiscardedLog(CountedEntry<ItemDefinition> discarded, CountedThing<ItemDefinition> incoming)
        {
            if (discarded == null)
            {
                throw new ArgumentNullException(nameof(discarded));
            }

            Require(incoming);
            return $"You threw away {DefinitePhrase(discarded.Definition, discarded.Count)} to make room for {DefinitePhrase(incoming.Definition, incoming.Count)}. {PronounIs(discarded.Count)} gone for good.";
        }

        private static string DefinitePhrase(ItemDefinition definition, int count) =>
            count == 1 ? $"the {definition.Name.ToLowerInvariant()}" : $"the {count.ToString(CultureInfo.InvariantCulture)} {definition.PluralName}";

        private static string Pronoun(int count) => count == 1 ? "It" : "They";

        private static string PronounIs(int count) => count == 1 ? "It's" : "They're";

        private static CountedThing<ItemDefinition> Require(CountedThing<ItemDefinition> incoming) =>
            incoming ?? throw new ArgumentNullException(nameof(incoming));
    }
}
