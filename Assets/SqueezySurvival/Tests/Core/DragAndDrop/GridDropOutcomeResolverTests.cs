using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.DragAndDrop
{
    [TestFixture]
    public sealed class GridDropOutcomeResolverTests
    {
        private static readonly ItemDefinition Rock = Item("rock");
        private static readonly ItemDefinition Paper = Item("paper");
        private static readonly ItemDefinition Scissors = Item("scissors");

        private static ItemDefinition Item(string id) =>
            new ItemDefinition(id, char.ToUpperInvariant(id[0]) + id.Substring(1), "a " + id, id + "s", "A " + id + ".", ItemCategory.Salvage, 1m);

        // Rock, paper, scissors in slots 0-2 of 8.
        private static CountedCollection<ItemDefinition> ThreeThings()
        {
            var collection = new CountedCollection<ItemDefinition>(8);
            collection.AddRange(new[] { Rock, Paper, Scissors });
            return collection;
        }

        private static GridDropOutcome Resolve(GridDropTarget target, IThingCombinationRules rules = null) =>
            new GridDropOutcomeResolver(rules ?? new NoThingCombinations()).Resolve(ThreeThings(), 2, target);

        [Test]
        public void EdgeOfAnotherThing_Swaps()
        {
            GridDropOutcome outcome = Resolve(GridDropTarget.OccupiedSlot(0, CellDropZone.Edge));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Swap));
            Assert.That(outcome.TargetSlot, Is.EqualTo(0));
            Assert.That(outcome.ChangesSomething, Is.True);
            Assert.That(outcome.Description, Is.EqualTo("The scissors and the rock trade places."));
        }

        [TestCase(CellDropZone.Edge)]
        [TestCase(CellDropZone.Center)]
        public void OverItself_IsNoChange(CellDropZone zone)
        {
            Assert.That(Resolve(GridDropTarget.OccupiedSlot(2, zone)).Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
        }

        [Test]
        public void EmptySlot_MovesThere()
        {
            GridDropOutcome outcome = Resolve(GridDropTarget.EmptySlot(6));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Move));
            Assert.That(outcome.TargetSlot, Is.EqualTo(6));
            Assert.That(outcome.Description, Is.EqualTo("Put the scissors here."));
        }

        [Test]
        public void Nowhere_ReturnsToPlace()
        {
            Assert.That(Resolve(GridDropTarget.Nowhere).ChangesSomething, Is.False);
        }

        [Test]
        public void Center_WithoutRecipe_CannotCombine()
        {
            GridDropOutcome outcome = Resolve(GridDropTarget.OccupiedSlot(1, CellDropZone.Center));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.CannotCombine));
            Assert.That(outcome.Description, Is.EqualTo("The scissors and the paper can't be combined."));
            Assert.That(outcome.ChangesSomething, Is.False);
        }

        [Test]
        public void Center_WithRecipe_Combines()
        {
            GridDropOutcome outcome = Resolve(GridDropTarget.OccupiedSlot(1, CellDropZone.Center), new EverythingCombines());

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Combine));
            Assert.That(outcome.Description, Is.EqualTo("Combine: scissors + paper."));
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
