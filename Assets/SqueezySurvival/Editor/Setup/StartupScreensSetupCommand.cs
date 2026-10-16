using SqueezySurvival.Presentation.MainMenu;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public const string BackgroundTexturePath = "Assets/SqueezySurvival/Art/Placeholder/PlaceholderBeachBackground.png";
        public const string StudioLogoTexturePath = "Assets/SqueezySurvival/Art/Placeholder/PlaceholderStudioLogo.png";
        public const string ThemeStyleSheetPath = "Assets/SqueezySurvival/UI/Themes/SqueezySurvivalRuntimeTheme.tss";
        public const string PanelSettingsPath = "Assets/SqueezySurvival/UI/SqueezySurvivalPanelSettings.asset";
        public const string MainMenuLayoutPath = "Assets/SqueezySurvival/UI/MainMenu/MainMenu.uxml";
        public const string MainMenuScenePath = "Assets/SqueezySurvival/Scenes/MainMenu.unity";

        private const float StudioLogoDurationSeconds = 2.5f;
        private static readonly Vector2Int ReferenceResolution = new Vector2Int(1920, 1080);

        [MenuItem(MenuPath)]
        public static void ConfigureStartupScreens()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            ImportBackgroundAsTexture();
            ImportStudioLogoAsSprite();
            ConfigureSplashScreen();
            PanelSettings panelSettings = CreateOrUpdatePanelSettings();
            CreateMainMenuScene(panelSettings);
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(MainMenuScenePath, true) };
            AssetDatabase.SaveAssets();

            Debug.Log($"Startup screens configured. Main menu scene: {MainMenuScenePath}.");
        }

        private static void ImportBackgroundAsTexture()
        {
            TextureImporter importer = GetTextureImporter(BackgroundTexturePath);
            importer.textureType = TextureImporterType.Default;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.mipmapEnabled = false;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.SaveAndReimport();
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
            Sprite studioLogo = LoadRequiredAsset<Sprite>(StudioLogoTexturePath);

            PlayerSettings.SplashScreen.show = true;
            PlayerSettings.SplashScreen.showUnityLogo = true;
            PlayerSettings.SplashScreen.drawMode = PlayerSettings.SplashScreen.DrawMode.AllSequential;
            PlayerSettings.SplashScreen.logos = new[]
            {
                PlayerSettings.SplashScreenLogo.CreateWithUnityLogo(),
                PlayerSettings.SplashScreenLogo.Create(StudioLogoDurationSeconds, studioLogo),
            };
        }

        private static PanelSettings CreateOrUpdatePanelSettings()
        {
            var panelSettings = AssetDatabase.LoadAssetAtPath<PanelSettings>(PanelSettingsPath);
            if (panelSettings == null)
            {
                panelSettings = ScriptableObject.CreateInstance<PanelSettings>();
                AssetDatabase.CreateAsset(panelSettings, PanelSettingsPath);
            }

            panelSettings.themeStyleSheet = LoadRequiredAsset<ThemeStyleSheet>(ThemeStyleSheetPath);
            panelSettings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
            panelSettings.referenceResolution = ReferenceResolution;
            panelSettings.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
            panelSettings.match = 0.5f;
            EditorUtility.SetDirty(panelSettings);
            return panelSettings;
        }

        private static void CreateMainMenuScene(PanelSettings panelSettings)
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var cameraObject = new GameObject("Main Camera") { tag = "MainCamera" };
            var mainCamera = cameraObject.AddComponent<Camera>();
            mainCamera.orthographic = true;
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = Color.black;

            var mainMenuObject = new GameObject("Main Menu");
            var document = mainMenuObject.AddComponent<UIDocument>();
            document.panelSettings = panelSettings;
            document.visualTreeAsset = LoadRequiredAsset<VisualTreeAsset>(MainMenuLayoutPath);
            mainMenuObject.AddComponent<MainMenuScreenController>();
            mainMenuObject.AddComponent<MainMenuCompositionRoot>();

            EditorSceneManager.SaveScene(scene, MainMenuScenePath);
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

        private static T LoadRequiredAsset<T>(string path) where T : Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                throw new System.InvalidOperationException($"Required {typeof(T).Name} not found at {path}.");
            }

            return asset;
        }
    }
}
