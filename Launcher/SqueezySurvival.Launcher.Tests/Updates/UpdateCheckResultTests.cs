using SqueezySurvival.Launcher.Core.Updates;

namespace SqueezySurvival.Launcher.Tests.Updates;

[TestFixture]
public sealed class UpdateCheckResultTests
{
    [Test]
    public void Available_WithVersion_ReportsUpdateAvailableAndVersion()
    {
        UpdateCheckResult result = UpdateCheckResult.Available("0.0.2");

        Assert.Multiple(() =>
        {
            Assert.That(result.Outcome, Is.EqualTo(UpdateCheckOutcome.UpdateAvailable));
            Assert.That(result.AvailableVersion, Is.EqualTo("0.0.2"));
        });
    }

    [TestCase("")]
    [TestCase(" ")]
    public void Available_WithBlankVersion_Throws(string version)
    {
        Assert.That(() => UpdateCheckResult.Available(version), Throws.ArgumentException);
    }

    [Test]
    public void UpToDate_HasNoAvailableVersion()
    {
        Assert.That(UpdateCheckResult.UpToDate.AvailableVersion, Is.Null);
    }
}
