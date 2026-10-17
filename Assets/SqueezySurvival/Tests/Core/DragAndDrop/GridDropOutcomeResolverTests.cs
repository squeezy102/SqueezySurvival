using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Tests.Zones;

namespace SqueezySurvival.Core.Tests.DragAndDrop
{
    [TestFixture]
    public sealed class GridDropOutcomeResolverTests
    {
        // Pebble in slot 0, twig in slot 1; every other slot is empty.
        private static GridDropOutcome Resolve(int sourceSlot, GridDropTarget target, IThingCombinationRules rules = null)
        {
            var collection = new CountedCollection<ItemDefinition>();
            collection.AddRange(new[] { TestZoneDefinitions.Pebble, TestZoneDefinitions.Twig });
            return new GridDropOutcomeResolver(rules ?? new NoThingCombinations()).Resolve(collection, sourceSlot, target);
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
        public void EdgeOfAnotherThing_SwapsWithThatThing()
        {
            GridDropOutcome outcome = Resolve(0, GridDropTarget.OccupiedSlot(1, CellDropZone.Top));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Swap));
            Assert.That(outcome.TargetSlot, Is.EqualTo(1));
            Assert.That(outcome.Description, Is.EqualTo("The pebble and the twig trade places."));
        }

        [Test]
        public void OverItself_IsNoChange()
        {
            GridDropOutcome outcome = Resolve(1, GridDropTarget.OccupiedSlot(1, CellDropZone.Left));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
            Assert.That(outcome.ChangesSomething, Is.False);
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
