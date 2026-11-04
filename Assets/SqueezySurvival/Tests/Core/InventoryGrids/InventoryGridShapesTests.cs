using System;
using NUnit.Framework;
using SqueezySurvival.Core.InventoryGrids;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.InventoryGrids
{
    [TestFixture]
    public sealed class InventoryGridShapesTests
    {
        [Test]
        public void Wireframe_GivesTheShapesInTheDesignDocument()
        {
            // Docs/Design/UI/InventoryGrids.md, The grids.
            var shapes = new InventoryGridShapes(SlotCapacityRules.Wireframe);

            AssertShape(shapes.Ground, 12, 3);
            AssertShape(shapes.ZoneFeatures, 12, 3);
            AssertShape(shapes.Inventory, 8, 1);
            AssertShape(shapes.Hands, 2, 1);
        }

        [Test]
        public void Constructor_RowsFollowTheCapacity()
        {
            var shapes = new InventoryGridShapes(new SlotCapacityRules(survivorInventorySlots: 16, handSlots: 4, groundSlots: 60, zoneFeatureSlots: 12));

            AssertShape(shapes.Inventory, 8, 2);
            AssertShape(shapes.Hands, 2, 2);
            AssertShape(shapes.Ground, 12, 5);
            AssertShape(shapes.ZoneFeatures, 12, 1);
        }

        [TestCase(7, 2, 36, 36)]
        [TestCase(8, 3, 36, 36)]
        [TestCase(8, 2, 37, 36)]
        [TestCase(8, 2, 36, 35)]
        public void Constructor_RejectsACapacityThatDoesNotFillWholeRows(int inventory, int hands, int ground, int features)
        {
            var rules = new SlotCapacityRules(inventory, hands, ground, features);

            Assert.Throws<ArgumentException>(() => new InventoryGridShapes(rules));
        }

        [Test]
        public void ForSlots_OneRowOfOne_IsTheSmallestShape()
        {
            AssertShape(FixedGridShape.ForSlots(1, 1, "Tiny"), 1, 1);
        }

        [TestCase(0, 4)]
        [TestCase(-4, 4)]
        [TestCase(5, 4)]
        public void ForSlots_RejectsSlotsThatDoNotFillWholeRows(int slotCount, int columns)
        {
            Assert.Throws<ArgumentException>(() => FixedGridShape.ForSlots(slotCount, columns, "Test"));
        }

        [Test]
        public void ForSlots_RejectsZeroColumns()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => FixedGridShape.ForSlots(4, 0, "Test"));
        }

        private static void AssertShape(FixedGridShape shape, int columns, int rows)
        {
            Assert.That(shape.Columns, Is.EqualTo(columns));
            Assert.That(shape.Rows, Is.EqualTo(rows));
        }
    }
}
