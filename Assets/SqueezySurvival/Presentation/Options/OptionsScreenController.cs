using System;
using SqueezySurvival.Core.Options;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Options
{
    /// <summary>
    /// Binds an OptionsScreen.uxml instance to the shared <see cref="PlayerOptions"/>. Used by both the main menu and
    /// the pause menu (Docs/Design/UI/Options.md).
    /// </summary>
    public sealed class OptionsScreenController : IDisposable
    {
        private readonly VisualElement _screen;
        private readonly GridPanelLayoutOptionsEditorController _zoneFeaturesEditor;
        private readonly GridPanelLayoutOptionsEditorController _groundPoolEditor;

        /// <summary>Binds the screen's controls. The screen starts hidden.</summary>
        public OptionsScreenController(VisualElement screen, PlayerOptions options)
        {
            _screen = screen ?? throw new ArgumentNullException(nameof(screen));
            OverlayElementStyler.CoverParent(screen);

            _zoneFeaturesEditor = new GridPanelLayoutOptionsEditorController(
                screen.Q<VisualElement>("zone-features-layout-editor"), options, GridPanel.ZoneFeatures);
            _groundPoolEditor = new GridPanelLayoutOptionsEditorController(
                screen.Q<VisualElement>("ground-pool-layout-editor"), options, GridPanel.GroundPool);
            screen.Q<Button>("options-back-button").clicked += () => Closed?.Invoke();
            Hide();
        }

        /// <summary>Raised when the player chooses Back. The owner hides the screen and restores focus.</summary>
        public event Action Closed;

        /// <summary>Whether the screen is showing.</summary>
        public bool IsVisible => _screen.style.display != DisplayStyle.None;

        /// <summary>Shows the screen and focuses its first setting.</summary>
        public void Show()
        {
            _screen.style.display = DisplayStyle.Flex;
            _zoneFeaturesEditor.FirstControl.Focus();
        }

        /// <summary>Hides the screen.</summary>
        public void Hide()
        {
            _screen.style.display = DisplayStyle.None;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _zoneFeaturesEditor.Dispose();
            _groundPoolEditor.Dispose();
        }
    }
}
