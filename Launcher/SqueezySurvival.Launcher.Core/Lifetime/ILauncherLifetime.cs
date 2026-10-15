namespace SqueezySurvival.Launcher.Core.Lifetime;

/// <summary>Controls the launcher application's lifetime.</summary>
public interface ILauncherLifetime
{
    /// <summary>Closes the launcher.</summary>
    void CloseLauncher();
}
