using System;
using NUnit.Framework;
using SqueezySurvival.Core.EventLog;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Survivor;
using SqueezySurvival.Core.Tests.Items;
using SqueezySurvival.Core.Tests.Zones;
using SqueezySurvival.Core.Time;
using SqueezySurvival.Core.Zones;

namespace SqueezySurvival.Core.Tests.Runs
{
    /// <summary>
    /// Moves between more than two panels (Docs/Design/UI/DragAndDrop.md, Destinations), with a stand-in container
    /// until containers are built.
    /// </summary>
    [TestFixture]
    public sealed class InventoryTransferServiceContainerTests
    {
        private static readonly ItemDefinition Stone = TestItemDefinitions.Stone;

        private static InventoryTransferService CreateTransfers(
            StandInOpenItemContainerSource container,
            out ISurvivorInventoryService survivorInventory,
            out IGroundInventoryService groundInventory,
            out RecordingSurvivorActionCompletionService completion)
        {
            var zone = new Zone(TestZoneDefinitions.Create(4), SlotCapacityRules.Wireframe);
            survivorInventory = new SurvivorInventoryService(SlotCapacityRules.Wireframe);
            groundInventory = new GroundInventoryService(zone, new GameTimeAdvancer(GameClock.RunStart, Array.Empty<ITimeDependentSystem>()), new RunEventLog());
            completion = new RecordingSurvivorActionCompletionService();
            return new InventoryTransferService(
                survivorInventory,
                groundInventory,
                container,
                new ItemMoveDestinationPolicy(),
                completion,
                ItemMoveTimeRules.Wireframe,
                new ItemArrivalStackingSetting());
        }

        private static void FillWithOtherKinds(CountedCollection<ItemDefinition> collection)
        {
            for (int slot = 0; slot < collection.SlotCapacity; slot++)
            {
                collection.Receive(new ItemDefinition($"filler-{slot}", "Filler", "a filler", "fillers", "Fills a slot.", ItemCategory.Salvage, 0.1m), 1, slot);
            }
        }

        [Test]
        public void Send_FromTheGroundWithAContainerOpen_PutsItInTheContainerNotTheInventory()
        {
            var container = new StandInOpenItemContainerSource();
            InventoryTransferService transfers = CreateTransfers(container, out ISurvivorInventoryService survivor, out IGroundInventoryService ground, out RecordingSurvivorActionCompletionService completion);
            ground.Ground.Receive(Stone, 5, 0);

            transfers.Send(ItemPanel.Ground, 0, ItemTransferQuantity.All);

            Assert.That(container.Contents.CountOf(Stone), Is.EqualTo(5));
            Assert.That(survivor.Inventory.CountOf(Stone), Is.EqualTo(0));
            Assert.That(ground.Ground.CountOf(Stone), Is.EqualTo(0));
            Assert.That(completion.Completed[0].MessageTexts, Is.EqualTo(new[] { "You put 5 stones in the woven palm basket." }));
        }

        [Test]
        public void Send_FromTheInventoryWithAContainerOpen_PutsItInTheContainerNotOnTheGround()
        {
            var container = new StandInOpenItemContainerSource();
            InventoryTransferService transfers = CreateTransfers(container, out ISurvivorInventoryService survivor, out IGroundInventoryService ground, out _);
            survivor.Inventory.Receive(Stone, 3, 0);

            transfers.Send(ItemPanel.Inventory, 0, ItemTransferQuantity.All);

            Assert.That(container.Contents.CountOf(Stone), Is.EqualTo(3));
            Assert.That(ground.Ground.CountOf(Stone), Is.EqualTo(0));
        }

        [Test]
        public void Send_FromTheContainer_GoesToTheInventory()
        {
            var container = new StandInOpenItemContainerSource();
            InventoryTransferService transfers = CreateTransfers(container, out ISurvivorInventoryService survivor, out IGroundInventoryService ground, out RecordingSurvivorActionCompletionService completion);
            container.Contents.Receive(Stone, 5, 0);

            transfers.Send(container.Panel, 0, ItemTransferQuantity.All);

            Assert.That(survivor.Inventory.CountOf(Stone), Is.EqualTo(5));
            Assert.That(ground.Ground.CountOf(Stone), Is.EqualTo(0));
            Assert.That(container.Contents.CountOf(Stone), Is.EqualTo(0));
            Assert.That(completion.Completed[0].MessageTexts, Is.EqualTo(new[] { "You picked up 5 stones." }));
        }

        [Test]
        public void Send_FromTheContainerWhenTheInventoryIsFull_GoesToTheGround()
        {
            var container = new StandInOpenItemContainerSource();
            InventoryTransferService transfers = CreateTransfers(container, out ISurvivorInventoryService survivor, out IGroundInventoryService ground, out RecordingSurvivorActionCompletionService completion);
            FillWithOtherKinds(survivor.Inventory);
            container.Contents.Receive(Stone, 5, 0);

            transfers.Send(container.Panel, 0, ItemTransferQuantity.All);

            Assert.That(ground.Ground.CountOf(Stone), Is.EqualTo(5));
            Assert.That(survivor.Inventory.CountOf(Stone), Is.EqualTo(0));
            Assert.That(completion.Completed[0].MessageTexts, Is.EqualTo(new[] { "You put 5 stones on the ground." }));
        }

