using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Items
{
    /// <summary>
    /// Which slots <see cref="CountedCollection{TDefinition}.Changed"/> reports for each command
    /// (Docs/Engineering/Architecture.md, Commands, state and change events). Each recorded change is the list of slots
    /// one event named; one inner list per event raised.
    /// </summary>
    [TestFixture]
    public sealed class CountedCollectionChangedEventTests
    {
        private static readonly ItemDefinition Stone = PlaceholderBeachZoneContent.Stone;
        private static readonly ItemDefinition Stick = PlaceholderBeachZoneContent.SmallStick;
        private static readonly ItemDefinition Shell = PlaceholderBeachZoneContent.Seashell;
        private static readonly ItemDefinition Conch = PlaceholderBeachZoneContent.Conch;
        private static readonly ItemDefinition Coconut = PlaceholderBeachZoneContent.Coconut;

        private static CountedCollection<ItemDefinition> StoneStickShell(int slotCapacity = 8)
        {
            var collection = new CountedCollection<ItemDefinition>(slotCapacity);
            collection.AddRange(new[] { Stone, Stick, Shell });
            return collection;
        }

        private static List<int[]> RecordChanges(CountedCollection<ItemDefinition> collection)
        {
            var changes = new List<int[]>();
            collection.Changed += (_, change) => changes.Add(change.ChangedSlots.ToArray());
            return changes;
        }

        [Test]
        public void Receive_KindAlreadyPresent_ReportsOnlyItsSlot()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();
            List<int[]> changes = RecordChanges(collection);

            collection.Receive(Stone, 2, 5);

            Assert.That(changes, Is.EqualTo(new[] { new[] { 0 } }));
        }

        [Test]
        public void Take_PartOfAStack_ReportsItsSlot()
        {
            var collection = new CountedCollection<ItemDefinition>(4);
            collection.Receive(Stone, 3, 2);
            List<int[]> changes = RecordChanges(collection);

            collection.Take(2, 1);

            Assert.That(changes, Is.EqualTo(new[] { new[] { 2 } }));
        }

        [Test]
        public void AddRange_SeveralSlots_ReportsThemOnceInSlotOrder()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();
            collection.MoveToSlot(0, 5);
            List<int[]> changes = RecordChanges(collection);

            collection.AddRange(new[] { Shell, Coconut, Shell });

            Assert.That(changes, Is.EqualTo(new[] { new[] { 0, 2 } }));
        }

        [Test]
        public void AddRange_WithNothing_RaisesNothing()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();
            List<int[]> changes = RecordChanges(collection);

            collection.AddRange(new ItemDefinition[0]);

            Assert.That(changes, Is.Empty);
        }

        [Test]
        public void AddRange_OnlyOverflow_RaisesNothing()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell(slotCapacity: 3);
            List<int[]> changes = RecordChanges(collection);

            collection.AddRange(new[] { Conch });

            Assert.That(changes, Is.Empty);
        }

        [Test]
        public void MoveToSlot_EmptySlot_ReportsTheSlotItLeftAndTheSlotItFilled()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();
            List<int[]> changes = RecordChanges(collection);

            collection.MoveToSlot(2, 6);

            Assert.That(changes, Is.EqualTo(new[] { new[] { 2, 6 } }));
        }

        [Test]
        public void MoveToSlot_SwapOfDifferentKinds_ReportsBothSlots()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();
            List<int[]> changes = RecordChanges(collection);

            collection.MoveToSlot(2, 0);

            Assert.That(changes, Is.EqualTo(new[] { new[] { 0, 2 } }));
        }

        [Test]
        public void MoveToSlot_SwapOfEqualStacks_RaisesNothing()
        {
            var collection = new CountedCollection<ItemDefinition>(4);
            collection.Receive(Stone, 6, 0);
            collection.SplitToSlot(0, 3, 3);
            List<int[]> changes = RecordChanges(collection);

            collection.MoveToSlot(0, 3);

            Assert.That(changes, Is.Empty);
        }

        [Test]
        public void MoveToSlot_SameSlot_RaisesNothing()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();
            List<int[]> changes = RecordChanges(collection);

            collection.MoveToSlot(1, 1);

            Assert.That(changes, Is.Empty);
        }

        [Test]
        public void SplitToSlot_PartOfAStack_ReportsBothSlots()
        {
            var collection = new CountedCollection<ItemDefinition>(4);
            collection.Receive(Stone, 7, 0);
            List<int[]> changes = RecordChanges(collection);

            collection.SplitToSlot(0, 3, 4);

            Assert.That(changes, Is.EqualTo(new[] { new[] { 0, 3 } }));
        }

        [Test]
        public void SplitToSlot_AllOfAStack_ReportsBothSlotsInOneChange()
        {
            var collection = new CountedCollection<ItemDefinition>(4);
            collection.Receive(Stone, 7, 0);
            List<int[]> changes = RecordChanges(collection);

            collection.SplitToSlot(0, 3, 7);

            Assert.That(changes, Is.EqualTo(new[] { new[] { 0, 3 } }));
        }

        [Test]
        public void MergeIntoSlot_ReportsBothSlots()
        {
            var collection = new CountedCollection<ItemDefinition>(4);
            collection.Receive(Stone, 7, 0);
            collection.SplitToSlot(0, 3, 4);
            List<int[]> changes = RecordChanges(collection);

            collection.MergeIntoSlot(3, 0, 2);

            Assert.That(changes, Is.EqualTo(new[] { new[] { 0, 3 } }));
        }

        [Test]
        public void ReplaceSlot_ReportsOnlyThatSlot()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();
            List<int[]> changes = RecordChanges(collection);

            collection.ReplaceSlot(1, new CountedThing<ItemDefinition>(Conch, 1));

            Assert.That(changes, Is.EqualTo(new[] { new[] { 1 } }));
        }

        [Test]
        public void Sort_ReportsOnlyTheSlotsWhoseContentsChanged()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();
            collection.MoveToSlot(0, 6);
            List<int[]> changes = RecordChanges(collection);

            collection.Sort(new ThingSortComparer<ItemDefinition>(ThingSortOrder.Alphabetical));

            // Seashell moves 2 -> 0 and stone 6 -> 2; the small stick was already in slot 1.
            Assert.That(changes, Is.EqualTo(new[] { new[] { 0, 2, 6 } }));
        }

        [Test]
        public void Sort_AlreadySorted_RaisesNothing()
        {
            CountedCollection<ItemDefinition> collection = StoneStickShell();
            collection.Sort(new ThingSortComparer<ItemDefinition>(ThingSortOrder.Alphabetical));
            List<int[]> changes = RecordChanges(collection);

            collection.Sort(new ThingSortComparer<ItemDefinition>(ThingSortOrder.Alphabetical));

            Assert.That(changes, Is.Empty);
        }
    }
}
