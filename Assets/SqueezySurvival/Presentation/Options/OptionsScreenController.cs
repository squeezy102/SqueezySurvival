using System;
using SqueezySurvival.Core.Options;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Options
{
    /// <summary>
    /// Binds an OptionsScreen.uxml instance to a set of settings: the run's settings during a run, or the player's
    /// global preference from the main menu (Docs/Design/UI/Options.md).
    /// </summary>
    public sealed class OptionsScreenController : IDisposable
    {
        private readonly VisualElement _screen;
        private readonly GridPanelLayoutOptionsEditorController _zoneFeaturesEditor;
        private readonly GridPanelLayoutOptionsEditorController _groundPoolEditor;
        private readonly SettingsMenuButtonsController _settingsMenuButtons;

        /// <summary>Binds the screen's controls. The screen starts hidden.</summary>
        /// <param name="screen">The template instance.</param>
        /// <param name="options">The settings the screen edits.</param>
        /// <param name="globalPreference">The global preference to capture into, or null when <paramref name="options"/> is the preference.</param>
        public OptionsScreenController(VisualElement screen, PlayerOptions options, PlayerOptions globalPreference)
        {
            _screen = screen ?? throw new ArgumentNullException(nameof(screen));
            OverlayElementStyler.CoverParent(screen);

            _zoneFeaturesEditor = new GridPanelLayoutOptionsEditorController(
                screen.Q<VisualElement>("zone-features-layout-editor"), options, GridPanel.ZoneFeatures);
            _groundPoolEditor = new GridPanelLayoutOptionsEditorController(
                screen.Q<VisualElement>("ground-pool-layout-editor"), options, GridPanel.GroundPool);
            screen.Q<Label>("options-scope-note").text = globalPreference == null
                ? "These settings are your global preference. Every game uses them unless that game has its own settings. Order of precedence: the game's own settings, then your global preference, then the defaults."
                : "These settings apply to this game only. To use them in every new game, capture them as your global preference.";
            _settingsMenuButtons = new SettingsMenuButtonsController(
                screen.Q<VisualElement>("options-settings-buttons"), options, globalPreference, () => PlayerOptions.AllGridPanels);
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
            _settingsMenuButtons.ClearStatus();
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
