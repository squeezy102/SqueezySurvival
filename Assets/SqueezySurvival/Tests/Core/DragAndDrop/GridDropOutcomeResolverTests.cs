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
        public void QuickDropOnAnotherThing_Swaps()
        {
            GridDropOutcome outcome = Resolve(GridDropTarget.OccupiedSlot(0, attemptsCombine: false));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Swap));
            Assert.That(outcome.TargetSlot, Is.EqualTo(0));
            Assert.That(outcome.ChangesSomething, Is.True);
            Assert.That(outcome.Description, Is.EqualTo("The scissors and the rock trade places."));
        }

        [Test]
        public void QuickDropOnItself_IsNoChange()
        {
            Assert.That(Resolve(GridDropTarget.OccupiedSlot(2, attemptsCombine: false)).Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
        }

        [Test]
        public void HeldOverItself_StackOfTwo_CombinesWithItself()
        {
            CountedCollection<ItemDefinition> collection = ThreeThings();
            collection.AddRange(new[] { Scissors });

            GridDropOutcome outcome = new GridDropOutcomeResolver(new EverythingCombines())
                .Resolve(collection, 2, GridDropTarget.OccupiedSlot(2, attemptsCombine: true));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Combine));
            Assert.That(outcome.Description, Is.EqualTo("Combine: scissors + scissors."));
        }

        [Test]
        public void HeldOverItself_SingleThing_CannotCombine()
        {
            GridDropOutcome outcome = Resolve(GridDropTarget.OccupiedSlot(2, attemptsCombine: true), new EverythingCombines());

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.CannotCombine));
            Assert.That(outcome.Description, Is.EqualTo("There is only one scissors. It takes two to combine one with itself."));
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
        public void HeldOverAnotherThing_WithoutRecipe_CannotCombine()
        {
            GridDropOutcome outcome = Resolve(GridDropTarget.OccupiedSlot(1, attemptsCombine: true));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.CannotCombine));
            Assert.That(outcome.Description, Is.EqualTo("The scissors and the paper can't be combined."));
            Assert.That(outcome.ChangesSomething, Is.False);
        }

        [Test]
        public void HeldOverAnotherThing_WithRecipe_Combines()
        {
            GridDropOutcome outcome = Resolve(GridDropTarget.OccupiedSlot(1, attemptsCombine: true), new EverythingCombines());

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
