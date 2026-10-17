using NUnit.Framework;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.EventLog
{
    [TestFixture]
    public sealed class RunEventLogTests
    {
        [Test]
        public void Add_AppendsEntryAndRaisesEvent()
        {
            var log = new RunEventLog();
            EventLogEntry raised = null;
            log.EntryAdded += entry => raised = entry;

            log.Add(GameClock.RunStart, "You picked up 5 sticks.");

            Assert.That(log.Entries, Has.Count.EqualTo(1));
            Assert.That(raised, Is.SameAs(log.Entries[0]));
            Assert.That(raised.Text, Is.EqualTo("You picked up 5 sticks."));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void Add_WithoutText_Throws(string text)
        {
            Assert.That(() => new RunEventLog().Add(GameClock.RunStart, text), Throws.ArgumentException);
        }
    }
}
