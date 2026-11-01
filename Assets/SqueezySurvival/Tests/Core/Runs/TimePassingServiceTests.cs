using NUnit.Framework;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Time;

namespace SqueezySurvival.Core.Tests.Runs
{
    [TestFixture]
    public sealed class TimePassingServiceTests
    {
        [TestCase(TimePassingAction.Wait, 15, "You waited 15 minutes.")]
        [TestCase(TimePassingAction.Rest, 60, "You rested for an hour.")]
        public void PassTime_CompletesTheActionWithItsDurationAndLogText(TimePassingAction action, int expectedMinutes, string expectedLogText)
        {
            var completion = new RecordingSurvivorActionCompletionService();
            var timePassing = new TimePassingService(completion);

            timePassing.PassTime(action);

            Assert.That(completion.Completed, Has.Count.EqualTo(1));
            Assert.That(completion.Completed[0].Duration, Is.EqualTo(ActionDuration.FromMinutes(expectedMinutes)));
            Assert.That(completion.Completed[0].MessageTexts, Is.EqualTo(new[] { expectedLogText }));
        }

        [Test]
        public void PassTime_WhenActionsCannotStart_IsUnavailableAndRefused()
        {
            var completion = new RecordingSurvivorActionCompletionService { CanStartAction = false };
            var timePassing = new TimePassingService(completion);

            Assert.That(timePassing.CanPassTime, Is.False);
            Assert.That(() => timePassing.PassTime(TimePassingAction.Wait), Throws.InvalidOperationException);
            Assert.That(completion.Completed, Is.Empty);
        }

        [Test]
        public void PassTime_WhileAFindWaitsForRoom_IsRefusedWithoutPassingTime()
        {
            RunSession session = TestRunSessions.WithPebbleWaitingForRoom();
            long secondsBefore = session.Clock.ElapsedSeconds;

            Assert.That(() => session.TimePassing.PassTime(TimePassingAction.Wait), Throws.InvalidOperationException);
            Assert.That(session.Clock.ElapsedSeconds, Is.EqualTo(secondsBefore));
        }
    }
}
