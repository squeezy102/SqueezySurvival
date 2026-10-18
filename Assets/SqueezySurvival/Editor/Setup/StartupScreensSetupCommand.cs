using SqueezySurvival.Presentation.MainMenu;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace SqueezySurvival.Editor.Setup
{
    /// <summary>
    /// Creates everything the startup flow needs inside Unity: placeholder art import settings, splash logos,
    /// panel settings, the main menu scene and the build scene list (Docs/Design/UI/StartupFlow.md).
    /// Safe to run again; each run rebuilds the same result.
    /// </summary>
    public static class StartupScreensSetupCommand
    {
        public const string MenuPath = "SqueezySurvival/Setup/Configure Startup Screens";
        public const string StudioLogoTexturePath = "Assets/SqueezySurvival/Art/Placeholder/PlaceholderStudioLogo.png";
        public const string ThemeStyleSheetPath = "Assets/SqueezySurvival/UI/Themes/SqueezySurvivalRuntimeTheme.tss";
        public const string MainMenuLayoutPath = "Assets/SqueezySurvival/UI/MainMenu/MainMenu.uxml";

        private const float StudioLogoDurationSeconds = 2.5f;
        private static readonly Vector2Int ReferenceResolution = new Vector2Int(1920, 1080);

        [MenuItem(MenuPath)]
        public static void ConfigureStartupScreens()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            ImportStudioLogoAsSprite();
            ConfigureSplashScreen();
            CreateOrUpdatePanelSettings();
            AssetDatabase.SaveAssets();
            UiSceneBuilder.BuildUiScene(
                UiSceneBuilder.MainMenuScenePath,
                MainMenuLayoutPath,
                "Main Menu",
                typeof(MainMenuScreenController),
                typeof(MainMenuCompositionRoot));
            UiSceneBuilder.ApplyBuildSceneList();

            Debug.Log($"Startup screens configured. Main menu scene: {UiSceneBuilder.MainMenuScenePath}.");
        }

        private static void ImportStudioLogoAsSprite()
        {
            TextureImporter importer = GetTextureImporter(StudioLogoTexturePath);
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.alphaIsTransparency = true;
            importer.mipmapEnabled = false;
            importer.SaveAndReimport();
        }

        private static void ConfigureSplashScreen()
        {
            Sprite studioLogo = UiSceneBuilder.LoadRequiredAsset<Sprite>(StudioLogoTexturePath);

            PlayerSettings.SplashScreen.show = true;
            PlayerSettings.SplashScreen.showUnityLogo = true;
            PlayerSettings.SplashScreen.drawMode = PlayerSettings.SplashScreen.DrawMode.AllSequential;
            PlayerSettings.SplashScreen.logos = new[]
            {
                PlayerSettings.SplashScreenLogo.CreateWithUnityLogo(),
                PlayerSettings.SplashScreenLogo.Create(StudioLogoDurationSeconds, studioLogo),
            };
        }

        private static void CreateOrUpdatePanelSettings()
        {
            var panelSettings = AssetDatabase.LoadAssetAtPath<PanelSettings>(UiSceneBuilder.PanelSettingsPath);
            if (panelSettings == null)
            {
                panelSettings = ScriptableObject.CreateInstance<PanelSettings>();
                AssetDatabase.CreateAsset(panelSettings, UiSceneBuilder.PanelSettingsPath);
            }

            panelSettings.themeStyleSheet = UiSceneBuilder.LoadRequiredAsset<ThemeStyleSheet>(ThemeStyleSheetPath);
            panelSettings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
            panelSettings.referenceResolution = ReferenceResolution;
            panelSettings.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
            panelSettings.match = 0.5f;
            EditorUtility.SetDirty(panelSettings);
        }

        private static TextureImporter GetTextureImporter(string path)
        {
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
            if (!(AssetImporter.GetAtPath(path) is TextureImporter importer))
            {
                throw new System.InvalidOperationException($"No texture found at {path}.");
            }

            return importer;
        }
    }
}
