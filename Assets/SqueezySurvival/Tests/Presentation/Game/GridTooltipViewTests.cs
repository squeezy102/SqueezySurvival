using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.Items;
using SqueezySurvival.Core.Zones;
using SqueezySurvival.Presentation.Game;
using SqueezySurvival.Presentation.PixelArt;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Tests.Game
{
    [TestFixture]
    public sealed class GridTooltipViewTests : GameScreenUITestFixture
    {
        private static readonly Rect Cell = new Rect(100, 100, 64, 64);

        private PixelArtTextureCache _textures;
        private GridTooltipView _tooltip;

        [SetUp]
        public void CreateTooltip()
        {
            _textures = new PixelArtTextureCache();
            _tooltip = new GridTooltipView(Element<VisualElement>("grid-tooltip"), _textures);
        }

        [TearDown]
        public void DisposeTextures() => _textures.Dispose();

        private static ThingTooltipContent Content(string subtitle, string iconId, params string[] detailLines) =>
            new ThingTooltipContent("Stone", subtitle, "A fist-sized stone.", detailLines, iconId);

        private List<string> VisibleDetailLines() =>
            Element<VisualElement>("grid-tooltip-details")
                .Query<Label>(className: "grid-tooltip-detail")
                .ToList()
                .Where(line => line.style.display.value != DisplayStyle.None)
                .Select(line => line.text)
                .ToList();

        private List<VisualElement> AllTooltipElements() => Element<VisualElement>("grid-tooltip").Query<VisualElement>().ToList();

        [Test]
        public void Show_ShowsTheContent()
        {
            _tooltip.Show(Content("Stone and minerals", PlaceholderBeachZoneContent.Stone.Id, "Weight: 1 lb each", "Stack of 3: 3 lb"), Cell);

            Assert.That(Element<VisualElement>("grid-tooltip").style.display.value, Is.EqualTo(DisplayStyle.Flex));
            Assert.That(Element<Label>("grid-tooltip-title").text, Is.EqualTo("Stone"));
            Assert.That(Element<Label>("grid-tooltip-subtitle").style.display.value, Is.EqualTo(DisplayStyle.Flex));
            Assert.That(Element<Label>("grid-tooltip-description").text, Is.EqualTo("A fist-sized stone."));
            Assert.That(Element<VisualElement>("grid-tooltip-icon").style.display.value, Is.EqualTo(DisplayStyle.Flex));
            Assert.That(VisibleDetailLines(), Is.EqualTo(new[] { "Weight: 1 lb each", "Stack of 3: 3 lb" }));
        }

        [Test]
        public void Show_NoSubtitleOrIcon_HidesThem()
        {
            _tooltip.Show(Content(string.Empty, string.Empty), Cell);

            Assert.That(Element<Label>("grid-tooltip-subtitle").style.display.value, Is.EqualTo(DisplayStyle.None));
            Assert.That(Element<VisualElement>("grid-tooltip-icon").style.display.value, Is.EqualTo(DisplayStyle.None));
        }

        [Test]
        public void Show_FewerDetailLinesThanBefore_HidesTheRest()
        {
            _tooltip.Show(Content(string.Empty, string.Empty, "First", "Second"), Cell);

            _tooltip.Show(Content(string.Empty, string.Empty, "Only"), Cell);

            Assert.That(VisibleDetailLines(), Is.EqualTo(new[] { "Only" }));
        }

        [Test]
        public void Show_AfterALongerTooltip_CreatesNoElements()
        {
            _tooltip.Show(Content("Stone and minerals", PlaceholderBeachZoneContent.Stone.Id, "First", "Second"), Cell);
            List<VisualElement> before = AllTooltipElements();

            _tooltip.Show(Content(string.Empty, PlaceholderBeachZoneContent.SmallStick.Id, "Only"), Cell);
            _tooltip.Show(Content("Stone and minerals", PlaceholderBeachZoneContent.Stone.Id, "First", "Second"), Cell);

            Assert.That(AllTooltipElements(), Is.EqualTo(before));
        }

        [Test]
        public void Hide_HidesTheTooltip()
        {
            _tooltip.Show(Content(string.Empty, string.Empty), Cell);

            _tooltip.Hide();

            Assert.That(Element<VisualElement>("grid-tooltip").style.display.value, Is.EqualTo(DisplayStyle.None));
        }
    }
}
