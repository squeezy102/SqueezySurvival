using System;
using System.Collections.Generic;
using NUnit.Framework;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.Time
{
    [TestFixture]
    public sealed class GameTimeAdvancerTests
    {
        private static GameTimeAdvancer CreateAdvancer(params ITimeDependentSystem[] systemsInOrder) =>
            new GameTimeAdvancer(GameClock.RunStart, systemsInOrder);

        [Test]
        public void NewAdvancer_ClockIsTheStart()
        {
            var start = new GameClock(90);

            Assert.That(new GameTimeAdvancer(start, Array.Empty<ITimeDependentSystem>()).Clock, Is.SameAs(start));
        }

        [Test]
        public void Advance_ActionOfSeconds_MovesTheClockAndTellsSystemsTheExactSeconds()
        {
            var system = new RecordingTimeDependentSystem();
            GameTimeAdvancer advancer = CreateAdvancer(system);

            advancer.Advance(ActionDuration.FromSeconds(7));

            Assert.That(advancer.Clock.ElapsedSeconds, Is.EqualTo(7));
            Assert.That(system.Advances, Has.Count.EqualTo(1));
            Assert.That(system.Advances[0].Duration, Is.EqualTo(ActionDuration.FromSeconds(7)));
        }

        [Test]
        public void Advance_ActionOfMinutes_MovesTheClockAndTellsSystemsTheExactMinutes()
        {
            var system = new RecordingTimeDependentSystem();
            GameTimeAdvancer advancer = CreateAdvancer(system);

            advancer.Advance(ActionDuration.FromMinutes(15));

            Assert.That(advancer.Clock.ElapsedMinutes, Is.EqualTo(15));
            Assert.That(system.Advances[0].Duration, Is.EqualTo(ActionDuration.FromMinutes(15)));
        }

        [Test]
        public void Advance_Twice_EachAdvanceStartsWhereTheLastEnded()
        {
            var system = new RecordingTimeDependentSystem();
            GameTimeAdvancer advancer = CreateAdvancer(system);

            advancer.Advance(ActionDuration.FromSeconds(5));
            advancer.Advance(ActionDuration.FromMinutes(1));

            Assert.That(system.Advances[1].Previous, Is.SameAs(system.Advances[0].Current));
            Assert.That(advancer.Clock.ElapsedSeconds, Is.EqualTo(65));
        }

        [Test]
        public void Advance_TellsSystemsInTheOrderGiven()
        {
            var told = new List<string>();
            GameTimeAdvancer advancer = CreateAdvancer(
                new RecordingTimeDependentSystem(_ => told.Add("first")),
                new RecordingTimeDependentSystem(_ => told.Add("second")),
                new RecordingTimeDependentSystem(_ => told.Add("third")));

            advancer.Advance(ActionDuration.FromMinutes(1));

            Assert.That(told, Is.EqualTo(new[] { "first", "second", "third" }));
        }

        [Test]
        public void Advance_RaisesTimeAdvancedAfterEverySystemWithTheSameAdvance()
        {
            var system = new RecordingTimeDependentSystem();
            GameTimeAdvancer advancer = CreateAdvancer(system);
            int systemAdvancesWhenRaised = -1;
            GameTimeAdvance raised = null;
            advancer.TimeAdvanced += (sender, advance) =>
            {
                systemAdvancesWhenRaised = system.Advances.Count;
                raised = advance;
            };

            GameTimeAdvance returned = advancer.Advance(ActionDuration.FromMinutes(1));

            Assert.That(systemAdvancesWhenRaised, Is.EqualTo(1));
            Assert.That(raised, Is.SameAs(returned));
            Assert.That(raised, Is.SameAs(system.Advances[0]));
            Assert.That(raised.Previous, Is.SameAs(GameClock.RunStart));
            Assert.That(raised.Current, Is.SameAs(advancer.Clock));
        }

        [Test]
        public void Advance_FromInsideATimeDependentSystem_Throws()
        {
            GameTimeAdvancer advancer = null;
            advancer = CreateAdvancer(new RecordingTimeDependentSystem(_ => advancer.Advance(ActionDuration.FromSeconds(1))));

            Assert.That(() => advancer.Advance(ActionDuration.FromMinutes(1)), Throws.InvalidOperationException);
        }

        [Test]
        public void Advance_FromInsideTimeAdvanced_Throws()
        {
            GameTimeAdvancer advancer = CreateAdvancer();
            advancer.TimeAdvanced += (sender, advance) => advancer.Advance(ActionDuration.FromSeconds(1));

            Assert.That(() => advancer.Advance(ActionDuration.FromMinutes(1)), Throws.InvalidOperationException);
        }

        [Test]
        public void Advance_AfterARefusedNestedAdvance_WorksAgain()
        {
            GameTimeAdvancer advancer = CreateAdvancer();
            EventHandler<GameTimeAdvance> nested = (sender, advance) => advancer.Advance(ActionDuration.FromSeconds(1));
            advancer.TimeAdvanced += nested;
            Assert.That(() => advancer.Advance(ActionDuration.FromMinutes(1)), Throws.InvalidOperationException);
            advancer.TimeAdvanced -= nested;

            advancer.Advance(ActionDuration.FromMinutes(1));

            Assert.That(advancer.Clock.ElapsedMinutes, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_WithANullSystem_Throws()
        {
            Assert.That(() => CreateAdvancer(new RecordingTimeDependentSystem(), null), Throws.ArgumentException);
        }
    }
}
