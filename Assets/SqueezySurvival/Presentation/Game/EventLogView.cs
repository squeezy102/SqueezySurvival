using System;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.PixelArt;
using SqueezySurvival.Presentation.Icons;
using SqueezySurvival.Presentation.PixelArt;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Shows the run's event log: its entries, the unread warning alert and the jump to the newest entry
    /// (Docs/Design/UI/HUD.md, Event log). Entries are shown by a virtualized <see cref="ListView"/> that reuses a few
    /// rows however long the log grows (Docs/Engineering/Architecture.md, Element lifetime). Which warnings are unread
    /// and whether the log follows new entries are kept by Core.
    /// </summary>
    public sealed class EventLogView : IDisposable
    {
        private const long PulseIntervalMilliseconds = 600;
        private const int NewestItemIndex = -1;
        private const string PulseBrightClass = "event-log-alert-outline--bright";

        private readonly RunEventLog _eventLog;
        private readonly UnreadWarningTracker _unreadWarnings;
        private readonly EventLogFollowState _followState = new EventLogFollowState();
        private readonly ReadOnlyListViewItemsSource<EventLogEntry> _itemsSource;
        private readonly ListView _entries;
        private readonly ScrollView _entriesScrollView;
        private readonly Button _unreadButton;
        private readonly VisualElement _alertOutline;
        private readonly IVisualElementScheduledItem _pulse;
        private int? _olderItemToScrollTo;

        /// <summary>Shows <paramref name="eventLog"/> in the event log panel and keeps it current.</summary>
        public EventLogView(VisualElement panel, RunEventLog eventLog, PixelArtTextureCache textures, UnreadWarningTracker unreadWarnings)
        {
            if (panel == null)
            {
                throw new ArgumentNullException(nameof(panel));
            }

            if (textures == null)
            {
                throw new ArgumentNullException(nameof(textures));
            }

            _eventLog = eventLog ?? throw new ArgumentNullException(nameof(eventLog));
            _unreadWarnings = unreadWarnings ?? throw new ArgumentNullException(nameof(unreadWarnings));
            _itemsSource = new ReadOnlyListViewItemsSource<EventLogEntry>(eventLog.Entries);
            _unreadButton = panel.Q<Button>("event-log-unread-button");
            _alertOutline = panel.Q<VisualElement>("event-log-alert-outline");

            Texture2D warningIcon = textures.StatusIcon(StatusIconCatalog.Warning);
            _entries = panel.Q<ListView>("event-log-entries");
            _entries.makeItem = () => new EventLogEntryRow(warningIcon);
            _entries.bindItem = (element, index) => ((EventLogEntryRow)element).Bind(_eventLog.Entries[index]);
            _entries.unbindItem = (element, index) => ((EventLogEntryRow)element).Unbind();
            _entries.itemsSource = _itemsSource;

            // ListView scrolls with a ScrollView child it builds itself; UQuery is the documented way to reach it.
            _entriesScrollView = _entries.Q<ScrollView>();

            // The jump area belongs in the scroll view's viewport, which UXML cannot reach: there it stays put while
            // the entries scroll, sits clear of the scroll bar, and passes mouse wheel events up to the scroll view.
            VisualElement jumpArea = panel.Q<VisualElement>("event-log-jump-area");
            _entriesScrollView.contentViewport.Add(jumpArea);
            Button jumpButton = jumpArea.Q<Button>("event-log-jump-button");
            jumpButton.Add(new PixelIconElement(textures.StatusIcon(StatusIconCatalog.JumpToNewest)));
            jumpButton.clicked += JumpToNewest;
            _unreadButton.clicked += GoToOldestUnreadWarning;

            // Rows are measured a frame after they are bound, so scrolls are applied again once the content's size settles.
            _entriesScrollView.contentContainer.RegisterCallback<GeometryChangedEvent>(_ => ApplyScroll());
            _entriesScrollView.RegisterCallback<WheelEvent>(_ => AfterPlayerScroll());
            // Only a drag counts: merely hovering the scroll bar does not scroll the log.
            _entriesScrollView.verticalScroller.RegisterCallback<PointerMoveEvent>(
                moved =>
                {
                    if (moved.pressedButtons != 0)
                    {
                        AfterPlayerScroll();
                    }
                },
                TrickleDown.TrickleDown);
            _entriesScrollView.verticalScroller.RegisterCallback<PointerUpEvent>(_ => AfterPlayerScroll(), TrickleDown.TrickleDown);

            _pulse = _alertOutline.schedule.Execute(() => _alertOutline.ToggleInClassList(PulseBrightClass)).Every(PulseIntervalMilliseconds);
            _unreadWarnings.Changed += RefreshUnreadAlert;
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
            _unreadWarnings.Record(entry);
            _entries.RefreshItems();
            ApplyScroll();
        }

        private void GoToOldestUnreadWarning()
        {
            EventLogEntry oldest = _unreadWarnings.OldestUnread;
            if (oldest == null)
            {
                return;
            }

            _followState.WentToOlderEntry();
            _olderItemToScrollTo = _itemsSource.IndexOf(oldest);
            _unreadWarnings.MarkRead(oldest);
            ApplyScroll();
        }

        private void JumpToNewest()
        {
            _followState.JumpedToNewest();
            _olderItemToScrollTo = null;
            ApplyScroll();
            AfterPlayerScroll();
        }

        // Applied when asked and again when the content's size changes, because rows not yet measured have estimated
        // heights; an older entry is scrolled to until the next size change, the newest for as long as the log follows.
        private void ApplyScroll()
        {
            if (_olderItemToScrollTo.HasValue)
            {
                _entries.ScrollToItem(_olderItemToScrollTo.Value);
                if (_entries.GetRootElementForIndex(_olderItemToScrollTo.Value) != null)
                {
                    _olderItemToScrollTo = null;
                }
            }
            else if (_followState.FollowsNewest && _eventLog.Entries.Count > 0)
            {
                _entries.ScrollToItem(NewestItemIndex);
            }
        }

        // The scroll view moves its content during its own handling of the input, so the result is read a frame later.
        private void AfterPlayerScroll()
        {
            _entries.schedule.Execute(() =>
            {
                Scroller scroller = _entriesScrollView.verticalScroller;
                _followState.PlayerScrolled(ScrolledIntoViewCalculator.IsScrolledToEnd(scroller.value, scroller.highValue));
                MarkFullyVisibleWarningsRead();
            });
        }

        // Only rows the ListView has bound exist, so this looks at the few rows near the view, never the whole log.
        private void MarkFullyVisibleWarningsRead()
        {
            if (!_unreadWarnings.HasUnread)
            {
                return;
            }

            Rect viewport = _entriesScrollView.contentViewport.worldBound;
            _entriesScrollView.contentContainer.Query<EventLogEntryRow>().ForEach(row =>
            {
                EventLogEntry entry = row.BoundEntry;
                if (entry != null
                    && entry.Severity == EventLogSeverity.Warning
                    && ScrolledIntoViewCalculator.IsFullyInView(row.worldBound.yMin, row.worldBound.yMax, viewport.yMin, viewport.yMax))
                {
                    _unreadWarnings.MarkRead(entry);
                }
            });
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
