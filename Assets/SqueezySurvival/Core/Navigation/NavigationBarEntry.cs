using System;

namespace SqueezySurvival.Core.Navigation
{
    /// <summary>One button of the nav bar: what it opens and how it is shown (Docs/Design/UI/ScreenLayout.md, Nav bar). Immutable.</summary>
    public sealed class NavigationBarEntry
    {
        /// <summary>Creates an entry.</summary>
        /// <param name="key">Stable identifier, such as "inventory"; never shown.</param>
        /// <param name="name">The name in its tooltip.</param>
        /// <param name="description">What the window is for, in its tooltip.</param>
        /// <param name="iconId">Its icon in the nav icon catalog.</param>
        /// <param name="isBuilt">Whether its window exists yet.</param>
        public NavigationBarEntry(string key, string name, string description, string iconId, bool isBuilt)
        {
            Key = string.IsNullOrWhiteSpace(key) ? throw new ArgumentException("Required.", nameof(key)) : key;
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Required.", nameof(name)) : name;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            IconId = string.IsNullOrWhiteSpace(iconId) ? throw new ArgumentException("Required.", nameof(iconId)) : iconId;
            IsBuilt = isBuilt;
        }

        /// <summary>Stable identifier, such as "inventory".</summary>
        public string Key { get; }

        /// <summary>The name in its tooltip.</summary>
        public string Name { get; }

        /// <summary>What the window is for.</summary>
        public string Description { get; }

        /// <summary>Its icon in the nav icon catalog.</summary>
        public string IconId { get; }

        /// <summary>Whether its window exists yet.</summary>
        public bool IsBuilt { get; }

        /// <summary>The extra tooltip line for an unbuilt window, or empty.</summary>
        public string AvailabilityNote => IsBuilt ? string.Empty : "Not built yet.";
    }
}
