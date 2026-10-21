using SqueezySurvival.Presentation.Game;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SqueezySurvival.Editor.Setup
{
    /// <summary>
    /// Creates the game scene and adds it to the build (Docs/Design/UI/ScreenLayout.md, Wireframe stage).
    /// Run Configure Startup Screens first; it creates the shared panel settings. Safe to run again.
    /// </summary>
    public static class GameScreenSetupCommand
    {
        public const string MenuPath = "SqueezySurvival/Setup/Configure Game Screen";
        public const string GameLayoutPath = "Assets/SqueezySurvival/UI/Game/GameScreen.uxml";

        [MenuItem(MenuPath)]
        public static void ConfigureGameScreen()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            UiSceneBuilder.BuildUiScene(
                UiSceneBuilder.GameScenePath,
                GameLayoutPath,
                "Game Screen",
                typeof(GameCompositionRoot));
            UiSceneBuilder.ApplyBuildSceneList();

            Debug.Log($"Game screen configured. Game scene: {UiSceneBuilder.GameScenePath}.");
        }
    }
}
