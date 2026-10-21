using NUnit.Framework;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Tests.Runs;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Presentation.Game;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Game
{
    [TestFixture]
    public sealed class HudClockPresenterTests : GameScreenUITestFixture
    {
        private RunSession _session;
        private HudClockPresenter _presenter;

        [SetUp]
        public void CreatePresenter()
        {
            _session = TestRunSessions.Start();
            _presenter = new HudClockPresenter(rootVisualElement, _session.GameTime, _session.DayPhaseSchedule);
        }

        [TearDown]
        public void DisposePresenter() => _presenter.Dispose();

        [Test]
        public void New_ShowsTheRunStartAndPaintsTheDial()
        {
            Assert.That(Element<Label>("hud-clock-day").text, Is.EqualTo("Day 1"));
            Assert.That(Element<Label>("hud-clock-time").text, Is.EqualTo("6:00:00 AM"));
            Assert.That(Element<Label>("hud-clock-phase").text, Is.EqualTo("Dawn"));
            Assert.That(Element<VisualElement>("hud-day-night-dial").style.backgroundImage.value.texture, Is.Not.Null);
        }

        [Test]
        public void TimeAdvanced_ShowsTheNewTimeAndPhase()
        {
            _session.TimePassing.PassTime(TimePassingAction.Rest);

            Assert.That(Element<Label>("hud-clock-time").text, Is.EqualTo("7:00:00 AM"));
            Assert.That(Element<Label>("hud-clock-phase").text, Is.EqualTo("Morning"));
        }

        [Test]
        public void AfterDispose_StopsFollowingTheClock()
        {
            _presenter.Dispose();

            _session.TimePassing.PassTime(TimePassingAction.Rest);

            Assert.That(Element<Label>("hud-clock-time").text, Is.EqualTo("6:00:00 AM"));
        }
    }
}
