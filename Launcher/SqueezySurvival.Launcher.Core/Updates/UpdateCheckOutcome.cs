namespace SqueezySurvival.Launcher.Core.Updates;

/// <summary>The possible results of checking the release source for a newer version.</summary>
public enum UpdateCheckOutcome
{
    /// <summary>The installed version is the latest release.</summary>
    UpToDate,

    /// <summary>A newer release is available.</summary>
    UpdateAvailable,

    /// <summary>The release source could not be reached or returned an error.</summary>
    Failed,
}
