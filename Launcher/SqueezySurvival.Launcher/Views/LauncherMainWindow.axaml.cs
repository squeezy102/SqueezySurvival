using Avalonia.Controls;
using SqueezySurvival.Launcher.Core.ViewModels;

namespace SqueezySurvival.Launcher.Views;

/// <summary>Launcher main window. All behavior lives in <see cref="LauncherMainWindowViewModel"/>.</summary>
public sealed partial class LauncherMainWindow : Window
{
    /// <summary>Creates the window.</summary>
    public LauncherMainWindow()
    {
        InitializeComponent();
    }

    /// <inheritdoc />
    protected override async void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        if (DataContext is LauncherMainWindowViewModel viewModel)
        {
            await viewModel.InitializeAsync().ConfigureAwait(true);
        }
    }
}
