using SqueezySurvival.Launcher.Core.Updates;

namespace SqueezySurvival.Launcher.Tests.Fakes;

/// <summary>Configurable <see cref="IGameUpdateService"/> that records calls.</summary>
internal sealed class FakeGameUpdateService : IGameUpdateService
{
    public bool IsInstalled { get; set; } = true;

    public string? InstalledVersion { get; set; } = "0.0.1";

    public UpdateCheckResult CheckResult { get; set; } = UpdateCheckResult.UpToDate;

    /// <summary>When set, the check waits for this task before returning <see cref="CheckResult"/>.</summary>
    public Task? CheckGate { get; set; }

    /// <summary>When set, the download waits for this task before completing.</summary>
    public Task? DownloadGate { get; set; }

    public Exception? DownloadException { get; set; }

    public int CheckCount { get; private set; }

    public int DownloadCount { get; private set; }

    public int ApplyCount { get; private set; }

    public async Task<UpdateCheckResult> CheckForUpdateAsync()
    {
        CheckCount++;
        if (CheckGate is not null)
        {
            await CheckGate.ConfigureAwait(false);
        }

        return CheckResult;
    }

    public async Task DownloadUpdateAsync(IProgress<int> progress, CancellationToken cancellationToken)
    {
        DownloadCount++;
        progress.Report(50);
        if (DownloadGate is not null)
        {
            await DownloadGate.ConfigureAwait(false);
        }

        if (DownloadException is not null)
        {
            throw DownloadException;
        }

        progress.Report(100);
    }

    public void ApplyUpdateAndRestart() => ApplyCount++;
}
