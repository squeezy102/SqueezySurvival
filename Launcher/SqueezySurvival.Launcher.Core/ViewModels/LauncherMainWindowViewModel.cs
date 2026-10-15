using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SqueezySurvival.Launcher.Core.GameProcess;
using SqueezySurvival.Launcher.Core.Lifetime;
using SqueezySurvival.Launcher.Core.Prompts;
using SqueezySurvival.Launcher.Core.Updates;

namespace SqueezySurvival.Launcher.Core.ViewModels;

/// <summary>
/// State and commands for the launcher main window. Behavior is specified in Docs/Design/Launcher.md.
/// </summary>
public sealed partial class LauncherMainWindowViewModel : ObservableObject
{
    private readonly IGameUpdateService _gameUpdateService;
    private readonly IGameProcessStarter _gameProcessStarter;
    private readonly IUserPromptService _userPromptService;
    private readonly ILauncherLifetime _launcherLifetime;

    /// <summary>Creates the view model.</summary>
    public LauncherMainWindowViewModel(
        IGameUpdateService gameUpdateService,
        IGameProcessStarter gameProcessStarter,
        IUserPromptService userPromptService,
        ILauncherLifetime launcherLifetime)
    {
        ArgumentNullException.ThrowIfNull(gameUpdateService);
        ArgumentNullException.ThrowIfNull(gameProcessStarter);
        ArgumentNullException.ThrowIfNull(userPromptService);
        ArgumentNullException.ThrowIfNull(launcherLifetime);

        _gameUpdateService = gameUpdateService;
        _gameProcessStarter = gameProcessStarter;
        _userPromptService = userPromptService;
        _launcherLifetime = launcherLifetime;

        InstalledVersionText = _gameUpdateService.InstalledVersion is { } installedVersion
            ? LauncherText.FormatVersion(installedVersion)
            : LauncherText.DevelopmentBuildVersion;
        StatusText = string.Empty;
    }

    /// <summary>The installed version as shown in the bottom-right corner.</summary>
    public string InstalledVersionText { get; }

    /// <summary>Tooltip for the update indicator.</summary>
    public string UpdateIndicatorTooltip => LauncherText.UpdateIndicatorTooltip;

    /// <summary>What the launcher is currently doing.</summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(PlayCommand))]
    [NotifyCanExecuteChangedFor(nameof(CheckForUpdateCommand))]
    public partial LauncherActivity Activity { get; private set; }

    /// <summary>True when a newer version is known to exist. Shows the update indicator.</summary>
    [ObservableProperty]
    public partial bool IsUpdateAvailable { get; private set; }

    /// <summary>Progress text for the current activity; empty when idle.</summary>
    [ObservableProperty]
    public partial string StatusText { get; private set; }

    /// <summary>
    /// Runs the background update check performed when the window opens. Failures are silent;
    /// the player can retry with Check for update.
    /// </summary>
    public async Task InitializeAsync()
    {
        if (!CanCheckForUpdate())
        {
            return;
        }

        UpdateCheckResult result = await RunCheckAsync().ConfigureAwait(true);
        if (result.Outcome != UpdateCheckOutcome.Failed)
        {
            IsUpdateAvailable = result.Outcome == UpdateCheckOutcome.UpdateAvailable;
        }
    }

    [RelayCommand(CanExecute = nameof(CanPlay))]
    private async Task PlayAsync()
    {
        GameStartResult result = _gameProcessStarter.StartGame();
        if (result.Succeeded)
        {
            _launcherLifetime.CloseLauncher();
            return;
        }

        await _userPromptService.ShowMessageAsync(LauncherText.DialogTitle, result.FailureReason!).ConfigureAwait(true);
    }

    private bool CanPlay() => Activity is LauncherActivity.Idle or LauncherActivity.Checking;

    [RelayCommand(CanExecute = nameof(CanCheckForUpdate))]
    private async Task CheckForUpdateAsync()
    {
        UpdateCheckResult result = await RunCheckAsync().ConfigureAwait(true);
        string installedVersion = _gameUpdateService.InstalledVersion!;

        switch (result.Outcome)
        {
            case UpdateCheckOutcome.UpToDate:
                IsUpdateAvailable = false;
                await _userPromptService.ShowMessageAsync(
                    LauncherText.DialogTitle,
                    LauncherText.UpToDateMessage(installedVersion)).ConfigureAwait(true);
                break;

            case UpdateCheckOutcome.Failed:
                await _userPromptService.ShowMessageAsync(LauncherText.DialogTitle, LauncherText.CheckFailed).ConfigureAwait(true);
                break;

            case UpdateCheckOutcome.UpdateAvailable:
                IsUpdateAvailable = true;
                bool confirmed = await _userPromptService.ConfirmAsync(
                    LauncherText.DialogTitle,
                    LauncherText.UpdateAvailablePrompt(result.AvailableVersion!, installedVersion),
                    LauncherText.UpdateConfirm,
                    LauncherText.UpdateCancel).ConfigureAwait(true);
                if (confirmed)
                {
                    await DownloadAndApplyUpdateAsync().ConfigureAwait(true);
                }

                break;
        }
    }

    private bool CanCheckForUpdate() => _gameUpdateService.IsInstalled && Activity == LauncherActivity.Idle;

    private async Task<UpdateCheckResult> RunCheckAsync()
    {
        Activity = LauncherActivity.Checking;
        StatusText = LauncherText.CheckingStatus;
        try
        {
            return await _gameUpdateService.CheckForUpdateAsync().ConfigureAwait(true);
        }
        finally
        {
            Activity = LauncherActivity.Idle;
            StatusText = string.Empty;
        }
    }

    private async Task DownloadAndApplyUpdateAsync()
    {
        Activity = LauncherActivity.Downloading;
        StatusText = LauncherText.DownloadingStatus(0);

        // Progress<T> posts to the UI synchronization context. Late reports are ignored once downloading ends.
        var progress = new Progress<int>(percent =>
        {
            if (Activity == LauncherActivity.Downloading)
            {
                StatusText = LauncherText.DownloadingStatus(percent);
            }
        });

        try
        {
            await _gameUpdateService.DownloadUpdateAsync(progress, CancellationToken.None).ConfigureAwait(true);
        }
        catch (Exception)
        {
            // The download source raises transport-specific exceptions; all leave the installation unchanged.
            Activity = LauncherActivity.Idle;
            StatusText = string.Empty;
            await _userPromptService.ShowMessageAsync(LauncherText.DialogTitle, LauncherText.DownloadFailed).ConfigureAwait(true);
            return;
        }

        Activity = LauncherActivity.Applying;
        StatusText = LauncherText.ApplyingStatus;
        _gameUpdateService.ApplyUpdateAndRestart();
    }
}
