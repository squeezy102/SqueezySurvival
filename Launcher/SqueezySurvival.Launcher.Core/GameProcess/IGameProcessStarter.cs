namespace SqueezySurvival.Launcher.Core.GameProcess;

/// <summary>Starts the installed game executable.</summary>
public interface IGameProcessStarter
{
    /// <summary>Starts the game. Never throws for a missing or unstartable executable.</summary>
    GameStartResult StartGame();
}
