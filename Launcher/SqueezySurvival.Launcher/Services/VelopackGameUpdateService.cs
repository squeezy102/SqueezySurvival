using SqueezySurvival.Launcher.Core.Updates;
using Velopack;
using Velopack.Sources;

namespace SqueezySurvival.Launcher.Services;

/// <summary>
/// <see cref="IGameUpdateService"/> backed by Velopack, reading releases from a public GitHub repository.
/// </summary>
public sealed class VelopackGameUpdateService : IGameUpdateService
{
    private readonly UpdateManager _updateManager;
    private UpdateInfo? _foundUpdate;
    private bool _isDownloaded;

    /// <summary>Creates the service.</summary>
    /// <param name="releasesRepositoryUrl">Full URL of the GitHub repository that hosts releases.</param>
    public VelopackGameUpdateService(string releasesRepositoryUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(releasesRepositoryUrl);
        _updateManager = new UpdateManager(new GithubSource(releasesRepositoryUrl, accessToken: null, prerelease: false));
    }

    /// <inheritdoc />
    public bool IsInstalled => _updateManager.IsInstalled;

    /// <inheritdoc />
    public string? InstalledVersion => _updateManager.CurrentVersion?.ToString();

    /// <inheritdoc />
    public async Task<UpdateCheckResult> CheckForUpdateAsync()
    {
        try
        {
            _foundUpdate = await _updateManager.CheckForUpdatesAsync().ConfigureAwait(false);
            _isDownloaded = false;
        }
        catch (Exception)
        {
            // Network, HTTP and feed parsing errors are reported to the player as a failed check.
            return UpdateCheckResult.Failed;
        }

        return _foundUpdate is null
            ? UpdateCheckResult.UpToDate
            : UpdateCheckResult.Available(_foundUpdate.TargetFullRelease.Version.ToString());
    }

    /// <inheritdoc />
    public async Task DownloadUpdateAsync(IProgress<int> progress, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(progress);
        UpdateInfo update = _foundUpdate ?? throw new InvalidOperationException("No update has been found to download.");

        await _updateManager.DownloadUpdatesAsync(update, progress.Report, cancellationToken).ConfigureAwait(false);
        _isDownloaded = true;
    }

    /// <inheritdoc />
    public void ApplyUpdateAndRestart()
    {
        if (_foundUpdate is null || !_isDownloaded)
        {
            throw new InvalidOperationException("No update has been downloaded.");
        }

        _updateManager.ApplyUpdatesAndRestart(_foundUpdate.TargetFullRelease);
    }
}
