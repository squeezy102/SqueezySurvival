using System;
using NUnit.Framework;
using SqueezySurvival.Core.Randomness;

namespace SqueezySurvival.Core.Tests.Randomness
{
    [TestFixture]
    public sealed class WeightedTableTests
    {
        private static WeightedTable<string> CreateTable() => new WeightedTable<string>(new[]
        {
            new WeightedTableEntry<string>("common", 3),
            new WeightedTableEntry<string>("rare", 1),
        });

        [Test]
        public void TotalWeight_IsSumOfWeights()
        {
            Assert.That(CreateTable().TotalWeight, Is.EqualTo(4));
        }

        [TestCase(0, "common")]
        [TestCase(2, "common")]
        [TestCase(3, "rare")]
        public void Pick_ReturnsOutcomeWhoseWeightBandHoldsTheRoll(int roll, string expected)
        {
            Assert.That(CreateTable().Pick(new QueuedRandomNumberSource(roll)), Is.EqualTo(expected));
        }

        [Test]
        public void Constructor_WithNoEntries_Throws()
        {
            Assert.Throws<ArgumentException>(() => new WeightedTable<string>(new WeightedTableEntry<string>[0]));
        }

        [Test]
        public void Entry_WithWeightBelowOne_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new WeightedTableEntry<string>("never", 0));
        }
    }
}
