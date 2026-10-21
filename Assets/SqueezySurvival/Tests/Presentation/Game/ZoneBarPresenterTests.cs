using NUnit.Framework;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Tests.Runs;
using SqueezySurvival.Presentation.Game;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Game
{
    [TestFixture]
    public sealed class ZoneBarPresenterTests : GameScreenUITestFixture
    {
        // The test Cove takes 4 explores to map, so one explore is 25%.
        private RunSession _session;
        private ZoneBarPresenter _presenter;

        [SetUp]
        public void CreatePresenter()
        {
            _session = TestRunSessions.Start(1, 0, 99, 1, 0, 99);
            _presenter = new ZoneBarPresenter(rootVisualElement, _session.CurrentZone);
        }

        [TearDown]
        public void DisposePresenter() => _presenter.Dispose();

        [Test]
        public void New_ShowsTheZoneNameAndNoProgress()
        {
            Assert.That(Element<Label>("zone-name").text, Is.EqualTo("Cove"));
            Assert.That(Element<Label>("zone-exploration-progress").text, Is.EqualTo("Explored 0%"));
        }

        [Test]
        public void Explore_ShowsTheNewProgress()
        {
            _session.ZoneExploration.Explore();

            Assert.That(Element<Label>("zone-exploration-progress").text, Is.EqualTo("Explored 25%"));
        }

        [Test]
        public void AfterDispose_StopsFollowingProgress()
        {
            _presenter.Dispose();

            _session.ZoneExploration.Explore();

            Assert.That(Element<Label>("zone-exploration-progress").text, Is.EqualTo("Explored 0%"));
        }
    }
}
