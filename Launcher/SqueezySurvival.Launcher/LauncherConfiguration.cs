namespace SqueezySurvival.Launcher;

/// <summary>Fixed launcher settings. Release locations are defined in ADR-0004.</summary>
public static class LauncherConfiguration
{
    /// <summary>Public repository whose GitHub Releases host player builds.</summary>
    public const string ReleasesRepositoryUrl = "https://github.com/squeezy102/SqueezySurvival-Releases";

    /// <summary>Folder, relative to the launcher, that contains the Unity player.</summary>
    public const string GameFolderName = "Game";

    /// <summary>File name of the Unity player executable.</summary>
    public const string GameExecutableFileName = "SqueezySurvival.exe";

    /// <summary>Absolute path to the installed game executable.</summary>
    public static string GameExecutablePath =>
        Path.Combine(AppContext.BaseDirectory, GameFolderName, GameExecutableFileName);
}
