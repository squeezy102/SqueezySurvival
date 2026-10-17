using NUnit.Framework;
using SqueezySurvival.Core.NewGame;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.Tests.NewGame
{
    [TestFixture]
    public sealed class NewGameSetupTests
    {
        [Test]
        public void New_DominantHand_IsNotChosen()
        {
            Assert.That(new NewGameSetup().DominantHand, Is.Null);
        }

        [Test]
        public void CanStart_WithoutDominantHand_IsFalseWithReason()
        {
            var setup = new NewGameSetup();

            Assert.That(setup.CanStart, Is.False);
            Assert.That(setup.StartUnavailableReason, Is.EqualTo(NewGameSetup.ChooseDominantHandReason));
        }

        [TestCase(DominantHand.Left)]
        [TestCase(DominantHand.Right)]
        public void CanStart_WithDominantHand_IsTrue(DominantHand hand)
        {
            var setup = new NewGameSetup { DominantHand = hand };

            Assert.That(setup.CanStart, Is.True);
            Assert.That(setup.StartUnavailableReason, Is.Empty);
        }

        [Test]
        public void New_HasNoRunOptions()
        {
            var setup = new NewGameSetup();

            Assert.That(setup.IsCasual || setup.IsEasy || setup.IsHard, Is.False);
        }

        [Test]
        public void SetHard_WhenCasualAndEasyTicked_ClearsBoth()
        {
            var setup = new NewGameSetup();
            setup.SetCasual(true);
            setup.SetEasy(true);

            setup.SetHard(true);

            Assert.That(setup.IsHard, Is.True);
            Assert.That(setup.IsCasual || setup.IsEasy, Is.False);
        }

        [Test]
        public void SetCasual_WhenHardTicked_ClearsHard()
        {
            var setup = new NewGameSetup();
            setup.SetHard(true);

            setup.SetCasual(true);

            Assert.That(setup.IsHard, Is.False);
        }

        [Test]
        public void SetEasy_WhenHardTicked_ClearsHard()
        {
            var setup = new NewGameSetup();
            setup.SetHard(true);

            setup.SetEasy(true);

            Assert.That(setup.IsHard, Is.False);
        }

        [Test]
        public void CasualAndEasyInConflict_WhenHardTicked_AreTrue()
        {
            var setup = new NewGameSetup();
            setup.SetHard(true);

            Assert.That(setup.IsCasualInConflict && setup.IsEasyInConflict, Is.True);
        }

        [Test]
        public void HardInConflict_WhenEasyTicked_IsTrue()
        {
            var setup = new NewGameSetup();
            setup.SetEasy(true);

            Assert.That(setup.IsHardInConflict, Is.True);
        }

        [Test]
        public void NoOptionInConflict_WhenNothingTicked()
        {
            var setup = new NewGameSetup();

            Assert.That(setup.IsCasualInConflict || setup.IsEasyInConflict || setup.IsHardInConflict, Is.False);
        }

        [Test]
        public void CreateRunConfiguration_WithoutDominantHand_Throws()
        {
            Assert.That(() => new NewGameSetup().CreateRunConfiguration(), Throws.InvalidOperationException);
        }

        [Test]
        public void CreateRunConfiguration_CarriesEveryChoice()
        {
            var setup = new NewGameSetup { DominantHand = DominantHand.Left };
            setup.SetHard(true);

            var configuration = setup.CreateRunConfiguration();

            Assert.That(configuration.DominantHand, Is.EqualTo(DominantHand.Left));
            Assert.That(configuration.RunOptions.IsHard, Is.True);
            Assert.That(configuration.RunOptions.IsCasual || configuration.RunOptions.IsEasy, Is.False);
        }

        [Test]
        public void ToRunOptions_ReflectsTickedOptions()
        {
            var setup = new NewGameSetup();
            setup.SetCasual(true);
            setup.SetEasy(true);

            var options = setup.ToRunOptions();

            Assert.That(options.IsCasual && options.IsEasy && !options.IsHard, Is.True);
        }
    }
}
