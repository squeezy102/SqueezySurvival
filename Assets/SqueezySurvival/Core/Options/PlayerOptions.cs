using System;
using System.Collections.Generic;

namespace SqueezySurvival.Core.Options
{
    /// <summary>
    /// The player's settings: one shared configuration that every screen reads and edits, held for the whole play
    /// session and saved between sessions (Docs/Design/UI/Options.md).
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
