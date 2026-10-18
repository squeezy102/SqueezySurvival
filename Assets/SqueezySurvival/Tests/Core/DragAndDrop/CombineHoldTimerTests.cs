using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;

namespace SqueezySurvival.Core.Tests.DragAndDrop
{
    [TestFixture]
    public sealed class CombineHoldTimerTests
    {
        // Just under the hold, exactly the hold, and over it.
        [TestCase(10.99, false)]
        [TestCase(11.0, true)]
        [TestCase(12.5, true)]
        public void Update_SameSlot_AttemptsCombineOnceHeldForTheHoldTime(double now, bool expected)
        {
            var timer = new CombineHoldTimer();
            timer.Update(3, 10.0);

            Assert.That(timer.Update(3, now), Is.EqualTo(expected));
        }

        [Test]
        public void Update_MovingToAnotherSlot_RestartsTheHold()
        {
            var timer = new CombineHoldTimer();
            timer.Update(3, 10.0);

            timer.Update(4, 10.5);

            Assert.That(timer.Update(4, 11.2), Is.False);
            Assert.That(timer.Update(4, 11.5), Is.True);
        }

        [Test]
        public void Update_LeavingAndReturning_RestartsTheHold()
        {
            var timer = new CombineHoldTimer();
            timer.Update(3, 10.0);

            timer.Update(-1, 10.8);

            Assert.That(timer.Update(3, 11.2), Is.False);
        }
    }
}
