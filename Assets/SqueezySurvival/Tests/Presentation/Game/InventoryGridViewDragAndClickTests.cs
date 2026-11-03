using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.InventoryGrids;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Presentation.Game;
using SqueezySurvival.Presentation.PixelArt;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Game
{
    /// <summary>
    /// A drag and a click are separate behaviors: a press of a cell ends as one or the other, never both
    /// (Docs/Design/UI/DragAndDrop.md, Dragging). A click selects; with Ctrl or Alt held it would send things to the
    /// other panel instead, which these tests do not hold because that needs a keyboard.
    /// </summary>
    [TestFixture]
    public sealed class InventoryGridViewDragAndClickTests : GameScreenUITestFixture
    {
        private const int StoneCount = 4;
        private const float SmallDragPixels = 8f;
        private const float GridWidthPixels = 612f;
        private const float GridHeightPixels = 204f;

        private PixelArtTextureCache _textures;
        private CountedCollection<ItemDefinition> _stones;
        private InventoryGridView _grid;
        private RecordingInventoryTransferService _transfers;

        [SetUp]
        public void CreateGridWithAStackOfStonesInTheFirstSlot()
        {
            _textures = new PixelArtTextureCache();
            var skin = new PixelArtInterfaceSkin(_textures);
            var tooltip = new GridTooltipView(Element<VisualElement>("grid-tooltip"), _textures);
            FixedGridShape shape = new InventoryGridShapes(SlotCapacityRules.Wireframe).Ground;
            _stones = new CountedCollection<ItemDefinition>(shape.SlotCount);
            var stone = new ItemDefinition("stone", "Stone", "a stone", "stones", "A stone.", ItemCategory.StoneAndMinerals, 1.0m);
            _stones.Receive(stone, StoneCount, 0);
            var contents = new CountedCollectionInventoryGridContents<ItemDefinition>(
                _stones, new GridDropOutcomeResolver(new NoThingCombinations()));
            _transfers = new RecordingInventoryTransferService(_stones, new CountedCollection<ItemDefinition>(shape.SlotCount));
            VisualElement gridElement = Element<VisualElement>("ground-pool-grid");
            _grid = new InventoryGridView(
                shape,
                FixedGridHorizontalAlignment.Center,
                contents,
                gridElement,
                tooltip,
                Element<VisualElement>("game-screen-root"),
                skin);

            // The view lays itself out when its element's geometry changes. The element was already laid out before the
            // view existed, so a fixed size is set to make that change happen, as the first layout pass does in the game.
            gridElement.style.width = GridWidthPixels;
            gridElement.style.height = GridHeightPixels;
            simulate.FrameUpdate();
            Assert.That(_grid.CurrentLayout, Is.Not.Null, "The grid never laid itself out, so no slot has a position.");
        }

        // The whole stack is moved by Shift; the same reading applies to Ctrl (half) and Alt (one).
        private void HoldAMoveModifier()
        {
            var moves = new ItemPanelMoveCoordinator(_transfers, new FixedMoveModifierReader(ItemTransferQuantity.All));
            moves.Register(ItemPanel.Ground, _grid);
        }

        [TearDown]
        public void DisposeGrid()
        {
            _grid.Dispose();
            _textures.Dispose();
        }

        private Vector2 FirstSlotCenter => _grid.SlotWorldBound(0).center;

        [Test]
        public void Click_OnAStack_SelectsIt()
        {
            simulate.Click(rootVisualElement.panel.Pick(FirstSlotCenter));
            simulate.FrameUpdate();

            Assert.That(_grid.HasSelection, Is.True);
        }

        [Test]
        public void Click_WithAMoveModifierHeld_SendsTheStackToTheOtherPanel()
        {
            HoldAMoveModifier();

            simulate.Click(rootVisualElement.panel.Pick(FirstSlotCenter));
            simulate.FrameUpdate();

            Assert.That(_transfers.Transfers, Is.EqualTo(new[] { (ItemPanel.Ground, 0, ItemTransferQuantity.All) }));
        }

        [Test]
        public void Drag_WithAMoveModifierHeldLetGoBackOverTheSlotItBeganOn_SendsNothingAndChangesNothing()
        {
            HoldAMoveModifier();

            simulate.DragAndDrop(FirstSlotCenter, FirstSlotCenter + new Vector2(SmallDragPixels, 0));
            simulate.FrameUpdate();

            Assert.That(_transfers.Transfers, Is.Empty);
            Assert.That(_stones.EntryAtSlot(0).Count, Is.EqualTo(StoneCount));
        }

        [Test]
        public void Drag_LetGoBackOverTheSlotItBeganOn_IsOnlyADragSoNothingIsSelected()
        {
            simulate.DragAndDrop(FirstSlotCenter, FirstSlotCenter + new Vector2(SmallDragPixels, 0));
            simulate.FrameUpdate();

            Assert.That(_grid.HasSelection, Is.False);
            Assert.That(_stones.EntryAtSlot(0).Count, Is.EqualTo(StoneCount));
        }

        [Test]
        public void Click_AfterADragLetGoOverItsOwnSlot_StillSelects()
        {
            simulate.DragAndDrop(FirstSlotCenter, FirstSlotCenter + new Vector2(SmallDragPixels, 0));
            simulate.FrameUpdate();

            simulate.Click(rootVisualElement.panel.Pick(FirstSlotCenter));
            simulate.FrameUpdate();

            Assert.That(_grid.HasSelection, Is.True);
        }
    }
}
