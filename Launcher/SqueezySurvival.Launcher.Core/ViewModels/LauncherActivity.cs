namespace SqueezySurvival.Launcher.Core.ViewModels;

/// <summary>What the launcher is currently doing. Drives button availability per the Launcher state table.</summary>
public enum LauncherActivity
{
    /// <summary>Waiting for player input.</summary>
    Idle,

    /// <summary>Checking the release source. Play remains available.</summary>
    Checking,

    /// <summary>Downloading an update. All actions are unavailable.</summary>
    Downloading,

    /// <summary>Applying an update and restarting. All actions are unavailable.</summary>
    Applying,
}
