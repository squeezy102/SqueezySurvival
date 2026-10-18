using System;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Options
{
    /// <summary>
    /// Binds an OptionsScreen.uxml instance. There are no settings yet, so it shows a note saying so and a Back
    /// button (Docs/Design/UI/Options.md, Wireframe stage).
    /// </summary>
    public sealed class OptionsScreenController
    {
        private readonly VisualElement _screen;
        private readonly Button _backButton;

        /// <summary>Binds the screen's controls. The screen starts hidden.</summary>
        public OptionsScreenController(VisualElement screen)
        {
            _screen = screen ?? throw new ArgumentNullException(nameof(screen));
            OverlayElementStyler.CoverParent(screen);
            _backButton = screen.Q<Button>("options-back-button");
            _backButton.clicked += () => Closed?.Invoke();
            Hide();
        }

        /// <summary>Raised when the player chooses Back. The owner hides the screen and restores focus.</summary>
        public event Action Closed;

        /// <summary>Whether the screen is showing.</summary>
        public bool IsVisible => _screen.style.display != DisplayStyle.None;

        /// <summary>Shows the screen and focuses Back.</summary>
        public void Show()
        {
            _screen.style.display = DisplayStyle.Flex;
            _backButton.Focus();
        }

        /// <summary>Hides the screen.</summary>
        public void Hide()
        {
            _screen.style.display = DisplayStyle.None;
        }
    }
}
