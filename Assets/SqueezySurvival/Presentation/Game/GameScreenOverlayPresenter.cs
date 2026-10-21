using System;
using System.Collections.Generic;
using SqueezySurvival.Core.GameScreen;
using SqueezySurvival.Core.Navigation;
using SqueezySurvival.Presentation.Options;
using SqueezySurvival.Presentation.Session;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.Game
{
    /// <summary>
    /// Shows the overlay on top of Core's <see cref="GameScreenOverlayStack"/>: the pause menu, Options or a "not built
    /// yet" notice, and returns focus to whatever opened each one when it closes (Docs/Design/UI/ScreenLayout.md).
    /// </summary>
    public sealed class GameScreenOverlayPresenter : IDisposable
    {
        private readonly GameScreenOverlayStack _overlays;
        private readonly Action _focusGame;
        private readonly VisualElement _pauseMenu;
        private readonly Button _pauseOptionsButton;
        private readonly OptionsScreenController _optionsScreen;
        private readonly VisualElement _notBuiltNotice;
        private readonly Label _notBuiltNoticeText;
        private readonly Button _notBuiltNoticeCloseButton;
        private readonly Dictionary<GameScreenOverlay, Focusable> _openedBy = new Dictionary<GameScreenOverlay, Focusable>();

        /// <summary>
        /// Binds the overlays in <paramref name="root"/> to <paramref name="overlays"/>. <paramref name="focusGame"/> gives
        /// the game screen its default focus when an overlay opened by the keyboard closes.
        /// </summary>
        public GameScreenOverlayPresenter(VisualElement root, GameScreenOverlayStack overlays, Action focusGame)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            _overlays = overlays ?? throw new ArgumentNullException(nameof(overlays));
            _focusGame = focusGame ?? throw new ArgumentNullException(nameof(focusGame));

            _pauseMenu = root.Q<VisualElement>("pause-menu");
            _pauseOptionsButton = root.Q<Button>("pause-options-button");
            _pauseOptionsButton.clicked += () => OpenOptions(_pauseOptionsButton);

            // Saving is not built yet, so Save and exit only exits (Docs/Design/UI/ScreenLayout.md, Wireframe stage).
            root.Q<Button>("save-and-exit-button").clicked += () => SceneManager.LoadScene(SceneNames.MainMenu);

            _optionsScreen = new OptionsScreenController(root.Q<VisualElement>("options-screen"));
            _optionsScreen.Closed += OnOptionsBackChosen;

            _notBuiltNotice = root.Q<VisualElement>("not-built-notice");
            _notBuiltNoticeText = root.Q<Label>("not-built-notice-text");
            _notBuiltNoticeCloseButton = root.Q<Button>("not-built-notice-close-button");
            _notBuiltNoticeCloseButton.clicked += OnNoticeCloseChosen;

            _overlays.Changed += OnOverlaysChanged;
            ShowTop();
        }

        /// <summary>Opens Options; closing it returns focus to <paramref name="openedBy"/>.</summary>
        public void OpenOptions(Focusable openedBy)
        {
            _openedBy[GameScreenOverlay.Options] = openedBy;
            _overlays.Open(GameScreenOverlay.Options);
        }

        /// <summary>Says that <paramref name="entry"/>'s window isn't built yet; closing returns focus to <paramref name="openedBy"/>.</summary>
        public void ShowNotBuiltNotice(NavigationBarEntry entry, Focusable openedBy)
        {
            _notBuiltNoticeText.text = NotBuiltNoticeTextFormatter.Format(entry);
            _openedBy[GameScreenOverlay.NotBuiltNotice] = openedBy;
            _overlays.Open(GameScreenOverlay.NotBuiltNotice);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _overlays.Changed -= OnOverlaysChanged;
            _optionsScreen.Closed -= OnOptionsBackChosen;
        }

        private void OnOptionsBackChosen() => CloseIfOnTop(GameScreenOverlay.Options);

        private void OnNoticeCloseChosen() => CloseIfOnTop(GameScreenOverlay.NotBuiltNotice);

        private void CloseIfOnTop(GameScreenOverlay overlay)
        {
            if (_overlays.Top == overlay)
            {
                _overlays.CloseTop();
            }
        }

        private void OnOverlaysChanged(object sender, GameScreenOverlayChange change)
        {
            ShowTop();
            if (change.Opened)
            {
                FocusOpened(change.Overlay);
            }
            else if (_openedBy.TryGetValue(change.Overlay, out Focusable openedBy) && openedBy != null)
            {
                _openedBy.Remove(change.Overlay);
                openedBy.Focus();
            }
            else
            {
                _focusGame();
            }
        }

        // Only the top overlay shows, so Options opened from the pause menu hides the menu until it closes.
        private void ShowTop()
        {
            GameScreenOverlay? top = _overlays.Top;
            _pauseMenu.style.display = top == GameScreenOverlay.PauseMenu ? DisplayStyle.Flex : DisplayStyle.None;
            _notBuiltNotice.style.display = top == GameScreenOverlay.NotBuiltNotice ? DisplayStyle.Flex : DisplayStyle.None;
            if (top == GameScreenOverlay.Options)
            {
                _optionsScreen.Show();
            }
            else
            {
                _optionsScreen.Hide();
            }
        }

        private void FocusOpened(GameScreenOverlay overlay)
        {
            switch (overlay)
            {
                case GameScreenOverlay.PauseMenu:
                    _pauseOptionsButton.Focus();
                    break;
                case GameScreenOverlay.NotBuiltNotice:
                    _notBuiltNoticeCloseButton.Focus();
                    break;
                case GameScreenOverlay.Options:
                    // Options focuses its own Back button when shown.
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(overlay), overlay, "Unknown overlay.");
            }
        }
    }
}
