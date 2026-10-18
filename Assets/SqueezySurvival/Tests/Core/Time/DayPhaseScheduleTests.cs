using System;
using System.Collections.Generic;
using NUnit.Framework;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.Time
{
    [TestFixture]
    public sealed class DayPhaseScheduleTests
    {
        [TestCase(0, DayPhase.Night)]
        [TestCase(4 * 60 + 59, DayPhase.Night)]
        [TestCase(5 * 60, DayPhase.Dawn)]
        [TestCase(7 * 60, DayPhase.Morning)]
        [TestCase(11 * 60, DayPhase.Midday)]
        [TestCase(14 * 60, DayPhase.Afternoon)]
        [TestCase(18 * 60, DayPhase.Dusk)]
        [TestCase(21 * 60 + 59, DayPhase.Dusk)]
        [TestCase(22 * 60, DayPhase.Night)]
        [TestCase(23 * 60 + 59, DayPhase.Night)]
        public void PhaseAt_DefaultSchedule_MatchesTimeDesign(int minuteOfDay, DayPhase expected)
        {
            Assert.That(DayPhaseSchedule.Default.PhaseAt(minuteOfDay), Is.EqualTo(expected));
        }

        [Test]
        public void Constructor_WithMissingPhase_Throws()
        {
            var starts = new Dictionary<DayPhase, int> { { DayPhase.Dawn, 300 } };

            Assert.That(() => new DayPhaseSchedule(starts), Throws.ArgumentException);
        }

        [TestCase(-1)]
        [TestCase(GameClock.MinutesPerDay)]
        public void PhaseAt_OutsideDay_Throws(int minuteOfDay)
        {
            Assert.That(() => DayPhaseSchedule.Default.PhaseAt(minuteOfDay), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
