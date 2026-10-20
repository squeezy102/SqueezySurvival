using System;
using System.Collections.Generic;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.PixelArt;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Presentation.Icons;
using SqueezySurvival.Presentation.PixelArt;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Shows the run's event log: its entries, the unread warning alert and the jump to the newest entry
    /// (Docs/Design/UI/HUD.md, Event log). Which warnings are unread is kept by Core's <see cref="UnreadWarningTracker"/>.
    /// </summary>
    public sealed class EventLogView : IDisposable
    {
        private const long PulseIntervalMilliseconds = 600;
        private const float FullyVisibleTolerance = 0.5f;
        private const string PulseBrightClass = "event-log-alert-outline--bright";

        private readonly RunEventLog _eventLog;
        private readonly PixelArtTextureCache _textures;
        private readonly UnreadWarningTracker _unreadWarnings;
        private readonly ScrollView _entries;
        private readonly Button _unreadButton;
        private readonly VisualElement _alertOutline;
        private readonly IVisualElementScheduledItem _pulse;
        private readonly Dictionary<EventLogEntry, VisualElement> _entryElements = new Dictionary<EventLogEntry, VisualElement>();
        private bool _followsNewest = true;
        private VisualElement _scrollToEntryPending;

        /// <summary>Shows <paramref name="eventLog"/> in the event log panel and keeps it current.</summary>
        public EventLogView(VisualElement panel, RunEventLog eventLog, PixelArtTextureCache textures, UnreadWarningTracker unreadWarnings)
        {
            if (panel == null)
            {
                throw new ArgumentNullException(nameof(panel));
            }

            _eventLog = eventLog ?? throw new ArgumentNullException(nameof(eventLog));
            _textures = textures ?? throw new ArgumentNullException(nameof(textures));
            _unreadWarnings = unreadWarnings ?? throw new ArgumentNullException(nameof(unreadWarnings));
            _entries = panel.Q<ScrollView>("event-log-entries");
            _unreadButton = panel.Q<Button>("event-log-unread-button");
            _alertOutline = panel.Q<VisualElement>("event-log-alert-outline");

            // The jump area belongs in the scroll view's viewport, which UXML cannot reach: there it stays put while
            // the entries scroll, sits clear of the scroll bar, and passes mouse wheel events up to the scroll view.
            VisualElement jumpArea = panel.Q<VisualElement>("event-log-jump-area");
            _entries.contentViewport.Add(jumpArea);
            Button jumpButton = jumpArea.Q<Button>("event-log-jump-button");
            jumpButton.Add(new PixelIconElement(_textures.StatusIcon(StatusIconCatalog.JumpToNewest)));
            jumpButton.clicked += JumpToNewest;
            _unreadButton.clicked += GoToOldestUnreadWarning;

            // New entries are laid out a frame after they are added, so scrolls wait for the content's size to change.
            _entries.contentContainer.RegisterCallback<GeometryChangedEvent>(_ => ApplyPendingScroll());
            _entries.RegisterCallback<WheelEvent>(_ => AfterPlayerScroll());
            // Only a drag counts: merely hovering the scroll bar does not scroll the log.
            _entries.verticalScroller.RegisterCallback<PointerMoveEvent>(
                moved =>
                {
                    if (moved.pressedButtons != 0)
                    {
                        AfterPlayerScroll();
                    }
                },
                TrickleDown.TrickleDown);
            _entries.verticalScroller.RegisterCallback<PointerUpEvent>(_ => AfterPlayerScroll(), TrickleDown.TrickleDown);

            _pulse = _alertOutline.schedule.Execute(() => _alertOutline.ToggleInClassList(PulseBrightClass)).Every(PulseIntervalMilliseconds);
            _unreadWarnings.Changed += RefreshUnreadAlert;

            foreach (EventLogEntry entry in eventLog.Entries)
            {
                ShowEntry(entry);
            }

            _eventLog.EntryAdded += OnEntryAdded;
            RefreshUnreadAlert();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _eventLog.EntryAdded -= OnEntryAdded;
            _unreadWarnings.Changed -= RefreshUnreadAlert;
            _pulse.Pause();
        }

        private void OnEntryAdded(EventLogEntry entry)
        {
            ShowEntry(entry);
            _unreadWarnings.Record(entry);
        }

        private void ShowEntry(EventLogEntry entry)
        {
            string time = GameClockFormatter.FormatTimeOfDay(entry.Time.MinuteOfDay);
            var line = new Label($"Day {entry.Time.Day}, {time}: {entry.Text}");
            line.AddToClassList("event-log-entry");
            VisualElement shown = line;
            if (entry.Severity == EventLogSeverity.Warning)
            {
                // Warnings are red and carry a warning icon, so color is never the only signal.
                shown = new VisualElement();
                shown.AddToClassList("event-log-warning-row");
                var icon = new PixelIconElement(_textures.StatusIcon(StatusIconCatalog.Warning), fitsParent: false);
                icon.AddToClassList("event-log-warning-icon");
                icon.style.position = Position.Relative;
                line.AddToClassList("event-log-entry--warning");
                shown.Add(icon);
                shown.Add(line);
            }

            _entries.Add(shown);
            _entryElements[entry] = shown;
        }

        private void GoToOldestUnreadWarning()
        {
            EventLogEntry oldest = _unreadWarnings.OldestUnread;
            if (oldest == null)
            {
                return;
            }

            _followsNewest = false;
            _scrollToEntryPending = _entryElements[oldest];
            _unreadWarnings.MarkRead(oldest);
            ApplyPendingScroll();
        }

        private void JumpToNewest()
        {
            _followsNewest = true;
            _scrollToEntryPending = null;
            ApplyPendingScroll();
            AfterPlayerScroll();
        }

        private void ApplyPendingScroll()
        {
            if (_scrollToEntryPending != null)
            {
                _entries.ScrollTo(_scrollToEntryPending);
                _scrollToEntryPending = null;
            }
            else if (_followsNewest)
            {
                _entries.verticalScroller.value = _entries.verticalScroller.highValue;
            }
        }

        // The scroll view moves its content during its own handling of the input, so the result is read a frame later.
        private void AfterPlayerScroll()
        {
            _entries.schedule.Execute(() =>
            {
                Scroller scroller = _entries.verticalScroller;
                _followsNewest = scroller.value >= scroller.highValue - FullyVisibleTolerance;
                MarkFullyVisibleWarningsRead();
            });
        }

        private void MarkFullyVisibleWarningsRead()
        {
            UnityEngine.Rect viewport = _entries.contentViewport.worldBound;
            var seen = new List<EventLogEntry>();
            foreach (EventLogEntry warning in _unreadWarnings.Unread)
            {
                UnityEngine.Rect bounds = _entryElements[warning].worldBound;
                if (bounds.yMin >= viewport.yMin - FullyVisibleTolerance && bounds.yMax <= viewport.yMax + FullyVisibleTolerance)
                {
                    seen.Add(warning);
                }
            }

            foreach (EventLogEntry warning in seen)
            {
                _unreadWarnings.MarkRead(warning);
            }
        }

        private void RefreshUnreadAlert()
        {
            bool hasUnread = _unreadWarnings.HasUnread;
            _unreadButton.text = UnreadWarningIndicatorTextFormatter.Format(_unreadWarnings.UnreadCount);
            _unreadButton.style.display = hasUnread ? DisplayStyle.Flex : DisplayStyle.None;
            _alertOutline.style.display = hasUnread ? DisplayStyle.Flex : DisplayStyle.None;
            if (hasUnread)
            {
                _pulse.Resume();
            }
            else
            {
                _pulse.Pause();
                _alertOutline.RemoveFromClassList(PulseBrightClass);
            }
        }
    }
}
