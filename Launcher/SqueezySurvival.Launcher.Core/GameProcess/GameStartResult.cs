namespace SqueezySurvival.Launcher.Core.GameProcess;

/// <summary>The result of attempting to start the game executable.</summary>
public sealed record GameStartResult
{
    private GameStartResult(bool succeeded, string? failureReason)
    {
        Succeeded = succeeded;
        FailureReason = failureReason;
    }

    /// <summary>True when the game process started.</summary>
    public bool Succeeded { get; }

    /// <summary>A player-readable reason, set only when the start failed.</summary>
    public string? FailureReason { get; }

    /// <summary>The game started.</summary>
    public static GameStartResult Success { get; } = new(true, null);

    /// <summary>The game did not start.</summary>
    /// <param name="failureReason">A player-readable reason.</param>
    public static GameStartResult Failure(string failureReason)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(failureReason);
        return new GameStartResult(false, failureReason);
    }
}
