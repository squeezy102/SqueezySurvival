using SqueezySurvival.Launcher.Core.Prompts;

namespace SqueezySurvival.Launcher.Tests.Fakes;

/// <summary><see cref="IUserPromptService"/> that records prompts and answers confirmations with a preset response.</summary>
internal sealed class FakeUserPromptService : IUserPromptService
{
    public bool ConfirmResponse { get; set; }

    public List<string> ShownMessages { get; } = [];

    public List<string> ConfirmationMessages { get; } = [];

    public Task<bool> ConfirmAsync(string title, string message, string confirmText, string cancelText)
    {
        ConfirmationMessages.Add(message);
        return Task.FromResult(ConfirmResponse);
    }

    public Task ShowMessageAsync(string title, string message)
    {
        ShownMessages.Add(message);
        return Task.CompletedTask;
    }
}
