using NUnit.Framework;
using SqueezySurvival.Core.DragAndDrop;

namespace SqueezySurvival.Core.Tests.DragAndDrop
{
    [TestFixture]
    public sealed class CellDropZoneClassifierTests
    {
        [TestCase(0.5f, 0.5f, CellDropZone.Center)]
        [TestCase(0.25f, 0.75f, CellDropZone.Center)]
        [TestCase(0.1f, 0.5f, CellDropZone.Left)]
        [TestCase(0.9f, 0.5f, CellDropZone.Right)]
        [TestCase(0.5f, 0.1f, CellDropZone.Top)]
        [TestCase(0.5f, 0.9f, CellDropZone.Bottom)]
        [TestCase(0.05f, 0.2f, CellDropZone.Left)]
        [TestCase(0.2f, 0.05f, CellDropZone.Top)]
        [TestCase(-3f, 0.5f, CellDropZone.Left)]
        public void Classify_ReturnsZoneAtPoint(float x, float y, CellDropZone expected)
        {
            Assert.That(CellDropZoneClassifier.Classify(x, y), Is.EqualTo(expected));
        }
    }
}
