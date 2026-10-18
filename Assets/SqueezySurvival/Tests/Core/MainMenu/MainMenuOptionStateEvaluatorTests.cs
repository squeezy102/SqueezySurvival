using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.MainMenu;

namespace SqueezySurvival.Core.Tests.MainMenu
{
    [TestFixture]
    public sealed class MainMenuOptionStateEvaluatorTests
    {
        [Test]
        public void EvaluateAll_ReturnsOptionsInDisplayOrder()
        {
            var evaluator = new MainMenuOptionStateEvaluator(new FakeSavedGameAvailability(false, false));

            MainMenuOption[] options = evaluator.EvaluateAll().Select(state => state.Option).ToArray();

            Assert.That(options, Is.EqualTo(new[]
            {
                MainMenuOption.NewGame,
                MainMenuOption.Continue,
                MainMenuOption.LoadGame,
                MainMenuOption.Options,
                MainMenuOption.Quit,
            }));
        }

        [TestCase(MainMenuOption.NewGame)]
        [TestCase(MainMenuOption.Options)]
        [TestCase(MainMenuOption.Quit)]
        public void Evaluate_OptionWithNoRequirements_WhenNoSavesExist_IsAvailable(MainMenuOption option)
        {
            var evaluator = new MainMenuOptionStateEvaluator(new FakeSavedGameAvailability(false, false));

            Assert.That(evaluator.Evaluate(option).IsAvailable, Is.True);
        }

        [Test]
        public void Evaluate_Continue_WhenNoRunInProgress_IsUnavailableWithReason()
        {
            var evaluator = new MainMenuOptionStateEvaluator(new FakeSavedGameAvailability(false, true));

            MainMenuOptionState state = evaluator.Evaluate(MainMenuOption.Continue);

            Assert.That(state.IsAvailable, Is.False);
            Assert.That(state.UnavailableReason, Is.EqualTo(MainMenuOptionStateEvaluator.NoGameInProgressReason));
        }

        [Test]
        public void Evaluate_Continue_WhenRunInProgress_IsAvailable()
        {
            var evaluator = new MainMenuOptionStateEvaluator(new FakeSavedGameAvailability(true, false));

            Assert.That(evaluator.Evaluate(MainMenuOption.Continue).IsAvailable, Is.True);
        }

        [Test]
        public void Evaluate_LoadGame_WhenNothingLoadable_IsUnavailableWithReason()
        {
            var evaluator = new MainMenuOptionStateEvaluator(new FakeSavedGameAvailability(true, false));

            MainMenuOptionState state = evaluator.Evaluate(MainMenuOption.LoadGame);

            Assert.That(state.IsAvailable, Is.False);
            Assert.That(state.UnavailableReason, Is.EqualTo(MainMenuOptionStateEvaluator.NoSavedGamesReason));
        }

        [Test]
        public void Evaluate_LoadGame_WhenSomethingLoadable_IsAvailable()
        {
            var evaluator = new MainMenuOptionStateEvaluator(new FakeSavedGameAvailability(false, true));

            Assert.That(evaluator.Evaluate(MainMenuOption.LoadGame).IsAvailable, Is.True);
        }
    }
}
