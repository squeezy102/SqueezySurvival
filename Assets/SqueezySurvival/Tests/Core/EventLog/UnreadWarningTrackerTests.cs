using NUnit.Framework;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.EventLog
{
    [TestFixture]
    public sealed class UnreadWarningTrackerTests
    {
        private static EventLogEntry Warning(string text) => new EventLogEntry(GameClock.RunStart, text, EventLogSeverity.Warning);

        private static EventLogEntry Ordinary(string text) => new EventLogEntry(GameClock.RunStart, text);

        [Test]
        public void New_HasNothingUnread()
        {
            var tracker = new UnreadWarningTracker();

            Assert.That(tracker.UnreadCount, Is.EqualTo(0));
            Assert.That(tracker.HasUnread, Is.False);
            Assert.That(tracker.OldestUnread, Is.Null);
        }

        [Test]
        public void Record_Warning_StartsUnreadAndRaisesChanged()
        {
            var tracker = new UnreadWarningTracker();
            int changes = 0;
            tracker.Changed += () => changes++;
            EventLogEntry warning = Warning("Snails are dangerous.");

            tracker.Record(warning);

            Assert.That(tracker.UnreadCount, Is.EqualTo(1));
            Assert.That(tracker.OldestUnread, Is.SameAs(warning));
            Assert.That(changes, Is.EqualTo(1));
        }

        [Test]
        public void Record_OrdinaryEntry_IsIgnored()
        {
            var tracker = new UnreadWarningTracker();
            int changes = 0;
            tracker.Changed += () => changes++;

            tracker.Record(Ordinary("You found a stone."));

            Assert.That(tracker.HasUnread, Is.False);
            Assert.That(changes, Is.EqualTo(0));
        }

        [Test]
        public void Record_SameWarningTwice_CountsOnce()
        {
            var tracker = new UnreadWarningTracker();
            EventLogEntry warning = Warning("Snails are dangerous.");

            tracker.Record(warning);
            tracker.Record(warning);

            Assert.That(tracker.UnreadCount, Is.EqualTo(1));
        }

        [Test]
        public void MarkRead_OldestOfSeveral_LeavesTheNextOldest()
        {
            var tracker = new UnreadWarningTracker();
            EventLogEntry first = Warning("First.");
            EventLogEntry second = Warning("Second.");
            EventLogEntry third = Warning("Third.");
            tracker.Record(first);
            tracker.Record(second);
            tracker.Record(third);

            tracker.MarkRead(tracker.OldestUnread);

            Assert.That(tracker.UnreadCount, Is.EqualTo(2));
            Assert.That(tracker.OldestUnread, Is.SameAs(second));
        }

        [Test]
        public void MarkRead_OutOfOrder_KeepsTheRestOldestFirst()
        {
            var tracker = new UnreadWarningTracker();
            EventLogEntry first = Warning("First.");
            EventLogEntry second = Warning("Second.");
            EventLogEntry third = Warning("Third.");
            tracker.Record(first);
            tracker.Record(second);
            tracker.Record(third);

            tracker.MarkRead(second);

            Assert.That(tracker.Unread, Is.EqualTo(new[] { first, third }));
        }

        [Test]
        public void MarkRead_LastUnread_LeavesNothingUnread()
        {
            var tracker = new UnreadWarningTracker();
            EventLogEntry warning = Warning("Snails are dangerous.");
            tracker.Record(warning);
            int changes = 0;
            tracker.Changed += () => changes++;

            tracker.MarkRead(warning);

            Assert.That(tracker.HasUnread, Is.False);
            Assert.That(tracker.OldestUnread, Is.Null);
            Assert.That(changes, Is.EqualTo(1));
        }

        [Test]
        public void MarkRead_EntryNotUnread_ChangesNothing()
        {
            var tracker = new UnreadWarningTracker();
            tracker.Record(Warning("Snails are dangerous."));
            int changes = 0;
            tracker.Changed += () => changes++;

            tracker.MarkRead(Warning("Never recorded."));

            Assert.That(tracker.UnreadCount, Is.EqualTo(1));
            Assert.That(changes, Is.EqualTo(0));
        }

        [Test]
        public void RecordAndMarkRead_Null_AreRefused()
        {
            var tracker = new UnreadWarningTracker();

            Assert.That(() => tracker.Record(null), Throws.ArgumentNullException);
            Assert.That(() => tracker.MarkRead(null), Throws.ArgumentNullException);
        }
    }
}
