using System;
using SqueezySurvival.Core.Options;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Options
{
    /// <summary>
    /// Binds a GridPanelSettingsWindow.uxml instance: the window a panel's gear opens, editing that panel's settings
    /// in the shared <see cref="PlayerOptions"/> (Docs/Design/UI/GridPanels.md, Where they are changed).
    /// </summary>
    public sealed class GridPanelSettingsWindowController : IDisposable
    {
        private readonly VisualElement _window;
        private readonly Label _heading;
        private readonly GridPanelLayoutOptionsEditorController _editor;

        /// <summary>Binds the window's controls. The window starts hidden.</summary>
        public GridPanelSettingsWindowController(VisualElement window, PlayerOptions options)
        {
            _window = window ?? throw new ArgumentNullException(nameof(window));
            OverlayElementStyler.CoverParent(window);

            _heading = window.Q<Label>("grid-panel-settings-heading");
            _editor = new GridPanelLayoutOptionsEditorController(
                window.Q<VisualElement>("grid-panel-settings-editor"), options, GridPanel.ZoneFeatures);
            window.Q<Button>("grid-panel-settings-close-button").clicked += () => Closed?.Invoke();
            Hide();
        }

        /// <summary>Raised when the player chooses Close. The owner hides the window and restores focus.</summary>
        public event Action Closed;

        /// <summary>Whether the window is showing.</summary>
        public bool IsVisible => _window.style.display != DisplayStyle.None;

        /// <summary>Shows the window for one panel.</summary>
        public void Show(GridPanel panel, string panelName)
        {
            _heading.text = $"{panelName} settings";
            _editor.ShowPanel(panel);
            _window.style.display = DisplayStyle.Flex;
            _editor.FirstControl.Focus();
        }

        /// <summary>Hides the window.</summary>
        public void Hide()
        {
            _window.style.display = DisplayStyle.None;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _editor.Dispose();
        }
    }
}
