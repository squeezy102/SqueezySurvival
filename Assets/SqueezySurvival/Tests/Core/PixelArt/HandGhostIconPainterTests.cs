using NUnit.Framework;
using SqueezySurvival.Core.PixelArt;
using SqueezySurvival.Core.Survivor;

namespace SqueezySurvival.Core.Tests.PixelArt
{
    [TestFixture]
    public sealed class HandGhostIconPainterTests
    {
        [Test]
        public void Create_LeftHand_IsTheRightHandMirrored()
        {
            PixelImage left = HandGhostIconPainter.Create(Hand.Left);
            PixelImage right = HandGhostIconPainter.Create(Hand.Right);

            bool mirrored = true;
            for (int y = 0; y < left.Height; y++)
            {
                for (int x = 0; x < left.Width; x++)
                {
                    mirrored &= left[x, y].Equals(right[right.Width - 1 - x, y]);
                }
            }

            Assert.That(mirrored, Is.True);
            Assert.That(left[3, 9], Is.Not.EqualTo(right[3, 9]), "A hand is not symmetric, so the two must differ.");
        }
    }
}
