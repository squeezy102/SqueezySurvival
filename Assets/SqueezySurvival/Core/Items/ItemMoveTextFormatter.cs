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
        /// <summary>"Pick up", "Put down", "Stack" or "No room".</summary>
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

            return preview.To == ItemPanel.Inventory ? "Pick up" : "Put down";
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
                return $"There's no free slot {PlaceIn(preview.To)}.";
            }

            string moving = Phrase(preview.Definition, preview.Count);
            string takes = $"Takes {GameClockFormatter.FormatDurationSeconds(preview.Seconds)}.";
            return preview.JoinsStack
                ? $"Add {moving} to the {preview.AlreadyThereCount} {PlaceIn(preview.To)}. {takes}"
                : $"Put {moving} {PlaceIn(preview.To)}. {takes}";
        }

        /// <summary>The event log entry: "You picked up 5 stones." or "You put a stick on the ground."</summary>
        public static string FormatLog(ItemMovePreview preview)
        {
            Require(preview);
            string moved = Phrase(preview.Definition, preview.Count);
            return preview.To == ItemPanel.Inventory ? $"You picked up {moved}." : $"You put {moved} on the ground.";
        }

        private static string PlaceIn(ItemPanel panel) => panel == ItemPanel.Inventory ? "in your inventory" : "on the ground";

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
