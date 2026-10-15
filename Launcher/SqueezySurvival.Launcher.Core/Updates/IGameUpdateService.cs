namespace SqueezySurvival.Launcher.Core.Updates;

/// <summary>Reports the installed game version and finds, downloads and applies newer releases.</summary>
public interface IGameUpdateService
{
    /// <summary>True when the launcher is running from an installation that can be updated.</summary>
    bool IsInstalled { get; }

    /// <summary>The installed version, or null when not installed.</summary>
    string? InstalledVersion { get; }

    /// <summary>Checks the release source for a newer version. Never throws for network or source errors.</summary>
    Task<UpdateCheckResult> CheckForUpdateAsync();

    /// <summary>Downloads the update found by the most recent successful check.</summary>
    /// <param name="progress">Receives download progress from 0 to 100.</param>
    /// <param name="cancellationToken">Cancels the download.</param>
    /// <exception cref="InvalidOperationException">No update was found by a previous check.</exception>
    Task DownloadUpdateAsync(IProgress<int> progress, CancellationToken cancellationToken);

    /// <summary>Applies the downloaded update and restarts the launcher on the new version.</summary>
    /// <exception cref="InvalidOperationException">No update has been downloaded.</exception>
    void ApplyUpdateAndRestart();
}