        [Test]
        public void PreviewSend_FromTheContainerWithNoRoomAnywhere_ReportsNoRoomInTheInventoryAndSendRefuses()
        {
            var container = new StandInOpenItemContainerSource();
            InventoryTransferService transfers = CreateTransfers(container, out ISurvivorInventoryService survivor, out IGroundInventoryService ground, out _);
            FillWithOtherKinds(survivor.Inventory);
            FillWithOtherKinds(ground.Ground);
            container.Contents.Receive(Stone, 5, 0);

            ItemMovePreview preview = transfers.PreviewSend(container.Panel, 0, ItemTransferQuantity.All);

            Assert.That(preview.CanMove, Is.False);
            Assert.That(preview.To, Is.EqualTo(ItemPanel.Inventory));
            Assert.That(() => transfers.Send(container.Panel, 0, ItemTransferQuantity.All), Throws.InvalidOperationException);
            Assert.That(container.Contents.CountOf(Stone), Is.EqualTo(5));
        }

        [Test]
        public void PreviewSend_FromTheGroundToAFullContainer_ReportsNoRoomInItAndDoesNotFallBack()
        {
            var container = new StandInOpenItemContainerSource();
            InventoryTransferService transfers = CreateTransfers(container, out ISurvivorInventoryService survivor, out IGroundInventoryService ground, out RecordingSurvivorActionCompletionService completion);
            FillWithOtherKinds(container.Contents);
            ground.Ground.Receive(Stone, 5, 0);

            ItemMovePreview preview = transfers.PreviewSend(ItemPanel.Ground, 0, ItemTransferQuantity.All);

            Assert.That(preview.CanMove, Is.False);
            Assert.That(preview.To, Is.EqualTo(container.Panel));
            Assert.That(ItemMoveTextFormatter.FormatDescription(preview), Is.EqualTo("There's no free slot in the woven palm basket."));
            Assert.That(() => transfers.Send(ItemPanel.Ground, 0, ItemTransferQuantity.All), Throws.InvalidOperationException);
            Assert.That(ground.Ground.CountOf(Stone), Is.EqualTo(5));
            Assert.That(survivor.Inventory.CountOf(Stone), Is.EqualTo(0));
            Assert.That(completion.Completed, Is.Empty);
        }

        [Test]
        public void Send_WithNoContainerOpen_GoesToTheOtherOfTheGroundAndTheInventory()
        {
            var container = new StandInOpenItemContainerSource { IsOpen = false };
            InventoryTransferService transfers = CreateTransfers(container, out ISurvivorInventoryService survivor, out IGroundInventoryService ground, out _);
            ground.Ground.Receive(Stone, 5, 0);

            transfers.Send(ItemPanel.Ground, 0, ItemTransferQuantity.All);
            transfers.Send(ItemPanel.Inventory, 0, ItemTransferQuantity.One);

            Assert.That(survivor.Inventory.CountOf(Stone), Is.EqualTo(4));
            Assert.That(ground.Ground.CountOf(Stone), Is.EqualTo(1));
        }

        [Test]
        public void Transfer_ToTheContainerLettingGoOverASlot_LandsInThatSlot()
        {
            var container = new StandInOpenItemContainerSource();
            InventoryTransferService transfers = CreateTransfers(container, out _, out IGroundInventoryService ground, out _);
            ground.Ground.Receive(Stone, 5, 0);

            transfers.Transfer(ItemPanel.Ground, container.Panel, 0, 3, ItemTransferQuantity.All);

            Assert.That(container.Contents.EntryAtSlot(3).Count, Is.EqualTo(5));
        }

        [Test]
        public void Transfer_FromTheContainerToTheGround_NeedsNoPolicy()
        {
            var container = new StandInOpenItemContainerSource();
            InventoryTransferService transfers = CreateTransfers(container, out _, out IGroundInventoryService ground, out _);
            container.Contents.Receive(Stone, 2, 0);

            transfers.Transfer(container.Panel, ItemPanel.Ground, 0, -1, ItemTransferQuantity.All);

            Assert.That(ground.Ground.CountOf(Stone), Is.EqualTo(2));
        }

        [Test]
        public void Transfer_ToAContainerThatIsNotOpen_Throws()
        {
            var container = new StandInOpenItemContainerSource { IsOpen = false };
            InventoryTransferService transfers = CreateTransfers(container, out _, out IGroundInventoryService ground, out _);
            ground.Ground.Receive(Stone, 5, 0);

            Assert.That(
                () => transfers.Transfer(ItemPanel.Ground, container.Panel, 0, -1, ItemTransferQuantity.All),
                Throws.TypeOf<ArgumentException>());
            Assert.That(ground.Ground.CountOf(Stone), Is.EqualTo(5));
        }

        [Test]
        public void Transfer_FromAPanelToItself_Throws()
        {
            var container = new StandInOpenItemContainerSource();
            InventoryTransferService transfers = CreateTransfers(container, out _, out IGroundInventoryService ground, out _);
            ground.Ground.Receive(Stone, 5, 0);

            Assert.That(
                () => transfers.Transfer(ItemPanel.Ground, ItemPanel.Ground, 0, -1, ItemTransferQuantity.All),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ContentsOf_TheOpenContainer_IsItsContents()
        {
            var container = new StandInOpenItemContainerSource();
            InventoryTransferService transfers = CreateTransfers(container, out _, out _, out _);

            Assert.That(transfers.ContentsOf(container.Panel), Is.SameAs(container.Contents));
        }

        [Test]
        public void ContentsOf_AContainerThatIsNotOpen_Throws()
        {
            var container = new StandInOpenItemContainerSource { IsOpen = false };
            InventoryTransferService transfers = CreateTransfers(container, out _, out _, out _);

            Assert.That(() => transfers.ContentsOf(container.Panel), Throws.TypeOf<ArgumentException>());
        }
    }
}
