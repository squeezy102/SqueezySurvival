using SqueezySurvival.Core.MainMenu;
using SqueezySurvival.Core.Saving;
using SqueezySurvival.Core.Versioning;
using SqueezySurvival.Presentation.Session;
using UnityEngine;

namespace SqueezySurvival.Presentation.MainMenu
{
    /// <summary>Creates the main menu's dependencies and hands them to the screen controller.</summary>
    [RequireComponent(typeof(MainMenuScreenController))]
    public sealed class MainMenuCompositionRoot : MonoBehaviour
    {
        // Start runs after every OnEnable, so the UIDocument has built its visual tree by then.
        private void Start()
        {
            // Until saving exists, development runs offer a placeholder run in progress so Continue can be
            // exercised; release builds have no saves (Docs/Design/UI/StartupFlow.md, Wireframe stage).
            if (Debug.isDebugBuild)
            {
                var placeholderSave = new DevelopmentPlaceholderSave();
                Compose(placeholderSave, placeholderSave);
            }
            else
            {
                var noSavedGames = new NoSavedGamesAvailability();
                Compose(noSavedGames, noSavedGames);
            }
        }

        private void Compose(ISavedGameAvailability savedGameAvailability, IRunInProgressLoader runInProgressLoader)
        {
            var optionStateEvaluator = new MainMenuOptionStateEvaluator(savedGameAvailability);
            var commandRouter = new MainMenuCommandRouter(optionStateEvaluator);
            string versionLabelText = GameVersionLabelFormatter.Format(Application.version, Application.isEditor);

            GetComponent<MainMenuScreenController>().Bind(
                optionStateEvaluator,
                commandRouter,
                runInProgressLoader,
                GameSessionHost.Current.RunLaunchHandoff,
                GameSessionHost.Current.PlayerOptions,
                versionLabelText);
        }
    }
}
