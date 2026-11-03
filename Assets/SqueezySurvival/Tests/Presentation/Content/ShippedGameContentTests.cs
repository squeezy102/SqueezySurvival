using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SqueezySurvival.Core.Content;
using SqueezySurvival.Core.PixelArt;
using SqueezySurvival.Core.Zones;
using SqueezySurvival.Editor.Setup;
using SqueezySurvival.Presentation.Content;
using UnityEditor;
using UnityEngine;

namespace SqueezySurvival.Presentation.Tests.Content
{
    /// <summary>
    /// Loads the content files the game ships, so a mistake in them fails a test before it reaches a build
    /// (Docs/Decisions/ADR-0009-Data-Driven-Content.md, Tests). The only tests that depend on shipped content.
    /// </summary>
    [TestFixture]
    public sealed class ShippedGameContentTests
    {
        private GameContentCatalog _catalog;

        [OneTimeSetUp]
        public void LoadShippedContent()
        {
            string[] zoneFilePaths = GameScreenSetupCommand.FindZoneFilePaths();
            Assert.That(zoneFilePaths, Is.Not.Empty, $"There are no zone files under {GameScreenSetupCommand.ZoneFilesFolderPath}.");

            List<ZoneJsonText> zoneFiles = zoneFilePaths
                .Select(path => new ZoneJsonText(System.IO.Path.GetFileNameWithoutExtension(path), LoadText(path)))
                .ToList();
            _catalog = new GameContentCatalogLoader(new GameContentJsonReader()).Load(
                LoadText(GameScreenSetupCommand.ItemsFilePath), LoadText(GameScreenSetupCommand.ZoneFeaturesFilePath), zoneFiles);
        }

        [Test]
        public void ShippedContent_HasTheZoneRunsStartIn()
        {
            ZoneDefinition startingZone = StartingZoneRules.Wireframe.SelectStartingZone(_catalog);

            Assert.That(startingZone.Id, Is.EqualTo(StartingZoneRules.Wireframe.StartingZoneId));
        }

        [Test]
        public void ShippedContent_GivesEveryItemAndZoneFeatureAPlaceholderIcon()
        {
            IEnumerable<string> ids = _catalog.Items.Select(item => item.Id).Concat(_catalog.ZoneFeatures.Select(feature => feature.Id));

            Assert.That(ids.Where(id => !PlaceholderIconCatalog.HasIcon(id)), Is.Empty);
        }

        private static string LoadText(string assetPath)
        {
            var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
            Assert.That(asset, Is.Not.Null, $"The content file {assetPath} is missing.");
            return asset.text;
        }
    }
}
