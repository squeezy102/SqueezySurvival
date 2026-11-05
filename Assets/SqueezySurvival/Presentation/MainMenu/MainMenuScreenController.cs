using SqueezySurvival.Core.MainMenu;
using SqueezySurvival.Core.Runs;
using SqueezySurvival.Core.Saving;
using SqueezySurvival.Presentation.PixelArt;
using SqueezySurvival.Presentation.Session;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Presentation.MainMenu
{
    /// <summary>
    /// Puts the main menu's UI Toolkit document under a <see cref="MainMenuScreenPresenter"/> and gives it Unity's scene
    /// loader and quit, and the pixel art look. All behavior is in the presenter.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public sealed class MainMenuScreenController : MonoBehaviour
    {
        private PixelArtTextureCache _pixelArtTextures;

        /// <summary>Fills in the menu and connects its controls. Called once by the scene's composition root.</summary>
        public void Bind(
            MainMenuOptionStateEvaluator optionStateEvaluator,
            MainMenuCommandRouter commandRouter,
            IRunInProgressLoader runInProgressLoader,
            RunLaunchHandoff runLaunchHandoff,
            string versionLabelText)
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            new MainMenuScreenPresenter(
                root,
                optionStateEvaluator,
                commandRouter,
                runInProgressLoader,
                runLaunchHandoff,
                new UnitySceneLoader(),
                new UnityApplicationQuitter(),
                versionLabelText);

            _pixelArtTextures = new PixelArtTextureCache();
            var skin = new PixelArtInterfaceSkin(_pixelArtTextures);
            skin.ApplyTo(root);
            skin.ApplyBackdrop(root.Q<VisualElement>("main-menu-root"));
        }

        private void OnDestroy()
        {
            _pixelArtTextures?.Dispose();
        }
    }
}
