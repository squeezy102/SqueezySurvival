using Avalonia;
using Velopack;

namespace SqueezySurvival.Launcher;

/// <summary>Launcher entry point.</summary>
internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        // Velopack must run first: install, update and uninstall hooks re-launch this executable and exit here.
        VelopackApp.Build().Run();
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    /// <summary>Avalonia configuration. Also used by the Avalonia designer; do not remove.</summary>
    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}
