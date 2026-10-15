using Avalonia.Controls.ApplicationLifetimes;
using SqueezySurvival.Launcher.Core.Lifetime;

namespace SqueezySurvival.Launcher.Services;

/// <summary><see cref="ILauncherLifetime"/> for the Avalonia classic desktop lifetime.</summary>
public sealed class DesktopLauncherLifetime : ILauncherLifetime
{
    private readonly IClassicDesktopStyleApplicationLifetime _desktopLifetime;

    /// <summary>Creates the lifetime wrapper.</summary>
    public DesktopLauncherLifetime(IClassicDesktopStyleApplicationLifetime desktopLifetime)
    {
        ArgumentNullException.ThrowIfNull(desktopLifetime);
        _desktopLifetime = desktopLifetime;
    }

    /// <inheritdoc />
    public void CloseLauncher() => _desktopLifetime.Shutdown();
}
