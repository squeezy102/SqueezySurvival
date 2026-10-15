using System.ComponentModel;
using System.Diagnostics;
using SqueezySurvival.Launcher.Core.GameProcess;

namespace SqueezySurvival.Launcher.Services;

/// <summary>Starts the Unity game executable as a separate process.</summary>
public sealed class GameProcessStarter : IGameProcessStarter
{
    private const string GameNotFoundReason = "The game files could not be found. Reinstall SqueezySurvival to repair the installation.";
    private const string GameFailedToStartReason = "The game could not be started. Restart your computer and try again.";

    private readonly string _gameExecutablePath;

    /// <summary>Creates the starter.</summary>
    /// <param name="gameExecutablePath">Absolute path to the game executable.</param>
    public GameProcessStarter(string gameExecutablePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(gameExecutablePath);
        _gameExecutablePath = gameExecutablePath;
    }

    /// <inheritdoc />
    public GameStartResult StartGame()
    {
        if (!File.Exists(_gameExecutablePath))
        {
            return GameStartResult.Failure(GameNotFoundReason);
        }

        var startInfo = new ProcessStartInfo(_gameExecutablePath)
        {
            WorkingDirectory = Path.GetDirectoryName(_gameExecutablePath),
            UseShellExecute = false,
        };

        try
        {
            using Process? process = Process.Start(startInfo);
            return process is null ? GameStartResult.Failure(GameFailedToStartReason) : GameStartResult.Success;
        }
        catch (Win32Exception)
        {
            return GameStartResult.Failure(GameFailedToStartReason);
        }
    }
}
