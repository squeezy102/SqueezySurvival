using SqueezySurvival.Launcher.Core.Lifetime;

namespace SqueezySurvival.Launcher.Tests.Fakes;

/// <summary><see cref="ILauncherLifetime"/> that records close requests.</summary>
internal sealed class FakeLauncherLifetime : ILauncherLifetime
{
    public int CloseCount { get; private set; }

    public void CloseLauncher() => CloseCount++;
}
