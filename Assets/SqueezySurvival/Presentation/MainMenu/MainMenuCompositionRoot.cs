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
            var optionStateEvaluator = new MainMenuOptionStateEvaluator(new NoSavedGamesAvailability());
            var commandRouter = new MainMenuCommandRouter(optionStateEvaluator);
            string versionLabelText = GameVersionLabelFormatter.Format(Application.version, Application.isEditor);

            GetComponent<MainMenuScreenController>().Bind(
                optionStateEvaluator, commandRouter, GameSessionHost.Current.RunLaunchHandoff, versionLabelText);
        }
    }
}
