namespace SqueezySurvival.Launcher.Core.Updates;

/// <summary>The result of an update check, including the available version when there is one.</summary>
public sealed record UpdateCheckResult
{
    private UpdateCheckResult(UpdateCheckOutcome outcome, string? availableVersion)
    {
        Outcome = outcome;
        AvailableVersion = availableVersion;
    }

    /// <summary>What the check found.</summary>
    public UpdateCheckOutcome Outcome { get; }

    /// <summary>The newer version, set only when <see cref="Outcome"/> is <see cref="UpdateCheckOutcome.UpdateAvailable"/>.</summary>
    public string? AvailableVersion { get; }

    /// <summary>The installed version is current.</summary>
    public static UpdateCheckResult UpToDate { get; } = new(UpdateCheckOutcome.UpToDate, null);

    /// <summary>The check could not complete.</summary>
    public static UpdateCheckResult Failed { get; } = new(UpdateCheckOutcome.Failed, null);

    /// <summary>A newer version exists.</summary>
    /// <param name="availableVersion">The newer version, for display.</param>
    public static UpdateCheckResult Available(string availableVersion)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(availableVersion);
        return new UpdateCheckResult(UpdateCheckOutcome.UpdateAvailable, availableVersion);
    }
}
