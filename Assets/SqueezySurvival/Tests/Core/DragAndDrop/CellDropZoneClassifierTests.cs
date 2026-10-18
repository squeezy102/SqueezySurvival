using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;

namespace SqueezySurvival.Core.Tests.DragAndDrop
{
    [TestFixture]
    public sealed class CellDropZoneClassifierTests
    {
        // The center runs from 0.35 to 0.65 inclusive on both axes; its boundaries and the values just outside them.
        [TestCase(0.35f, 0.65f, CellDropZone.Center)]
        [TestCase(0.65f, 0.35f, CellDropZone.Center)]
        [TestCase(0.34f, 0.5f, CellDropZone.Edge)]
        [TestCase(0.66f, 0.5f, CellDropZone.Edge)]
        [TestCase(0.5f, 0.34f, CellDropZone.Edge)]
        [TestCase(0.5f, 0.66f, CellDropZone.Edge)]
        [TestCase(-3f, 0.5f, CellDropZone.Edge)]
        public void Classify_ReturnsZoneAtPoint(float x, float y, CellDropZone expected)
        {
            Assert.That(CellDropZoneClassifier.Classify(x, y), Is.EqualTo(expected));
        }
    }
}
