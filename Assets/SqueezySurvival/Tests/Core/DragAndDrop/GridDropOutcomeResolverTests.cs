using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Options;
using SqueezySurvival.Core.Tests.Zones;

namespace SqueezySurvival.Core.Tests.DragAndDrop
{
    [TestFixture]
    public sealed class GridDropOutcomeResolverTests
    {
        private static readonly ItemDefinition Shell = new ItemDefinition("shell", "Shell", "a shell", "shells", "A shell.", 0.1m);

        // A vertical grid of 4 columns: pebble in slot 0, twig in 1, shell in 2; slot 3 and everything after is empty.
        private static readonly GridSlotNeighborhood FourColumns = new GridSlotNeighborhood(GridPanelScrollDirection.Vertical, 4);

        private static GridDropOutcome Resolve(int sourceSlot, GridDropTarget target, IThingCombinationRules rules = null)
        {
            var collection = new CountedCollection<ItemDefinition>();
            collection.AddRange(new[] { TestZoneDefinitions.Pebble, TestZoneDefinitions.Twig, Shell });
            return new GridDropOutcomeResolver(rules ?? new NoThingCombinations()).Resolve(collection, FourColumns, sourceSlot, target);
        }

        [Test]
        public void EmptySlot_MovesThere()
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.EmptySlot(6));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Move));
            Assert.That(outcome.TargetSlot, Is.EqualTo(6));
            Assert.That(outcome.Description, Is.EqualTo("Place the pebble here."));
        }

        [Test]
        public void EdgeWithEmptyNeighbor_MovesBesideTheHoveredCell()
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.OccupiedSlot(2, CellDropZone.Right));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Move));
            Assert.That(outcome.TargetSlot, Is.EqualTo(3));
            Assert.That(outcome.Description, Is.EqualTo("Place the pebble right of the shell."));
        }

        [Test]
        public void EdgeWithOccupiedNeighbor_SwapsWithThatNeighbor()
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.OccupiedSlot(1, CellDropZone.Right));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Swap));
            Assert.That(outcome.TargetSlot, Is.EqualTo(2));
            Assert.That(outcome.Description, Is.EqualTo("The pebble and the shell trade places."));
        }

        [Test]
        public void EdgePointingAtTheDraggedThingsOwnSlot_IsNoChange()
        {
            Assert.That(Resolve(0, GridDropTarget.OccupiedSlot(1, CellDropZone.Left)).Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
        }

        [Test]
        public void EdgePastTheGridsSide_IsNoChange()
        {
            GridDropOutcome outcome = Resolve(1, GridDropTarget.OccupiedSlot(0, CellDropZone.Left));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
            Assert.That(outcome.Description, Is.EqualTo("There is no space on that side."));
        }

        [Test]
        public void Nowhere_ReturnsToPlace()
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.Nowhere);

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.ReturnToPlace));
            Assert.That(outcome.ChangesSomething, Is.False);
        }

        [Test]
        public void Center_OfItself_IsNoChange()
        {
            Assert.That(Resolve(1, GridDropTarget.OccupiedSlot(1, CellDropZone.Center)).Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
        }

        [Test]
        public void Center_WithoutRecipe_CannotCombine()
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.OccupiedSlot(1, CellDropZone.Center));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.CannotCombine));
            Assert.That(outcome.Description, Is.EqualTo("The pebble and the twig can't be combined."));
            Assert.That(outcome.ChangesSomething, Is.False);
        }

        [Test]
        public void Center_WithRecipe_Combines()
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.OccupiedSlot(1, CellDropZone.Center), new EverythingCombines());

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Combine));
            Assert.That(outcome.Description, Is.EqualTo("Combine: pebble + twig."));
        }

        private sealed class EverythingCombines : IThingCombinationRules
        {
            public bool TryCombine(ICountableThingDefinition dragged, ICountableThingDefinition target, out string outcomeDescription)
            {
                outcomeDescription = $"Combine: {dragged.Id} + {target.Id}.";
                return true;
            }
        }
    }
}
