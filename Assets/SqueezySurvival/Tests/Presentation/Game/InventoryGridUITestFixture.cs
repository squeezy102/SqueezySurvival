using System.Collections.Generic;
using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.InventoryGrids;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Presentation.Game;
using SqueezySurvival.Presentation.PixelArt;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Game
{
    /// <summary>
    /// A game screen test panel that can build inventory grids over its grid elements, the way the composition root
    /// does, and disposes them after each test.
    /// </summary>
    public abstract class InventoryGridUITestFixture : GameScreenUITestFixture
    {
        // The view lays itself out when its element's geometry changes, so tests give each grid element a fixed size.
        private const float GridWidthPixels = 612f;
        private const float GridHeightPixels = 204f;

        private readonly List<InventoryGridView> _grids = new List<InventoryGridView>();
        private PixelArtInterfaceSkin _skin;

        /// <summary>The tooltip shared by the grids.</summary>
        protected GridTooltipView Tooltip { get; private set; }

        private PixelArtTextureCache Textures { get; set; }

        /// <summary>Creates what every grid needs.</summary>
        [SetUp]
        public void CreateGridDependencies()
        {
            Textures = new PixelArtTextureCache();
            _skin = new PixelArtInterfaceSkin(Textures);
            Tooltip = new GridTooltipView(Element<VisualElement>("grid-tooltip"), Textures);
        }

        /// <summary>Disposes the grids built during the test.</summary>
        [TearDown]
        public void DisposeGrids()
        {
            foreach (InventoryGridView grid in _grids)
            {
                grid.Dispose();
            }

            _grids.Clear();
            Textures.Dispose();
        }

        /// <summary>A grid over the layout's element named <paramref name="gridName"/>, showing <paramref name="things"/>.</summary>
        protected InventoryGridView CreateGrid(FixedGridShape shape, string gridName, CountedCollection<ItemDefinition> things)
        {
            var contents = new CountedCollectionInventoryGridContents<ItemDefinition>(
                things, new GridDropOutcomeResolver(new NoThingCombinations()));
            VisualElement gridElement = Element<VisualElement>(gridName);
            var grid = new InventoryGridView(
                shape,
                FixedGridHorizontalAlignment.Center,
                contents,
                gridElement,
                Tooltip,
                Element<VisualElement>("game-screen-root"),
                _skin);
            _grids.Add(grid);

            gridElement.style.width = GridWidthPixels;
            gridElement.style.height = GridHeightPixels;
            simulate.FrameUpdate();
            Assert.That(grid.CurrentLayout, Is.Not.Null, $"The {gridName} never laid itself out, so no slot has a position.");
            return grid;
        }
    }
}
