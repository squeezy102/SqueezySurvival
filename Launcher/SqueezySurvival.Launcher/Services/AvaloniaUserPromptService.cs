using Avalonia.Controls;
using SqueezySurvival.Launcher.Core.Prompts;
using SqueezySurvival.Launcher.Views;

namespace SqueezySurvival.Launcher.Services;

/// <summary><see cref="IUserPromptService"/> that shows modal dialogs over the launcher window.</summary>
public sealed class AvaloniaUserPromptService : IUserPromptService
{
    private const string DismissText = "OK";

    private readonly Window _ownerWindow;

    /// <summary>Creates the service.</summary>
    /// <param name="ownerWindow">The window dialogs are shown over.</param>
    public AvaloniaUserPromptService(Window ownerWindow)
    {
        ArgumentNullException.ThrowIfNull(ownerWindow);
        _ownerWindow = ownerWindow;
    }

    /// <inheritdoc />
    public Task<bool> ConfirmAsync(string title, string message, string confirmText, string cancelText)
    {
        var dialog = new PromptDialogWindow(title, message, confirmText, cancelText);
        return dialog.ShowDialog<bool>(_ownerWindow);
    }

    /// <inheritdoc />
    public Task ShowMessageAsync(string title, string message)
    {
        var dialog = new PromptDialogWindow(title, message, DismissText, cancelText: null);
        return dialog.ShowDialog<bool>(_ownerWindow);
    }
}
