using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;

namespace SqueezySurvival.Core.Tests.DragAndDrop
{
    [TestFixture]
    public sealed class CellDropZoneClassifierTests
    {
        [TestCase(0.35f, 0.65f, CellDropZone.Center)]
        [TestCase(0.3f, 0.5f, CellDropZone.Left)]
        [TestCase(0.7f, 0.5f, CellDropZone.Right)]
        [TestCase(0.45f, 0.05f, CellDropZone.Left)]
        [TestCase(0.55f, 0.95f, CellDropZone.Right)]
        [TestCase(-3f, 0.5f, CellDropZone.Left)]
        public void Classify_ReturnsZoneAtPoint(float x, float y, CellDropZone expected)
        {
            Assert.That(CellDropZoneClassifier.Classify(x, y), Is.EqualTo(expected));
        }
    }
}
