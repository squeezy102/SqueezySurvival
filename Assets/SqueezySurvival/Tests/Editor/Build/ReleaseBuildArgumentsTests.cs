using System;
using NUnit.Framework;
using SqueezySurvival.Editor.Build;

namespace SqueezySurvival.Editor.Tests.Build
{
    [TestFixture]
    public sealed class ReleaseBuildArgumentsTests
    {
        [Test]
        public void Parse_WithVersionAndOutputPath_ReturnsBoth()
        {
            string[] commandLine = { "Unity.exe", "-batchmode", "-releaseVersion", "1.2.3", "-outputPath", @"C:\Builds\Game", "-quit" };

            ReleaseBuildArguments arguments = ReleaseBuildArguments.Parse(commandLine);

            Assert.That(arguments.ReleaseVersion, Is.EqualTo("1.2.3"));
            Assert.That(arguments.OutputPath, Is.EqualTo(@"C:\Builds\Game"));
        }

        [Test]
        public void Parse_WithoutVersion_Throws()
        {
            string[] commandLine = { "Unity.exe", "-outputPath", @"C:\Builds\Game" };

            Assert.That(() => ReleaseBuildArguments.Parse(commandLine), Throws.ArgumentException);
        }

        [Test]
        public void Parse_WithoutOutputPath_Throws()
        {
            string[] commandLine = { "Unity.exe", "-releaseVersion", "1.2.3" };

            Assert.That(() => ReleaseBuildArguments.Parse(commandLine), Throws.ArgumentException);
        }

        [Test]
        public void Parse_WhenFlagIsFollowedByAnotherFlag_Throws()
        {
            string[] commandLine = { "Unity.exe", "-releaseVersion", "-outputPath", @"C:\Builds\Game" };

            Assert.That(() => ReleaseBuildArguments.Parse(commandLine), Throws.ArgumentException);
        }

        [TestCase("1.2")]
        [TestCase("1.2.3.4")]
        [TestCase("v1.2.3")]
        [TestCase("1.2.3-beta")]
        public void Parse_WithMalformedVersion_Throws(string version)
        {
            string[] commandLine = { "Unity.exe", "-releaseVersion", version, "-outputPath", @"C:\Builds\Game" };

            Assert.That(() => ReleaseBuildArguments.Parse(commandLine), Throws.ArgumentException);
        }
    }
}
