// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DemoLauncherUITests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.UITests
{
    using System;
    using System.IO;

    using FlaUI.Core;
    using FlaUI.Core.AutomationElements;
    using FlaUI.UIA3;

    using NUnit.Framework;

    /// <summary>
    /// End-to-end UI automation tests that drive the DemoLauncher application through
    /// UI Automation (FlaUI/UIA3). These tests require a Windows desktop session and are
    /// run from the scheduled ui-tests workflow, not from the regular CI test run.
    /// </summary>
    [TestFixture]
    [Category("E2E")]
    public class DemoLauncherUITests
    {
        private Application application;
        private UIA3Automation automation;

        [SetUp]
        public void SetUp()
        {
            this.application = Application.Launch(GetDemoLauncherPath());
            this.automation = new UIA3Automation();
        }

        [TearDown]
        public void TearDown()
        {
            this.automation?.Dispose();
            if (this.application != null)
            {
                this.application.Close();
                this.application.Dispose();
            }
        }

        [Test]
        public void Launch_DemoLauncher_ShowsMainWindow()
        {
            // Act
            var window = this.application.GetMainWindow(this.automation, TimeSpan.FromSeconds(30));

            // Assert
            Assert.That(window, Is.Not.Null, "The DemoLauncher main window should appear");
            Assert.That(window.Title, Does.Contain("PropertyTools Demo Launcher"));
        }

        [Test]
        public void Launch_DemoLauncher_ListsDiscoveredExamples()
        {
            // Arrange
            var window = this.application.GetMainWindow(this.automation, TimeSpan.FromSeconds(30));
            Assert.That(window, Is.Not.Null);

            // Act
            var listBox = window.FindFirstDescendant(cf => cf.ByAutomationId("ExamplesListBox"))?.AsListBox();

            // Assert
            Assert.That(listBox, Is.Not.Null, "The examples list should exist");
            Assert.That(listBox.Items.Length, Is.GreaterThan(0), "The examples list should contain discovered examples");
        }

        /// <summary>
        /// Gets the path of the DemoLauncher executable.
        /// Can be overridden with the PROPERTYTOOLS_DEMOLAUNCHER_PATH environment variable.
        /// </summary>
        /// <returns>The full path of DemoLauncher.exe.</returns>
        private static string GetDemoLauncherPath()
        {
            var overridePath = Environment.GetEnvironmentVariable("PROPERTYTOOLS_DEMOLAUNCHER_PATH");
            if (!string.IsNullOrEmpty(overridePath))
            {
                return overridePath;
            }

            var testDirectory = AppContext.BaseDirectory;
            foreach (var configuration in new[] { "Release", "Debug" })
            {
                var candidate = Path.GetFullPath(
                    Path.Combine(
                        testDirectory,
                        "..", "..", "..", "..",
                        "Examples", "DemoLauncher", "bin", configuration, "net10.0-windows", "DemoLauncher.exe"));
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            throw new FileNotFoundException(
                "DemoLauncher.exe was not found. Build Source/Examples/DemoLauncher/DemoLauncher.csproj first or set the PROPERTYTOOLS_DEMOLAUNCHER_PATH environment variable.");
        }
    }
}
