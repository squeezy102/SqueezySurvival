using NUnit.Framework;
using SqueezySurvival.Core.Items;

namespace SqueezySurvival.Core.Tests.Items
{
    /// <summary><see cref="CountedCollection{TDefinition}.Revision"/> moves exactly when <c>Changed</c> is raised.</summary>
    [TestFixture]
    public sealed class CountedCollectionRevisionTests
    {
        private static readonly ItemDefinition Stone = TestItemDefinitions.Stone;

        [Test]
        public void Revision_NewCollection_IsZero()
        {
            Assert.That(new CountedCollection<ItemDefinition>(4).Revision, Is.EqualTo(0));
        }

        [Test]
        public void Revision_AfterAChange_IsHigher()
        {
            var collection = new CountedCollection<ItemDefinition>(4);

            collection.AddRange(new[] { Stone });

            Assert.That(collection.Revision, Is.GreaterThan(0));
        }

        [Test]
        public void Revision_IsAlreadyNewWhenChangedIsRaised()
        {
            var collection = new CountedCollection<ItemDefinition>(4);
            long revisionSeenByListener = -1;
            collection.Changed += (_, _) => revisionSeenByListener = collection.Revision;

            collection.AddRange(new[] { Stone });

            Assert.That(revisionSeenByListener, Is.EqualTo(collection.Revision));
            Assert.That(revisionSeenByListener, Is.GreaterThan(0));
        }

        [Test]
        public void Revision_WhenNothingChanges_StaysTheSame()
        {
            var collection = new CountedCollection<ItemDefinition>(4);
            collection.AddRange(new[] { Stone });
            long before = collection.Revision;

            collection.Sort(new ThingSortComparer<ItemDefinition>(ThingSortOrder.Alphabetical));

            Assert.That(collection.Revision, Is.EqualTo(before));
        }
    }
}
