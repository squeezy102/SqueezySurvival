using Avalonia.Controls;
using Avalonia.Interactivity;

namespace SqueezySurvival.Launcher.Views;

/// <summary>Modal message or confirmation dialog. Closes with true when confirmed, false otherwise.</summary>
public sealed partial class PromptDialogWindow : Window
{
    /// <summary>Creates an empty dialog. Required by the Avalonia XAML loader and designer.</summary>
    public PromptDialogWindow()
    {
        InitializeComponent();
    }

    /// <summary>Creates a dialog.</summary>
    /// <param name="title">Window title.</param>
    /// <param name="message">Message body.</param>
    /// <param name="confirmText">Confirm button text.</param>
    /// <param name="cancelText">Cancel button text, or null for a message with a single button.</param>
    public PromptDialogWindow(string title, string message, string confirmText, string? cancelText)
        : this()
    {
        Title = title;
        MessageTextBlock.Text = message;
        ConfirmButton.Content = confirmText;
        CancelButton.Content = cancelText;
        CancelButton.IsVisible = cancelText is not null;
    }

    private void OnConfirmClicked(object? sender, RoutedEventArgs e) => Close(true);

    private void OnCancelClicked(object? sender, RoutedEventArgs e) => Close(false);
}
