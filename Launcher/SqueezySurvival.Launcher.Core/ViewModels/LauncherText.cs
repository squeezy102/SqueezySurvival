namespace SqueezySurvival.Launcher.Core.ViewModels;

/// <summary>Player-facing launcher text. Wording is specified in Docs/Design/Launcher.md.</summary>
public static class LauncherText
{
    /// <summary>Dialog title used by every launcher prompt.</summary>
    public const string DialogTitle = "SqueezySurvival";

    /// <summary>Tooltip on the update indicator.</summary>
    public const string UpdateIndicatorTooltip = "A newer version is available.";

    /// <summary>Version label shown when the launcher is not running from an installation.</summary>
    public const string DevelopmentBuildVersion = "Development build";

    /// <summary>Confirm button for the update prompt.</summary>
    public const string UpdateConfirm = "Update";

    /// <summary>Cancel button for the update prompt.</summary>
    public const string UpdateCancel = "Not now";

    /// <summary>Message when the check fails.</summary>
    public const string CheckFailed = "Couldn't check for updates. Check your internet connection and try again.";

    /// <summary>Message when the download fails.</summary>
    public const string DownloadFailed = "The update couldn't be downloaded. Your installed version has not changed. Check your internet connection and try again.";

    /// <summary>Status text while checking.</summary>
    public const string CheckingStatus = "Checking for updates...";

    /// <summary>Status text while applying.</summary>
    public const string ApplyingStatus = "Installing update...";

    /// <summary>Formats an installed version for display.</summary>
    public static string FormatVersion(string version) => $"v{version}";

    /// <summary>Prompt shown when a newer version exists.</summary>
    public static string UpdateAvailablePrompt(string availableVersion, string installedVersion) =>
        $"Version {availableVersion} is available. You have {installedVersion}. Update now?";

    /// <summary>Message shown when the installed version is current.</summary>
    public static string UpToDateMessage(string installedVersion) =>
        $"You have the latest version ({installedVersion}).";

    /// <summary>Status text while downloading.</summary>
    public static string DownloadingStatus(int percent) => $"Downloading update... {percent}%";
}
