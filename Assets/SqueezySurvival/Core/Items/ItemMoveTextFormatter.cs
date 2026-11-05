using System;
using System.Linq;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Items
{
    /// <summary>
    /// The words for a move between panels: its preview while dragging, and its event log entry
    /// (Docs/Design/UI/DragAndDrop.md, Between panels; Docs/Design/Systems/Inventory.md, Moving things).
    /// </summary>
    public static class ItemMoveTextFormatter
    {
        /// <summary>"Pick up", "Put down", "Put in", "Stack" or "No room".</summary>
        public static string FormatTitle(ItemMovePreview preview)
        {
            Require(preview);
            if (!preview.CanMove)
            {
                return "No room";
            }

            if (preview.JoinsStack)
            {
                return "Stack";
            }

            switch (preview.To.Kind)
            {
                case ItemPanelKind.Inventory:
                    return "Pick up";
                case ItemPanelKind.Ground:
                    return "Put down";
                case ItemPanelKind.Container:
                    return "Put in";
                default:
                    throw UnknownPanelKind(preview.To);
            }
        }

        /// <summary>
        /// What letting go would do and how long it takes: "Put 5 stones in your inventory. Takes 10 sec.", "Add a stone
        /// to the 12 on the ground. Takes 2 sec.", or "There's no free slot in your inventory."
        /// </summary>
        public static string FormatDescription(ItemMovePreview preview)
        {
            Require(preview);
            if (!preview.CanMove)
            {
                return $"There's no free slot {PlaceIn(preview)}.";
            }

            string moving = Phrase(preview.Definition, preview.Count);
            string takes = $"Takes {GameClockFormatter.FormatDurationSeconds(preview.Seconds)}.";
            return preview.JoinsStack
                ? $"Add {moving} to the {preview.AlreadyThereCount} {PlaceIn(preview)}. {takes}"
                : $"Put {moving} {PlaceIn(preview)}. {takes}";
        }

        /// <summary>The event log entry: "You picked up 5 stones.", "You put a stick on the ground." or "You put a stick in the basket."</summary>
        public static string FormatLog(ItemMovePreview preview)
        {
            Require(preview);
            string moved = Phrase(preview.Definition, preview.Count);
            return preview.To.Kind == ItemPanelKind.Inventory ? $"You picked up {moved}." : $"You put {moved} {PlaceIn(preview)}.";
        }

        private static string PlaceIn(ItemMovePreview preview)
        {
            switch (preview.To.Kind)
            {
                case ItemPanelKind.Inventory:
                    return "in your inventory";
                case ItemPanelKind.Ground:
                    return "on the ground";
                case ItemPanelKind.Container:
                    return $"in the {preview.DestinationName}";
                default:
                    throw UnknownPanelKind(preview.To);
            }
        }

        private static ArgumentOutOfRangeException UnknownPanelKind(ItemPanel panel) =>
            new ArgumentOutOfRangeException(nameof(panel), panel.Kind, "Unknown kind of panel.");

        private static string Phrase(ItemDefinition definition, int count) =>
            CountedThingTextFormatter.FormatSentencePhrase(Enumerable.Repeat<ICountableThingDefinition>(definition, count));

        private static void Require(ItemMovePreview preview)
        {
            if (preview == null)
            {
                throw new ArgumentNullException(nameof(preview));
            }
        }
    }
}
