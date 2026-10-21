using NUnit.Framework;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Tests.Runs;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Zones;
using SqueezySurvival.Presentation.Game;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Game
{
    [TestFixture]
    public sealed class TimeTakingActionsPresenterTests : GameScreenUITestFixture
    {
        private RunSession _session;
        private TimeTakingActionsPresenter _presenter;

        private Button ExploreButton => Element<Button>("explore-button");

        private Button WaitButton => Element<Button>("wait-button");

        private Button RestButton => Element<Button>("rest-button");

        [TearDown]
        public void DisposePresenter() => _presenter?.Dispose();

        [Test]
        public void New_ShowsEachActionWithItsDuration()
        {
            Bind(TestRunSessions.Start());

            Assert.That(ExploreButton.text, Is.EqualTo("Explore (15 min)"));
            Assert.That(WaitButton.text, Is.EqualTo("Wait (15 min)"));
            Assert.That(RestButton.text, Is.EqualTo("Rest (1 hr)"));
        }

        [TestCase("wait-button", 15)]
        [TestCase("rest-button", 60)]
        public void ClickingATimePassingButton_PassesItsTime(string buttonName, int expectedMinutes)
        {
            Bind(TestRunSessions.Start());

            simulate.Click(Element<Button>(buttonName));

            Assert.That(_session.Clock.ElapsedMinutes, Is.EqualTo(expectedMinutes));
        }

        [Test]
        public void ClickingExplore_ExploresTheZone()
        {
            Bind(TestRunSessions.Start(1, 0, 99));

            simulate.Click(ExploreButton);

            Assert.That(_session.CurrentZone.ExplorationProgress.ExploresCompleted, Is.EqualTo(1));
        }

        [Test]
        public void WhileAFindWaitsForRoom_EveryActionIsDisabledUntilItIsResolved()
        {
            Bind(TestRunSessions.Start(1, 0, 99));
            _session.GroundInventory.Ground.AddRange(TestZoneDefinitions.DistinctItems(_session.GroundInventory.Ground.SlotCapacity));

            _session.ZoneExploration.Explore();

            Assert.That(ExploreButton.enabledSelf, Is.False);
            Assert.That(WaitButton.enabledSelf, Is.False);
            Assert.That(RestButton.enabledSelf, Is.False);

            _session.GroundInventory.LeavePendingOverflowBehind();

            Assert.That(ExploreButton.enabledSelf, Is.True);
            Assert.That(WaitButton.enabledSelf, Is.True);
            Assert.That(RestButton.enabledSelf, Is.True);
        }

        [Test]
        public void WhileImmobile_OnlyExploreIsDisabled()
        {
            Bind(TestRunSessions.Start());

            _session.SurvivorInventory.Inventory.Receive(PlaceholderBeachZoneContent.Log, 4, -1);

            Assert.That(ExploreButton.enabledSelf, Is.False);
            Assert.That(WaitButton.enabledSelf, Is.True);
            Assert.That(RestButton.enabledSelf, Is.True);
        }

        [Test]
        public void ImmobileFromTheHands_DisablesExploreToo()
        {
            Bind(TestRunSessions.Start());
            _session.SurvivorInventory.Inventory.Receive(PlaceholderBeachZoneContent.Log, 3, -1);

            _session.SurvivorInventory.Hands.Receive(PlaceholderBeachZoneContent.Log, 1, 0);

            Assert.That(ExploreButton.enabledSelf, Is.False);
        }

        [Test]
        public void AfterDispose_StopsRefreshing()
        {
            Bind(TestRunSessions.Start());
            _presenter.Dispose();

            _session.SurvivorInventory.Inventory.Receive(PlaceholderBeachZoneContent.Log, 4, -1);

            Assert.That(ExploreButton.enabledSelf, Is.True);
        }

        private void Bind(RunSession session)
        {
            _session = session;
            _presenter = new TimeTakingActionsPresenter(
                rootVisualElement, session.ZoneExploration, session.TimePassing, session.SurvivorInventory, session.GroundInventory);
            simulate.FrameUpdate();
        }
    }
}
