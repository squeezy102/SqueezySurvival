using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Options
{
    /// <summary>
    /// A set of the player's settings. The game keeps two: the run's settings, which every screen of a run reads and
    /// edits, and the player's global preference, which new runs start from (Docs/Design/UI/Options.md, Where settings
    /// come from).
    /// </summary>
    public sealed class PlayerOptions
    {
        private readonly Dictionary<GridPanel, GridPanelLayoutOptions> _gridPanelLayouts = new Dictionary<GridPanel, GridPanelLayoutOptions>();

        /// <summary>Creates options with every setting at its default.</summary>
        public PlayerOptions()
        {
            foreach (GridPanel panel in AllGridPanels)
            {
                _gridPanelLayouts[panel] = GridPanelLayoutOptions.Default;
            }
        }

        /// <summary>Raised after a grid panel's settings change.</summary>
        public event Action<GridPanel> GridPanelLayoutChanged;

        /// <summary>Every configurable grid panel, in display order.</summary>
        public static IReadOnlyList<GridPanel> AllGridPanels { get; } = new[] { GridPanel.ZoneFeatures, GridPanel.GroundPool };

        /// <summary>A grid panel's current settings.</summary>
        public GridPanelLayoutOptions GridPanelLayout(GridPanel panel) => _gridPanelLayouts[panel];

        /// <summary>Copies the given panels' settings from another set, as when capturing a global preference.</summary>
        public void CopyGridPanelLayoutsFrom(PlayerOptions source, IEnumerable<GridPanel> panels)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            foreach (GridPanel panel in panels ?? throw new ArgumentNullException(nameof(panels)))
            {
                SetGridPanelLayout(panel, source.GridPanelLayout(panel));
            }
        }

        /// <summary>Sets the given panels back to their defaults.</summary>
        public void ResetGridPanelLayouts(IEnumerable<GridPanel> panels)
        {
            foreach (GridPanel panel in panels ?? throw new ArgumentNullException(nameof(panels)))
            {
                SetGridPanelLayout(panel, GridPanelLayoutOptions.Default);
            }
        }

        /// <summary>A separate copy of these settings.</summary>
        public PlayerOptions Copy()
        {
            var copy = new PlayerOptions();
            copy.CopyGridPanelLayoutsFrom(this, AllGridPanels);
            return copy;
        }

        /// <summary>Changes a grid panel's settings and raises <see cref="GridPanelLayoutChanged"/> if they differ.</summary>
        public void SetGridPanelLayout(GridPanel panel, GridPanelLayoutOptions layout)
        {
            if (layout == null)
            {
                throw new ArgumentNullException(nameof(layout));
            }

            if (!_gridPanelLayouts.ContainsKey(panel))
            {
                throw new ArgumentOutOfRangeException(nameof(panel), panel, "Unknown grid panel.");
            }

            if (_gridPanelLayouts[panel].HasSameValuesAs(layout))
            {
                return;
            }

            _gridPanelLayouts[panel] = layout;
            GridPanelLayoutChanged?.Invoke(panel);
        }
    }
}
