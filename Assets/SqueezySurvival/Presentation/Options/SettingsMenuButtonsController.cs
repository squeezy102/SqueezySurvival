using System;
using System.Collections.Generic;
using SqueezySurvival.Core.Options;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Options
{
    /// <summary>
    /// Binds a SettingsMenuButtons.uxml instance: "Return to default settings" and "Capture these settings as my global
    /// preference", acting on the settings the menu shows (Docs/Design/UI/Options.md, Buttons in every settings menu).
    /// </summary>
    public sealed class SettingsMenuButtonsController
    {
        private readonly PlayerOptions _settings;
        private readonly PlayerOptions _globalPreference;
        private readonly Func<IReadOnlyList<GridPanel>> _panelsShown;
        private readonly Label _status;

        /// <summary>Binds the buttons.</summary>
        /// <param name="buttons">The template instance.</param>
        /// <param name="settings">The settings the menu edits.</param>
        /// <param name="globalPreference">
        /// The player's global preference to capture into, or null when the menu already edits the preference itself
        /// (the main menu's Options screen), which hides the capture button.
        /// </param>
        /// <param name="panelsShown">The grid panels whose settings the menu currently shows.</param>
        public SettingsMenuButtonsController(
            VisualElement buttons,
            PlayerOptions settings,
            PlayerOptions globalPreference,
            Func<IReadOnlyList<GridPanel>> panelsShown)
        {
            if (buttons == null)
            {
                throw new ArgumentNullException(nameof(buttons));
            }

            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _globalPreference = globalPreference;
            _panelsShown = panelsShown ?? throw new ArgumentNullException(nameof(panelsShown));
            _status = buttons.Q<Label>("settings-status");

            buttons.Q<Button>("settings-reset-button").clicked += ReturnToDefaults;
            Button captureButton = buttons.Q<Button>("settings-capture-button");
            captureButton.clicked += CaptureAsGlobalPreference;
            captureButton.style.display = globalPreference == null ? DisplayStyle.None : DisplayStyle.Flex;
            ClearStatus();
        }

        /// <summary>Clears the confirmation message, for when the menu opens.</summary>
        public void ClearStatus()
        {
            _status.text = string.Empty;
        }

        private void ReturnToDefaults()
        {
            _settings.ResetGridPanelLayouts(_panelsShown());
            _status.text = "Settings returned to their defaults.";
        }

        private void CaptureAsGlobalPreference()
        {
            _globalPreference.CopyGridPanelLayoutsFrom(_settings, _panelsShown());
            _status.text = "Saved as your global preference. New games start with these settings.";
        }
    }
}
