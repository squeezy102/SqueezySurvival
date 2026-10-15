namespace SqueezySurvival.Launcher.Core.Prompts;

/// <summary>Shows modal messages and confirmations to the player.</summary>
public interface IUserPromptService
{
    /// <summary>Asks the player to confirm an action.</summary>
    /// <returns>True when the player chose <paramref name="confirmText"/>.</returns>
    Task<bool> ConfirmAsync(string title, string message, string confirmText, string cancelText);

    /// <summary>Shows a message with a single dismiss button.</summary>
    Task ShowMessageAsync(string title, string message);
}
