using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using SqueezySurvival.Launcher.Core.ViewModels;
using SqueezySurvival.Launcher.Services;
using SqueezySurvival.Launcher.Views;

namespace SqueezySurvival.Launcher;

/// <summary>Avalonia application and composition root for the launcher.</summary>
public sealed partial class App : Application
{
    /// <inheritdoc />
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    /// <inheritdoc />
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            var mainWindow = new LauncherMainWindow();
            mainWindow.DataContext = new LauncherMainWindowViewModel(
                new VelopackGameUpdateService(LauncherConfiguration.ReleasesRepositoryUrl),
                new GameProcessStarter(LauncherConfiguration.GameExecutablePath),
                new AvaloniaUserPromptService(mainWindow),
                new DesktopLauncherLifetime(desktopLifetime));
            desktopLifetime.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
