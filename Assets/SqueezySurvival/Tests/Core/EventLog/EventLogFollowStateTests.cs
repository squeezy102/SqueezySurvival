using NUnit.Framework;
using SqueezySurvival.Core.EventLog;

namespace SqueezySurvival.Core.Tests.EventLog
{
    [TestFixture]
    public sealed class EventLogFollowStateTests
    {
        [Test]
        public void NewLog_FollowsNewest()
        {
            Assert.That(new EventLogFollowState().FollowsNewest, Is.True);
        }

        [Test]
        public void PlayerScrolled_AwayFromTheBottom_StopsFollowing()
        {
            var state = new EventLogFollowState();

            state.PlayerScrolled(isScrolledToBottom: false);

            Assert.That(state.FollowsNewest, Is.False);
        }

        [Test]
        public void PlayerScrolled_BackToTheBottom_FollowsAgain()
        {
            var state = new EventLogFollowState();
            state.PlayerScrolled(isScrolledToBottom: false);

            state.PlayerScrolled(isScrolledToBottom: true);

            Assert.That(state.FollowsNewest, Is.True);
        }

        [Test]
        public void WentToOlderEntry_StopsFollowing()
        {
            var state = new EventLogFollowState();

            state.WentToOlderEntry();

            Assert.That(state.FollowsNewest, Is.False);
        }

        [Test]
        public void JumpedToNewest_AfterScrollingUp_FollowsAgain()
        {
            var state = new EventLogFollowState();
            state.PlayerScrolled(isScrolledToBottom: false);

            state.JumpedToNewest();

            Assert.That(state.FollowsNewest, Is.True);
        }
    }
}
