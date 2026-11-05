using NUnit.Framework;
using SqueezySurvival.Core.InventoryGrids;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Tests.Runs;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Presentation.Game;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Game
{
    /// <summary>
    /// The question for a find with no room on the ground (Docs/Design/UI/InventoryGrids.md, When the ground is full).
    /// Core decides the rules and the words; these tests check the screen shows them and forwards the player's choice.
    /// </summary>
    [TestFixture]
    public sealed class GroundOverflowPromptControllerTests : InventoryGridUITestFixture
    {
        private const int SlotToDiscard = 5;

        private RunSession _session;
        private InventoryGridView _groundGrid;
        private GroundOverflowPromptController _prompt;

        [TearDown]
        public void DisposePrompt() => _prompt?.Dispose();

        private void ComposePrompt(RunSession session)
        {
            _session = session;
            var shapes = new InventoryGridShapes(SlotCapacityRules.Wireframe);
            _groundGrid = CreateGrid(shapes.Ground, "ground-pool-grid", _session.GroundInventory.Ground);
            _prompt = new GroundOverflowPromptController(
                _session.GroundInventory, _groundGrid, Element<VisualElement>("ground-overflow-prompt"), Element<Label>("ground-discard-hint"));
            simulate.FrameUpdate();
        }

        private static CountedThing<ItemDefinition> WaitingPebble => new CountedThing<ItemDefinition>(TestZoneDefinitions.Pebble, 1);

        private bool QuestionIsShowing => Element<VisualElement>("ground-overflow-prompt").style.display == DisplayStyle.Flex;

        private bool HintIsShowing => Element<Label>("ground-discard-hint").style.display == DisplayStyle.Flex;

        private void ClickChooseDiscard()
        {
            simulate.Click(Element<Button>("ground-overflow-discard-button"));
            simulate.FrameUpdate();
        }

        [Test]
        public void New_WithAFindWaiting_AsksWhatToDoWithIt()
        {
            ComposePrompt(TestRunSessions.WithPebbleWaitingForRoom());

            Assert.That(QuestionIsShowing, Is.True);
            Assert.That(_prompt.IsAsking, Is.True);
            Assert.That(Element<Label>("ground-overflow-question").text, Is.EqualTo(GroundOverflowTextFormatter.FormatQuestion(WaitingPebble)));
            Assert.That(Element<Button>("ground-overflow-leave-button").text, Is.EqualTo(GroundOverflowTextFormatter.FormatLeaveBehindChoice(WaitingPebble)));
            Assert.That(Element<Label>("ground-overflow-leave-warning").text, Is.EqualTo(GroundOverflowTextFormatter.FormatLeaveBehindWarning(WaitingPebble)));
            Assert.That(Element<Label>("ground-overflow-discard-warning").text, Is.EqualTo(GroundOverflowTextFormatter.FormatDiscardChoiceWarning(WaitingPebble)));
        }

        [Test]
        public void New_WithNothingWaiting_AsksNothing()
        {
            ComposePrompt(TestRunSessions.Start());

            Assert.That(QuestionIsShowing, Is.False);
            Assert.That(_prompt.IsAsking, Is.False);
        }

        [Test]
        public void QueueingAFind_ShowsTheQuestion()
        {
            ComposePrompt(TestRunSessions.Start());

            _session.GroundInventory.QueueOverflow(new[] { WaitingPebble });

            Assert.That(QuestionIsShowing, Is.True);
        }

        [Test]
        public void LeaveBehindButton_Click_LeavesTheFindBehindAndHidesTheQuestion()
        {
            ComposePrompt(TestRunSessions.WithPebbleWaitingForRoom());

            simulate.Click(Element<Button>("ground-overflow-leave-button"));
            simulate.FrameUpdate();

            Assert.That(_session.GroundInventory.HasPendingOverflow, Is.False);
            Assert.That(QuestionIsShowing, Is.False);
        }

        [Test]
        public void ChooseDiscardButton_Click_HidesTheQuestionAndShowsTheHint()
        {
            ComposePrompt(TestRunSessions.WithPebbleWaitingForRoom());

            ClickChooseDiscard();

            Assert.That(_prompt.IsChoosingDiscard, Is.True);
            Assert.That(QuestionIsShowing, Is.False);
            Assert.That(HintIsShowing, Is.True);
            Assert.That(Element<Label>("ground-discard-hint").text, Is.EqualTo(GroundOverflowTextFormatter.FormatDiscardHint(WaitingPebble)));
        }

        [Test]
        public void ClickingASlotWhileChoosingWhatToDiscard_DiscardsItAndEndsTheChoice()
        {
            ComposePrompt(TestRunSessions.WithPebbleWaitingForRoom());
            ClickChooseDiscard();

            simulate.Click(_groundGrid.SlotWorldBound(SlotToDiscard).center);
            simulate.FrameUpdate();

            Assert.That(_session.GroundInventory.HasPendingOverflow, Is.False);
            Assert.That(_session.GroundInventory.Ground.EntryAtSlot(SlotToDiscard).Definition, Is.SameAs(TestZoneDefinitions.Pebble));
            Assert.That(_prompt.IsChoosingDiscard, Is.False);
            Assert.That(HintIsShowing, Is.False);
            Assert.That(QuestionIsShowing, Is.False);
        }

        [Test]
        public void ReturnToQuestion_WhileChoosingWhatToDiscard_AsksAgainAndHidesTheHint()
        {
            ComposePrompt(TestRunSessions.WithPebbleWaitingForRoom());
            ClickChooseDiscard();

            _prompt.ReturnToQuestion();

            Assert.That(_prompt.IsChoosingDiscard, Is.False);
            Assert.That(QuestionIsShowing, Is.True);
            Assert.That(HintIsShowing, Is.False);
            Assert.That(_session.GroundInventory.HasPendingOverflow, Is.True);
        }

        [Test]
        public void AfterDispose_StopsFollowingTheQueue()
        {
            ComposePrompt(TestRunSessions.Start());
            _prompt.Dispose();

            _session.GroundInventory.QueueOverflow(new[] { WaitingPebble });

            Assert.That(QuestionIsShowing, Is.False);
        }
    }
}
