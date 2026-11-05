using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.DragAndDrop
{
    [TestFixture]
    public sealed class DropOutcomeMemoTests
    {
        private static DropOutcomeKey OwnGridKey(
            int sourceSlot = 1, GridDropTarget target = null, ItemTransferQuantity quantity = ItemTransferQuantity.All, long revision = 0) =>
            DropOutcomeKey.ForOwnGrid(sourceSlot, target ?? GridDropTarget.EmptySlot(3), quantity, revision);

        [Test]
        public void TryGet_NothingStored_FindsNothing()
        {
            var memo = new DropOutcomeMemo();

            Assert.That(memo.TryGet(OwnGridKey(), out _), Is.False);
        }

        [Test]
        public void TryGet_SameKeyAsStored_ReturnsTheStoredOutcome()
        {
            var memo = new DropOutcomeMemo();
            memo.Store(OwnGridKey(), GridDropOutcome.ReturnToPlace);

            Assert.That(memo.TryGet(OwnGridKey(), out GridDropOutcome outcome), Is.True);
            Assert.That(outcome, Is.SameAs(GridDropOutcome.ReturnToPlace));
        }

        [TestCaseSource(nameof(KeysThatDifferFromTheBaseKey))]
        public void TryGet_AnyPartOfTheKeyDiffers_FindsNothing(DropOutcomeKey differingKey)
        {
            var memo = new DropOutcomeMemo();
            memo.Store(OwnGridKey(), GridDropOutcome.ReturnToPlace);

            Assert.That(memo.TryGet(differingKey, out _), Is.False);
        }

        private static System.Collections.Generic.IEnumerable<TestCaseData> KeysThatDifferFromTheBaseKey()
        {
            yield return new TestCaseData(OwnGridKey(sourceSlot: 2)).SetName("DifferentSourceSlot");
            yield return new TestCaseData(OwnGridKey(target: GridDropTarget.EmptySlot(4))).SetName("DifferentTargetSlot");
            yield return new TestCaseData(OwnGridKey(target: GridDropTarget.OccupiedSlot(3, attemptsCombine: false))).SetName("DifferentTargetKind");
            yield return new TestCaseData(OwnGridKey(target: GridDropTarget.Nowhere)).SetName("NoTarget");
            yield return new TestCaseData(OwnGridKey(quantity: ItemTransferQuantity.Half)).SetName("DifferentQuantity");
            yield return new TestCaseData(OwnGridKey(revision: 1)).SetName("ContentsChanged");
            yield return new TestCaseData(DropOutcomeKey.ForOtherPanel(1, ItemPanel.Inventory, 3, ItemTransferQuantity.All, true, 0)).SetName("OverTheOtherPanel");
        }

        [Test]
        public void TryGet_OccupiedSlotWithAndWithoutCombineAttempt_AreDifferentKeys()
        {
            var memo = new DropOutcomeMemo();
            memo.Store(OwnGridKey(target: GridDropTarget.OccupiedSlot(3, attemptsCombine: false)), GridDropOutcome.ReturnToPlace);

            Assert.That(memo.TryGet(OwnGridKey(target: GridDropTarget.OccupiedSlot(3, attemptsCombine: true)), out _), Is.False);
        }

        [Test]
        public void TryGet_OtherPanelCanTransferChanged_FindsNothing()
        {
            var memo = new DropOutcomeMemo();
            memo.Store(DropOutcomeKey.ForOtherPanel(1, ItemPanel.Inventory, 3, ItemTransferQuantity.All, true, 5), GridDropOutcome.ReturnToPlace);

            Assert.That(memo.TryGet(DropOutcomeKey.ForOtherPanel(1, ItemPanel.Inventory, 3, ItemTransferQuantity.All, false, 5), out _), Is.False);
            Assert.That(memo.TryGet(DropOutcomeKey.ForOtherPanel(1, ItemPanel.Inventory, 3, ItemTransferQuantity.All, true, 5), out _), Is.True);
        }

        [Test]
        public void Store_ForgetsTheEarlierOutcome()
        {
            var memo = new DropOutcomeMemo();
            memo.Store(OwnGridKey(revision: 0), GridDropOutcome.ReturnToPlace);
            memo.Store(OwnGridKey(revision: 1), GridDropOutcome.ReturnToPlace);

            Assert.That(memo.TryGet(OwnGridKey(revision: 0), out _), Is.False);
            Assert.That(memo.TryGet(OwnGridKey(revision: 1), out _), Is.True);
        }

        [Test]
        public void Clear_ForgetsTheOutcome()
        {
            var memo = new DropOutcomeMemo();
            memo.Store(OwnGridKey(), GridDropOutcome.ReturnToPlace);

            memo.Clear();

            Assert.That(memo.TryGet(OwnGridKey(), out _), Is.False);
        }

        [Test]
        public void ForOwnGrid_NoTarget_IsRefused()
        {
            Assert.That(() => DropOutcomeKey.ForOwnGrid(1, null, ItemTransferQuantity.All, 0), Throws.ArgumentNullException);
        }

        [Test]
        public void ForOtherPanel_DifferentTargetPanels_AreDifferentKeys()
        {
            var memo = new DropOutcomeMemo();
            memo.Store(DropOutcomeKey.ForOtherPanel(1, ItemPanel.Inventory, 3, ItemTransferQuantity.All, true, 5), GridDropOutcome.ReturnToPlace);

            Assert.That(memo.TryGet(DropOutcomeKey.ForOtherPanel(1, ItemPanel.ForContainer("basket"), 3, ItemTransferQuantity.All, true, 5), out _), Is.False);
            Assert.That(memo.TryGet(DropOutcomeKey.ForOtherPanel(1, ItemPanel.Inventory, 3, ItemTransferQuantity.All, true, 5), out _), Is.True);
        }
    }
}
