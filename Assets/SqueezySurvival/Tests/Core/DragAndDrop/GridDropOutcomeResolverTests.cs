using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Tests.Zones;

namespace SqueezySurvival.Core.Tests.DragAndDrop
{
    [TestFixture]
    public sealed class GridDropOutcomeResolverTests
    {
        private static readonly ItemDefinition Shell = new ItemDefinition("shell", "Shell", "a shell", "shells", "A shell.", 0.1m);

        // Pebble, Twig, Shell.
        private static CountedCollection<ItemDefinition> ThreeKinds()
        {
            var collection = new CountedCollection<ItemDefinition>();
            collection.AddRange(new[] { TestZoneDefinitions.Pebble, TestZoneDefinitions.Twig, Shell });
            return collection;
        }

        private static GridDropOutcome Resolve(int source, GridDropTarget target, IThingCombinationRules rules = null) =>
            new GridDropOutcomeResolver(rules ?? new NoThingCombinations()).Resolve(ThreeKinds().Entries, source, target);

        [Test]
        public void LeftEdge_MovesBeforeTarget()
        {
            GridDropOutcome outcome = Resolve(2, GridDropTarget.Cell(0, CellDropZone.Left));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Move));
            Assert.That(outcome.InsertionIndex, Is.EqualTo(0));
            Assert.That(outcome.MarkerSide, Is.EqualTo(InsertionMarkerSide.Before));
            Assert.That(outcome.Description, Is.EqualTo("Place the shell before the pebble."));
            Assert.That(outcome.Title, Is.EqualTo("Move"));
        }

        [Test]
        public void BottomEdge_MovesAfterTarget()
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.Cell(1, CellDropZone.Bottom));

            Assert.That(outcome.InsertionIndex, Is.EqualTo(2));
            Assert.That(outcome.MarkerSide, Is.EqualTo(InsertionMarkerSide.After));
        }

        [Test]
        public void DropThatKeepsPlace_IsNoChange()
        {
            Assert.That(Resolve(1, GridDropTarget.Cell(0, CellDropZone.Right)).Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
            Assert.That(Resolve(1, GridDropTarget.Cell(2, CellDropZone.Left)).Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
            Assert.That(Resolve(1, GridDropTarget.Cell(1, CellDropZone.Center)).Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
        }

        [Test]
        public void EmptyGridSpace_MovesToEnd()
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.EmptyGridSpace);

            Assert.That(outcome.InsertionIndex, Is.EqualTo(3));
            Assert.That(outcome.Description, Is.EqualTo("Place the pebble at the end."));
        }

        [Test]
        public void EmptyGridSpace_WhenAlreadyLast_IsNoChange()
        {
            Assert.That(Resolve(2, GridDropTarget.EmptyGridSpace).Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
        }

        [Test]
        public void Nowhere_ReturnsToPlace()
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.Nowhere);

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.ReturnToPlace));
            Assert.That(outcome.ChangesSomething, Is.False);
        }

        [Test]
        public void Center_WithoutRecipe_CannotCombine()
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.Cell(1, CellDropZone.Center));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.CannotCombine));
            Assert.That(outcome.Description, Is.EqualTo("The pebble and the twig can't be combined."));
            Assert.That(outcome.Title, Is.EqualTo("Can't combine"));
            Assert.That(outcome.ChangesSomething, Is.False);
        }

        [Test]
        public void Center_WithRecipe_Combines()
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.Cell(1, CellDropZone.Center), new EverythingCombines());

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
