using SqueezySurvival.Launcher.Core.GameProcess;

namespace SqueezySurvival.Launcher.Tests.Fakes;

/// <summary>Configurable <see cref="IGameProcessStarter"/> that records calls.</summary>
internal sealed class FakeGameProcessStarter : IGameProcessStarter
{
    public GameStartResult Result { get; set; } = GameStartResult.Success;

    public int StartCount { get; private set; }

    public GameStartResult StartGame()
    {
        StartCount++;
        return Result;
    }
}
