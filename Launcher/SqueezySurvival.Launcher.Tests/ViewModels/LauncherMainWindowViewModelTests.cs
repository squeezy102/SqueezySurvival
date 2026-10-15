using SqueezySurvival.Launcher.Core.GameProcess;
using SqueezySurvival.Launcher.Core.Updates;
using SqueezySurvival.Launcher.Core.ViewModels;
using SqueezySurvival.Launcher.Tests.Fakes;

namespace SqueezySurvival.Launcher.Tests.ViewModels;

/// <summary>Verifies the behavior and state table in Docs/Design/Launcher.md.</summary>
[TestFixture]
public sealed class LauncherMainWindowViewModelTests
{
    private FakeGameUpdateService _updateService = null!;
    private FakeGameProcessStarter _processStarter = null!;
    private FakeUserPromptService _promptService = null!;
    private FakeLauncherLifetime _lifetime = null!;

    [SetUp]
    public void SetUp()
    {
        _updateService = new FakeGameUpdateService();
        _processStarter = new FakeGameProcessStarter();
        _promptService = new FakeUserPromptService();
        _lifetime = new FakeLauncherLifetime();
    }

    private LauncherMainWindowViewModel CreateViewModel() =>
        new(_updateService, _processStarter, _promptService, _lifetime);

    [Test]
    public void Constructor_WhenInstalled_ShowsFormattedInstalledVersion()
    {
        _updateService.InstalledVersion = "1.2.3";

        LauncherMainWindowViewModel viewModel = CreateViewModel();

        Assert.That(viewModel.InstalledVersionText, Is.EqualTo("v1.2.3"));
    }

    [Test]
    public void Constructor_WhenNotInstalled_ShowsDevelopmentBuild()
    {
        _updateService.IsInstalled = false;
        _updateService.InstalledVersion = null;

        LauncherMainWindowViewModel viewModel = CreateViewModel();

        Assert.That(viewModel.InstalledVersionText, Is.EqualTo(LauncherText.DevelopmentBuildVersion));
    }

