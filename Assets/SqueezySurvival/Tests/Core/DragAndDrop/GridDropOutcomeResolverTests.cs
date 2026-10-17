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
        private static readonly ItemDefinition Lizard = Item("lizard");
        private static readonly ItemDefinition Spock = Item("spock");

        private static ItemDefinition Item(string id) => new ItemDefinition(id, char.ToUpperInvariant(id[0]) + id.Substring(1), "a " + id, id + "s", "A " + id + ".", 1m);

        // Rock, paper, scissors, lizard, spock in slots 0-4.
        private static CountedCollection<ItemDefinition> FiveThings()
        {
            var collection = new CountedCollection<ItemDefinition>();
            collection.AddRange(new[] { Rock, Paper, Scissors, Lizard, Spock });
            return collection;
        }

        private static GridDropOutcome Resolve(CountedCollection<ItemDefinition> collection, int sourceSlot, GridDropTarget target, IThingCombinationRules rules = null) =>
            new GridDropOutcomeResolver(rules ?? new NoThingCombinations()).Resolve(collection, sourceSlot, target);

        [Test]
        public void LeftSide_PutsBeforeTheHoveredThing()
        {
            GridDropOutcome outcome = Resolve(FiveThings(), 3, GridDropTarget.OccupiedSlot(1, CellDropZone.Left));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Move));
            Assert.That(outcome.TargetSlot, Is.EqualTo(1));
            Assert.That(outcome.MarkerSide, Is.EqualTo(DropMarkerSide.Before));
            Assert.That(outcome.Description, Is.EqualTo("Put the lizard before the paper."));
        }

        [Test]
        public void LeftSide_WithGapBefore_UsesTheGap()
        {
            CountedCollection<ItemDefinition> collection = FiveThings();
            collection.MoveToSlot(1, 7);

            Assert.That(Resolve(collection, 4, GridDropTarget.OccupiedSlot(2, CellDropZone.Left)).TargetSlot, Is.EqualTo(1));
        }

        [Test]
        public void RightSide_PutsAfterTheHoveredThing()
        {
            GridDropOutcome outcome = Resolve(FiveThings(), 0, GridDropTarget.OccupiedSlot(2, CellDropZone.Right));

            Assert.That(outcome.TargetSlot, Is.EqualTo(3));
            Assert.That(outcome.MarkerSide, Is.EqualTo(DropMarkerSide.After));
        }

        [Test]
        public void BesideItsOwnSlot_IsNoChange()
        {
            Assert.That(Resolve(FiveThings(), 3, GridDropTarget.OccupiedSlot(4, CellDropZone.Left)).Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
            Assert.That(Resolve(FiveThings(), 3, GridDropTarget.OccupiedSlot(2, CellDropZone.Right)).Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
            Assert.That(Resolve(FiveThings(), 3, GridDropTarget.OccupiedSlot(3, CellDropZone.Left)).Kind, Is.EqualTo(GridDropOutcomeKind.NoChange));
        }

        [Test]
        public void EmptySlot_MovesThere()
        {
            GridDropOutcome outcome = Resolve(FiveThings(), 0, GridDropTarget.EmptySlot(6));

            Assert.That(outcome.TargetSlot, Is.EqualTo(6));
            Assert.That(outcome.Description, Is.EqualTo("Put the rock here."));
        }

        [Test]
        public void Nowhere_ReturnsToPlace()
        {
            Assert.That(Resolve(FiveThings(), 0, GridDropTarget.Nowhere).ChangesSomething, Is.False);
        }

        [Test]
        public void Center_WithoutRecipe_CannotCombine()
        {
            GridDropOutcome outcome = Resolve(FiveThings(), 0, GridDropTarget.OccupiedSlot(1, CellDropZone.Center));

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.CannotCombine));
            Assert.That(outcome.Description, Is.EqualTo("The rock and the paper can't be combined."));
            Assert.That(outcome.ChangesSomething, Is.False);
        }

        [Test]
        public void Center_WithRecipe_Combines()
        {
            GridDropOutcome outcome = Resolve(FiveThings(), 0, GridDropTarget.OccupiedSlot(1, CellDropZone.Center), new EverythingCombines());

            Assert.That(outcome.Kind, Is.EqualTo(GridDropOutcomeKind.Combine));
            Assert.That(outcome.Description, Is.EqualTo("Combine: rock + paper."));
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
