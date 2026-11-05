using NUnit.Framework;
using SqueezySurvival.Core.GameScreen;
using SqueezySurvival.Core.InventoryGrids;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Tests.Runs;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Presentation.Game;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Game
{
    /// <summary>
    /// Escape does the first thing that applies, in Core's order (Docs/Design/UI/Controls.md, Escape). Core's resolver
    /// is tested on its own; these tests check that the screen reads the state of every grid and the question, and
    /// that each decision reaches the grids, the question and the overlays. Escape is pressed on a test keyboard
    /// (Docs/Decisions/ADR-0008-Presentation-Testing.md).
    /// </summary>
    [TestFixture]
    public sealed class GameScreenEscapeKeyPresenterTests : InventoryGridUITestFixture
    {
        private const float DragDistancePixels = 40f;

        // A fixture that already has a base class creates the Input System fixture and calls its Setup and TearDown
        // itself (Input System manual: testing).
        private readonly InputTestFixture _input = new InputTestFixture();
        private Keyboard _keyboard;
        private InventoryGridView _groundGrid;
        private InventoryGridView _inventoryGrid;
        private GameScreenOverlayStack _overlays;
        private GroundOverflowPromptController _prompt;
        private GameScreenEscapeKeyPresenter _presenter;

        [SetUp]
        public void AddATestKeyboard()
        {
            _input.Setup();
            _keyboard = InputSystem.AddDevice<Keyboard>();
        }

        [TearDown]
        public void DisposeTheScreen()
        {
            _presenter?.Dispose();
            _prompt?.Dispose();
            _input.TearDown();
        }

        // Builds the grids, the question and the Escape presenter over a run, as the game's composition root does. A
        // pebble is in the first slot of each grid so there is something to select and drag.
        private void ComposeScreen(RunSession session)
        {
            var shapes = new InventoryGridShapes(SlotCapacityRules.Wireframe);
            var groundThings = new CountedCollection<ItemDefinition>(shapes.Ground.SlotCount);
            groundThings.Receive(TestZoneDefinitions.Pebble, 1, 0);
            var inventoryThings = new CountedCollection<ItemDefinition>(shapes.Inventory.SlotCount);
            inventoryThings.Receive(TestZoneDefinitions.Pebble, 1, 0);
            _groundGrid = CreateGrid(shapes.Ground, "ground-pool-grid", groundThings);
            _inventoryGrid = CreateGrid(shapes.Inventory, "inventory-grid", inventoryThings);
            _prompt = new GroundOverflowPromptController(
                session.GroundInventory, _groundGrid, Element<VisualElement>("ground-overflow-prompt"), Element<Label>("ground-discard-hint"));
            _overlays = new GameScreenOverlayStack();
            _presenter = new GameScreenEscapeKeyPresenter(
                new GameScreenEscapeResolver(_overlays), new[] { _groundGrid, _inventoryGrid }, _prompt);
            simulate.FrameUpdate();
        }

        private void PressEscape() => _input.PressAndRelease(_keyboard.escapeKey);

        private bool QuestionIsShowing => Element<VisualElement>("ground-overflow-prompt").style.display == DisplayStyle.Flex;

        [Test]
        public void Escape_WhileDragging_CancelsTheDragAndOpensNothing()
        {
            ComposeScreen(TestRunSessions.Start());
            Vector2 start = _groundGrid.SlotWorldBound(0).center;
            simulate.MouseDown(start);
            simulate.MouseMove(start + new Vector2(DragDistancePixels, 0));
            Assert.That(_groundGrid.IsDragging, Is.True, "The drag never began, so Escape has nothing to cancel.");

            PressEscape();

            Assert.That(_groundGrid.IsDragging, Is.False);
            Assert.That(_overlays.Top, Is.Null);
        }

        [Test]
        public void Escape_WithASelectionInTheSecondGrid_ClearsItAndOpensNothing()
        {
            ComposeScreen(TestRunSessions.Start());
            simulate.Click(_inventoryGrid.SlotWorldBound(0).center);
            simulate.FrameUpdate();
            Assert.That(_inventoryGrid.HasSelection, Is.True, "Nothing was selected, so Escape has nothing to clear.");

            PressEscape();

            Assert.That(_inventoryGrid.HasSelection, Is.False);
            Assert.That(_overlays.Top, Is.Null);
        }

        [Test]
        public void Escape_WhileChoosingWhatToDiscard_ReturnsToTheQuestion()
        {
            ComposeScreen(TestRunSessions.WithPebbleWaitingForRoom());
            simulate.Click(Element<Button>("ground-overflow-discard-button"));
            simulate.FrameUpdate();
            Assert.That(_prompt.IsChoosingDiscard, Is.True, "Choosing a discard never began, so Escape has nothing to leave.");

            PressEscape();

            Assert.That(_prompt.IsChoosingDiscard, Is.False);
            Assert.That(QuestionIsShowing, Is.True);
            Assert.That(_overlays.Top, Is.Null);
        }

        [Test]
        public void Escape_WhileTheQuestionShows_KeepsAskingAndOpensNothing()
        {
            ComposeScreen(TestRunSessions.WithPebbleWaitingForRoom());
            Assert.That(QuestionIsShowing, Is.True, "The question is not showing, so Escape has nothing to wait on.");

            PressEscape();

            Assert.That(QuestionIsShowing, Is.True);
            Assert.That(_overlays.Top, Is.Null);
        }

        [Test]
        public void Escape_WithNothingActive_OpensThePauseMenu()
        {
            ComposeScreen(TestRunSessions.Start());

            PressEscape();

            Assert.That(_overlays.Top, Is.EqualTo(GameScreenOverlay.PauseMenu));
        }

        [Test]
        public void Escape_AfterDispose_DoesNothing()
        {
            ComposeScreen(TestRunSessions.Start());
            _presenter.Dispose();

            PressEscape();

            Assert.That(_overlays.Top, Is.Null);
        }
    }
}
