using NUnit.Framework;
using SqueezySurvival.Core.Options;

namespace SqueezySurvival.Core.Tests.Options
{
    [TestFixture]
    public sealed class PlayerOptionsPersistenceTests
    {
        private static readonly GridPanelLayoutOptions HorizontalInverted = new GridPanelLayoutOptions(GridPanelScrollDirection.Horizontal, 7, 2, true);

        [Test]
        public void Load_NothingSaved_GivesDefaultsWithoutWriting()
        {
            var store = new InMemoryPlayerOptionsStore();

            PlayerOptions options = PlayerOptionsPersistence.Load(store).Options;

            Assert.That(options.GridPanelLayout(GridPanel.GroundPool), Is.SameAs(GridPanelLayoutOptions.Default));
            Assert.That(store.WriteCount, Is.Zero);
        }

        [Test]
        public void Change_IsSavedAndReadBack()
        {
            var store = new InMemoryPlayerOptionsStore();
            PlayerOptionsPersistence.Load(store).Options.SetGridPanelLayout(GridPanel.GroundPool, HorizontalInverted);

            PlayerOptions reloaded = PlayerOptionsPersistence.Load(store).Options;

            Assert.That(store.WriteCount, Is.EqualTo(1));
            Assert.That(reloaded.GridPanelLayout(GridPanel.GroundPool).HasSameValuesAs(HorizontalInverted), Is.True);
            Assert.That(reloaded.GridPanelLayout(GridPanel.ZoneFeatures).HasSameValuesAs(GridPanelLayoutOptions.Default), Is.True);
        }
    }
}
