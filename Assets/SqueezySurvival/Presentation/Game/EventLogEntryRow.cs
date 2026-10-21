using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Presentation.Icons;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// One row of the event log, built once and reused by the log's <c>ListView</c> for whichever entry scrolls into it.
    /// Warnings are red and carry a warning icon, so color is never the only signal (Docs/Design/UI/HUD.md, Event log).
    /// </summary>
    public sealed class EventLogEntryRow : VisualElement
    {
        private const string RowClass = "event-log-entry-row";
        private const string WarningRowClass = "event-log-entry-row--warning";

        private readonly Label _text;

        /// <summary>Creates an empty row whose warning icon shows <paramref name="warningIcon"/>.</summary>
        public EventLogEntryRow(Texture2D warningIcon)
        {
            AddToClassList(RowClass);
            var icon = new PixelIconElement(warningIcon, fitsParent: false);
            icon.AddToClassList("event-log-warning-icon");
            icon.style.position = Position.Relative;
            _text = new Label();
            _text.AddToClassList("event-log-entry");
            Add(icon);
            Add(_text);
        }

        /// <summary>The entry the row shows, or null while it is in the pool.</summary>
        public EventLogEntry BoundEntry { get; private set; }

        /// <summary>Shows <paramref name="entry"/>.</summary>
        public void Bind(EventLogEntry entry)
        {
            BoundEntry = entry;
            _text.text = EventLogEntryTextFormatter.Format(entry);
            EnableInClassList(WarningRowClass, entry.Severity == EventLogSeverity.Warning);
        }

        /// <summary>Returns the row to the pool.</summary>
        public void Unbind()
        {
            BoundEntry = null;
        }
    }
}
