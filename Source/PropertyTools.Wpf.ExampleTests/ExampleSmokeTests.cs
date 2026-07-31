// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleSmokeTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.ExampleTests
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Windows;

    using NUnit.Framework;

    /// <summary>
    /// Smoke tests that instantiate every example window headlessly (without showing it),
    /// apply templates and force a layout pass of the window content. This converts the
    /// manual demo applications into automated regression coverage.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [Category("WpfHeadless")]
    [Category("ExampleSmoke")]
    public class ExampleSmokeTests
    {
        /// <summary>
        /// Gets the example window types used as test cases.
        /// </summary>
        public static object[] ExampleWindowTypes => ExampleWindowSource.GetExampleWindowTypes().Cast<object>().ToArray();

        [Test]
        public void Discovery_ReferencedDemoAssemblies_FindsExamples()
        {
            // Arrange / Act
            var types = ExampleWindowSource.GetExampleWindowTypes().ToList();

            // Assert
            Assert.That(types, Is.Not.Empty, "Example windows should be discovered in the referenced demo assemblies");
        }

        [Test]
        [TestCaseSource(nameof(ExampleWindowTypes))]
        public void CreateInstance_ExampleWindow_InstantiatesAndLaysOutWithoutExceptions(Type exampleType)
        {
            // Arrange / Act
            var window = (Window)Activator.CreateInstance(exampleType);

            try
            {
                // Assert
                Assert.That(window.Content, Is.Not.Null, "The example window should have content");

                // Force a full template + layout pass of the window content without showing the window
                var content = (FrameworkElement)window.Content;
                content.ApplyTemplate();
                content.Measure(new Size(1024, 768));
                content.Arrange(new Rect(0, 0, 1024, 768));
                content.UpdateLayout();
            }
            finally
            {
                window.Close();
            }
        }
    }
}