    [Test]
    public void Constructor_WhenIdle_EnablesPlayAndCheckAndHidesIndicator()
    {
        LauncherMainWindowViewModel viewModel = CreateViewModel();

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.PlayCommand.CanExecute(null), Is.True);
            Assert.That(viewModel.CheckForUpdateCommand.CanExecute(null), Is.True);
            Assert.That(viewModel.IsUpdateAvailable, Is.False);
        });
    }

    [Test]
    public void Constructor_WhenNotInstalled_EnablesPlayAndDisablesCheck()
    {
        _updateService.IsInstalled = false;
        _updateService.InstalledVersion = null;

        LauncherMainWindowViewModel viewModel = CreateViewModel();

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.PlayCommand.CanExecute(null), Is.True);
            Assert.That(viewModel.CheckForUpdateCommand.CanExecute(null), Is.False);
        });
    }

    [Test]
    public async Task InitializeAsync_WhenUpdateAvailable_ShowsIndicatorWithoutPrompting()
    {
        _updateService.CheckResult = UpdateCheckResult.Available("0.0.2");
        LauncherMainWindowViewModel viewModel = CreateViewModel();

        await viewModel.InitializeAsync();

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.IsUpdateAvailable, Is.True);
            Assert.That(_promptService.ConfirmationMessages, Is.Empty);
            Assert.That(_promptService.ShownMessages, Is.Empty);
        });
    }

    [Test]
    public async Task InitializeAsync_WhenCheckFails_HidesIndicatorWithoutMessage()
    {
        _updateService.CheckResult = UpdateCheckResult.Failed;
        LauncherMainWindowViewModel viewModel = CreateViewModel();

        await viewModel.InitializeAsync();

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.IsUpdateAvailable, Is.False);
            Assert.That(_promptService.ShownMessages, Is.Empty);
        });
    }

    [Test]
    public async Task InitializeAsync_WhenNotInstalled_DoesNotCheck()
    {
        _updateService.IsInstalled = false;
        _updateService.InstalledVersion = null;
        LauncherMainWindowViewModel viewModel = CreateViewModel();

        await viewModel.InitializeAsync();

        Assert.That(_updateService.CheckCount, Is.Zero);
    }

    [Test]
    public async Task Activity_WhileChecking_EnablesPlayAndDisablesCheck()
    {
        var checkGate = new TaskCompletionSource();
        _updateService.CheckGate = checkGate.Task;
        LauncherMainWindowViewModel viewModel = CreateViewModel();

        Task initialization = viewModel.InitializeAsync();

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Activity, Is.EqualTo(LauncherActivity.Checking));
            Assert.That(viewModel.PlayCommand.CanExecute(null), Is.True);
            Assert.That(viewModel.CheckForUpdateCommand.CanExecute(null), Is.False);
        });

        checkGate.SetResult();
        await initialization;
        Assert.That(viewModel.Activity, Is.EqualTo(LauncherActivity.Idle));
    }

    [Test]
    public async Task Play_WhenGameStarts_ClosesLauncher()
    {
        LauncherMainWindowViewModel viewModel = CreateViewModel();

        await viewModel.PlayCommand.ExecuteAsync(null);

        Assert.Multiple(() =>
        {
            Assert.That(_processStarter.StartCount, Is.EqualTo(1));
            Assert.That(_lifetime.CloseCount, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task Play_WhenGameFailsToStart_ShowsReasonAndKeepsLauncherOpen()
    {
        _processStarter.Result = GameStartResult.Failure("Missing files.");
        LauncherMainWindowViewModel viewModel = CreateViewModel();

        await viewModel.PlayCommand.ExecuteAsync(null);

        Assert.Multiple(() =>
        {
            Assert.That(_promptService.ShownMessages, Is.EqualTo(new[] { "Missing files." }));
            Assert.That(_lifetime.CloseCount, Is.Zero);
        });
    }

    [Test]
    public async Task CheckForUpdate_WhenUpToDate_ShowsLatestVersionMessageAndClearsIndicator()
    {
        _updateService.CheckResult = UpdateCheckResult.Available("0.0.2");
        LauncherMainWindowViewModel viewModel = CreateViewModel();
        await viewModel.InitializeAsync();
        _updateService.CheckResult = UpdateCheckResult.UpToDate;

        await viewModel.CheckForUpdateCommand.ExecuteAsync(null);

        Assert.Multiple(() =>
        {
            Assert.That(_promptService.ShownMessages, Is.EqualTo(new[] { LauncherText.UpToDateMessage("0.0.1") }));
            Assert.That(viewModel.IsUpdateAvailable, Is.False);
        });
    }

    [Test]
    public async Task CheckForUpdate_WhenCheckFails_ShowsCheckFailedMessage()
    {
        _updateService.CheckResult = UpdateCheckResult.Failed;
        LauncherMainWindowViewModel viewModel = CreateViewModel();

        await viewModel.CheckForUpdateCommand.ExecuteAsync(null);

        Assert.That(_promptService.ShownMessages, Is.EqualTo(new[] { LauncherText.CheckFailed }));
    }

    [Test]
    public async Task CheckForUpdate_WhenUpdateAvailable_PromptsWithAvailableAndInstalledVersions()
    {
        _updateService.CheckResult = UpdateCheckResult.Available("0.0.2");
        LauncherMainWindowViewModel viewModel = CreateViewModel();

        await viewModel.CheckForUpdateCommand.ExecuteAsync(null);

        Assert.Multiple(() =>
        {
            Assert.That(
                _promptService.ConfirmationMessages,
                Is.EqualTo(new[] { LauncherText.UpdateAvailablePrompt("0.0.2", "0.0.1") }));
            Assert.That(viewModel.IsUpdateAvailable, Is.True);
        });
    }

    [Test]
    public async Task CheckForUpdate_WhenPlayerDeclines_DoesNotDownloadAndKeepsIndicator()
    {
        _updateService.CheckResult = UpdateCheckResult.Available("0.0.2");
        _promptService.ConfirmResponse = false;
        LauncherMainWindowViewModel viewModel = CreateViewModel();

        await viewModel.CheckForUpdateCommand.ExecuteAsync(null);

        Assert.Multiple(() =>
        {
            Assert.That(_updateService.DownloadCount, Is.Zero);
            Assert.That(viewModel.IsUpdateAvailable, Is.True);
            Assert.That(viewModel.Activity, Is.EqualTo(LauncherActivity.Idle));
        });
    }

    [Test]
    public async Task CheckForUpdate_WhenPlayerAccepts_DownloadsThenApplies()
    {
        _updateService.CheckResult = UpdateCheckResult.Available("0.0.2");
        _promptService.ConfirmResponse = true;
        LauncherMainWindowViewModel viewModel = CreateViewModel();

        await viewModel.CheckForUpdateCommand.ExecuteAsync(null);

        Assert.Multiple(() =>
        {
            Assert.That(_updateService.DownloadCount, Is.EqualTo(1));
            Assert.That(_updateService.ApplyCount, Is.EqualTo(1));
            Assert.That(viewModel.Activity, Is.EqualTo(LauncherActivity.Applying));
            Assert.That(viewModel.PlayCommand.CanExecute(null), Is.False);
            Assert.That(viewModel.CheckForUpdateCommand.CanExecute(null), Is.False);
        });
    }

    [Test]
    public async Task Activity_WhileDownloading_DisablesPlayAndCheck()
    {
        var downloadGate = new TaskCompletionSource();
        _updateService.CheckResult = UpdateCheckResult.Available("0.0.2");
        _updateService.DownloadGate = downloadGate.Task;
        _promptService.ConfirmResponse = true;
        LauncherMainWindowViewModel viewModel = CreateViewModel();

        Task checkAndUpdate = viewModel.CheckForUpdateCommand.ExecuteAsync(null);

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.Activity, Is.EqualTo(LauncherActivity.Downloading));
            Assert.That(viewModel.PlayCommand.CanExecute(null), Is.False);
            Assert.That(viewModel.CheckForUpdateCommand.CanExecute(null), Is.False);
            Assert.That(viewModel.IsUpdateAvailable, Is.True);
        });

        downloadGate.SetResult();
        await checkAndUpdate;
    }

    [Test]
    public async Task CheckForUpdate_WhenDownloadFails_ShowsErrorReturnsToIdleAndDoesNotApply()
    {
        _updateService.CheckResult = UpdateCheckResult.Available("0.0.2");
        _updateService.DownloadException = new HttpRequestException("Connection lost.");
        _promptService.ConfirmResponse = true;
        LauncherMainWindowViewModel viewModel = CreateViewModel();

        await viewModel.CheckForUpdateCommand.ExecuteAsync(null);

        Assert.Multiple(() =>
        {
            Assert.That(_promptService.ShownMessages, Is.EqualTo(new[] { LauncherText.DownloadFailed }));
            Assert.That(_updateService.ApplyCount, Is.Zero);
            Assert.That(viewModel.Activity, Is.EqualTo(LauncherActivity.Idle));
            Assert.That(viewModel.StatusText, Is.Empty);
            Assert.That(viewModel.PlayCommand.CanExecute(null), Is.True);
        });
    }
}
