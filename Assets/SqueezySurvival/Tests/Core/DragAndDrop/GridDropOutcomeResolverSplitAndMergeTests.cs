using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.DragAndDrop
{
    [TestFixture]
    public sealed class GridDropOutcomeResolverSplitAndMergeTests
    {
        private static readonly ItemDefinition Stone = PlaceholderBeachZoneContent.Stone;
        private static readonly ItemDefinition Stick = PlaceholderBeachZoneContent.SmallStick;

        // 7 stones in slot 0, 5 stones in slot 1, a stick in slot 2; slot 3 empty.
        private static CountedCollection<ItemDefinition> TwoStoneStacksAndAStick()
        {
            var collection = new CountedCollection<ItemDefinition>(4);
            collection.Receive(Stone, 12, 0);
            collection.SplitToSlot(0, 1, 5);
            collection.Receive(Stick, 1, 2);
            return collection;
        }

        private static GridDropOutcome Resolve(int sourceSlot, GridDropTarget target, ItemTransferQuantity quantity) =>
            new GridDropOutcomeResolver(new NoThingCombinations()).Resolve(TwoStoneStacksAndAStick(), sourceSlot, target, quantity);

        [TestCase(ItemTransferQuantity.Half, 4, "Put 4 of the 7 stones here.")]
        [TestCase(ItemTransferQuantity.One, 1, "Put one of the 7 stones here.")]
        public void PartOfAStackOnAnEmptySlot_Splits(ItemTransferQuantity quantity, int expectedCount, string expectedDescription)
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.EmptySlot(3), quantity);

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Split));
            Assert.That(outcome.Title, Is.EqualTo("Split"));
            Assert.That(outcome.TargetSlot, Is.EqualTo(3));
            Assert.That(outcome.Count, Is.EqualTo(expectedCount));
            Assert.That(outcome.Description, Is.EqualTo(expectedDescription));
            Assert.That(outcome.ChangesSomething, Is.True);
        }

        [Test]
        public void WholeStackOnAnEmptySlot_StillMoves()
        {
            Assert.That(Resolve(0, GridDropTarget.EmptySlot(3), ItemTransferQuantity.All).Kind, Is.EqualTo(GridDropOutcomeKind.Move));
        }

        [Test]
        public void OneOfASingleThingOnAnEmptySlot_MovesRatherThanSplits()
        {
            Assert.That(Resolve(2, GridDropTarget.EmptySlot(3), ItemTransferQuantity.One).Kind, Is.EqualTo(GridDropOutcomeKind.Move));
        }

        [Test]
        public void WholeStackOnTheSameKind_Merges()
        {
            GridDropOutcome outcome = Resolve(1, GridDropTarget.OccupiedSlot(0, attemptsCombine: false), ItemTransferQuantity.All);

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Merge));
            Assert.That(outcome.Title, Is.EqualTo("Stack"));
            Assert.That(outcome.Count, Is.EqualTo(5));
            Assert.That(outcome.Description, Is.EqualTo("Add the 5 stones to the stack of 7 here."));
        }

        [Test]
        public void OneOnTheSameKind_MergesOne()
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.OccupiedSlot(1, attemptsCombine: false), ItemTransferQuantity.One);

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Merge));
            Assert.That(outcome.Count, Is.EqualTo(1));
            Assert.That(outcome.Description, Is.EqualTo("Add one of the 7 stones to the stack of 5 here."));
        }

        [Test]
        public void SameKindAfterTheHold_AttemptsToCombineInstead()
        {
            GridDropOutcome outcome = Resolve(1, GridDropTarget.OccupiedSlot(0, attemptsCombine: true), ItemTransferQuantity.All);

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.CannotCombine));
        }

        [Test]
        public void PartOfAStackOnAnotherKind_ChangesNothingAndSaysWhere()
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.OccupiedSlot(2, attemptsCombine: false), ItemTransferQuantity.Half);

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
            Assert.That(outcome.ChangesSomething, Is.False);
            Assert.That(outcome.Description, Is.EqualTo("Split onto an empty slot or another stack of stones."));
        }

        [Test]
        public void WholeStackOnAnotherKind_StillSwaps()
        {
            Assert.That(Resolve(0, GridDropTarget.OccupiedSlot(2, attemptsCombine: false), ItemTransferQuantity.All).Kind, Is.EqualTo(GridDropOutcomeKind.Swap));
        }

        [Test]
        public void PartOfAStackOnItself_IsNoChange()
        {
            Assert.That(Resolve(0, GridDropTarget.OccupiedSlot(0, attemptsCombine: false), ItemTransferQuantity.Half).Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
        }
    }
}
