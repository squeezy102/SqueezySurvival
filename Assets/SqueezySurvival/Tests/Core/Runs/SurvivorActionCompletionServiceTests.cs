using System;
using System.Collections.Generic;
using NUnit.Framework;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Tests.Time;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class SurvivorActionCompletionServiceTests
    {
        private static SurvivorActionCompletionService Create(
            out RunEventLog eventLog,
            out GameTimeAdvancer gameTimeAdvancer,
            out GroundInventoryService groundInventory,
            params ITimeDependentSystem[] systems)
        {
            eventLog = new RunEventLog();
            gameTimeAdvancer = new GameTimeAdvancer(GameClock.RunStart, systems);
            groundInventory = new GroundInventoryService(new Zone(TestZoneDefinitions.Create(4)), gameTimeAdvancer, eventLog);
            return new SurvivorActionCompletionService(gameTimeAdvancer, eventLog, groundInventory);
        }

        [Test]
        public void Complete_LogsStampedWithTheEndTimeBeforeTimeDependentSystemsAreTold()
        {
            RunEventLog eventLog = null;
            int entriesWhenTold = -1;
            var system = new RecordingTimeDependentSystem(_ => entriesWhenTold = eventLog.Entries.Count);
            SurvivorActionCompletionService completion = Create(out eventLog, out GameTimeAdvancer advancer, out _, system);

            completion.Complete(
                ActionDuration.FromSeconds(4),
                new[] { new EventLogMessage("First."), new EventLogMessage("Second.", EventLogSeverity.Warning) });

            Assert.That(entriesWhenTold, Is.EqualTo(2));
            Assert.That(eventLog.Entries[0].Time.ElapsedSeconds, Is.EqualTo(4));
            Assert.That(eventLog.Entries[1].Severity, Is.EqualTo(EventLogSeverity.Warning));
            Assert.That(advancer.Clock.ElapsedSeconds, Is.EqualTo(4));
        }

        [Test]
        public void CanStartAction_FalseOnlyWhileAFindWaitsForRoom()
        {
            SurvivorActionCompletionService completion = Create(out _, out _, out GroundInventoryService groundInventory);
            Assert.That(completion.CanStartAction, Is.True);

            groundInventory.QueueOverflow(new[] { new CountedThing<ItemDefinition>(TestZoneDefinitions.Pebble, 1) });

            Assert.That(completion.CanStartAction, Is.False);
            Assert.That(() => completion.RequireCanStartAction(), Throws.InvalidOperationException);
        }

        [Test]
        public void Complete_NullArguments_AreRefused()
        {
            SurvivorActionCompletionService completion = Create(out _, out _, out _);

            Assert.That(() => completion.Complete(null, Array.Empty<EventLogMessage>()), Throws.ArgumentNullException);
            Assert.That(() => completion.Complete(ActionDuration.FromSeconds(1), (IEnumerable<EventLogMessage>)null), Throws.ArgumentNullException);
        }
    }
}
