using System.Text;
using NUnit.Framework;
using SqueezySurvival.Core.PixelArt;

namespace SqueezySurvival.Core.Tests.PixelArt
{
    [TestFixture]
    public sealed class PixelIconCanvasTests
    {
        private static readonly PixelColor Ink = new PixelColor(10, 20, 30);

        // An 6 x 6 image with a 1-pixel margin draws on the inner 4 x 4; '#' is filled, '.' is empty.
        private static string Draw(System.Action<PixelIconCanvas> draw)
        {
            var image = new PixelImage(6, 6);
            draw(new PixelIconCanvas(image, 1));
            var rows = new StringBuilder();
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    rows.Append(image[x, y].IsVisible ? '#' : '.');
                }

                rows.Append('/');
            }

            return rows.ToString();
        }

        [Test]
        public void Polygon_FillsPixelsWhoseCentersAreInside()
        {
            // The left half of the square covers the centers of the left two of four columns.
            string drawn = Draw(canvas => canvas.Polygon(Ink, 0f, 0f, 0.5f, 0f, 0.5f, 1f, 0f, 1f));

            Assert.That(drawn, Is.EqualTo("....../.##.../.##.../.##.../.##.../....../"));
        }

        [Test]
        public void Line_ThinnerThanAPixel_IsStillOnePixelWide()
        {
            string drawn = Draw(canvas => canvas.Line(Ink, 0.01f, 0.375f, 0.125f, 0.375f, 0.875f));

            Assert.That(drawn, Is.EqualTo("....../..#.../..#.../..#.../..#.../....../"));
        }

        [Test]
        public void Ellipse_SmallerThanAPixel_IsStillOnePixel()
        {
            string drawn = Draw(canvas => canvas.Ellipse(Ink, 0.625f, 0.375f, 0.01f, 0.01f));

            Assert.That(drawn, Is.EqualTo("....../....../...#../....../....../....../"));
        }
    }
}
