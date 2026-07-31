// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleSnapshotTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.ExampleTests
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using NUnit.Framework;

    /// <summary>
    /// Visual regression tests that render every example window off-screen to a bitmap
    /// and compare it against a committed baseline image (with a tolerance).
    /// When a baseline does not exist, a candidate image is written to the test output
    /// directory so it can be reviewed and committed as the new baseline.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    [Category("Visual")]
    public class ExampleSnapshotTests
    {
        /// <summary>
        /// The width used when rendering the snapshots.
        /// </summary>
        private const int SnapshotWidth = 1024;

        /// <summary>
        /// The height used when rendering the snapshots.
        /// </summary>
        private const int SnapshotHeight = 768;

        /// <summary>
        /// The per-channel difference (0-255) below which two pixels are considered equal.
        /// </summary>
        private const int ChannelTolerance = 10;

        /// <summary>
        /// The maximum fraction of differing pixels allowed before the comparison fails.
        /// </summary>
        private const double MaxDifferingPixelFraction = 0.005;

        /// <summary>
        /// Gets the example window types used as test cases.
        /// </summary>
        public static object[] ExampleWindowTypes => ExampleWindowSource.GetExampleWindowTypes().Cast<object>().ToArray();

        /// <summary>
        /// Gets the directory containing the committed baseline images.
        /// Can be overridden with the PROPERTYTOOLS_SNAPSHOT_BASELINE_DIR environment variable.
        /// </summary>
        private static string BaselineDirectory =>
            Environment.GetEnvironmentVariable("PROPERTYTOOLS_SNAPSHOT_BASELINE_DIR")
            ?? Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Snapshots"));

        /// <summary>
        /// Gets the directory where candidate/actual images are written.
        /// </summary>
        private static string OutputDirectory => Path.Combine(TestContext.CurrentContext.WorkDirectory, "Snapshots");

        [Test]
        [TestCaseSource(nameof(ExampleWindowTypes))]
        public void Render_ExampleWindow_MatchesBaselineSnapshot(Type exampleType)
        {
            // Arrange
            var window = (Window)Activator.CreateInstance(exampleType);
            byte[] actualPixels;
            try
            {
                actualPixels = RenderContentToPixels(window);
            }
            finally
            {
                window.Close();
            }

            Directory.CreateDirectory(OutputDirectory);
            var actualPath = Path.Combine(OutputDirectory, exampleType.FullName + ".png");
            WritePng(actualPath, actualPixels);
            TestContext.AddTestAttachment(actualPath, "Rendered snapshot");

            var baselinePath = Path.Combine(BaselineDirectory, exampleType.FullName + ".png");
            if (!File.Exists(baselinePath))
            {
                Assert.Inconclusive($"No baseline found at '{baselinePath}'. A candidate image was written to '{actualPath}'.");
            }

            // Act
            var baselinePixels = ReadPng(baselinePath);
            var differingFraction = GetDifferingPixelFraction(baselinePixels, actualPixels);

            // Assert
            Assert.That(
                differingFraction,
                Is.LessThanOrEqualTo(MaxDifferingPixelFraction),
                $"The rendered snapshot differs from the baseline '{baselinePath}' by {differingFraction:P2} of pixels. The actual image was written to '{actualPath}'.");
        }

        /// <summary>
        /// Renders the content of the specified window off-screen and returns the BGRA pixel buffer.
        /// </summary>
        /// <param name="window">The window to render.</param>
        /// <returns>The BGRA pixels.</returns>
        private static byte[] RenderContentToPixels(Window window)
        {
            var content = (FrameworkElement)window.Content;
            Assert.That(content, Is.Not.Null, "The example window should have content");

            content.ApplyTemplate();
            content.Measure(new Size(SnapshotWidth, SnapshotHeight));
            content.Arrange(new Rect(0, 0, SnapshotWidth, SnapshotHeight));
            content.UpdateLayout();

            var bitmap = new RenderTargetBitmap(SnapshotWidth, SnapshotHeight, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(content);

            var pixels = new byte[SnapshotWidth * SnapshotHeight * 4];
            bitmap.CopyPixels(pixels, SnapshotWidth * 4, 0);
            return pixels;
        }

        /// <summary>
        /// Writes the specified BGRA pixel buffer as a PNG file.
        /// </summary>
        /// <param name="path">The output path.</param>
        /// <param name="pixels">The BGRA pixels.</param>
        private static void WritePng(string path, byte[] pixels)
        {
            var bitmap = BitmapSource.Create(SnapshotWidth, SnapshotHeight, 96, 96, PixelFormats.Pbgra32, null, pixels, SnapshotWidth * 4);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using (var stream = File.Create(path))
            {
                encoder.Save(stream);
            }
        }

        /// <summary>
        /// Reads a PNG file into a BGRA pixel buffer of the snapshot size.
        /// </summary>
        /// <param name="path">The path of the PNG file.</param>
        /// <returns>The BGRA pixels.</returns>
        private static byte[] ReadPng(string path)
        {
            BitmapSource bitmap;
            using (var stream = File.OpenRead(path))
            {
                var decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                bitmap = decoder.Frames[0];
            }

            if (bitmap.Format != PixelFormats.Pbgra32)
            {
                bitmap = new FormatConvertedBitmap(bitmap, PixelFormats.Pbgra32, null, 0);
            }

            Assert.That(bitmap.PixelWidth, Is.EqualTo(SnapshotWidth), "Baseline image width should match the snapshot size");
            Assert.That(bitmap.PixelHeight, Is.EqualTo(SnapshotHeight), "Baseline image height should match the snapshot size");

            var pixels = new byte[SnapshotWidth * SnapshotHeight * 4];
            bitmap.CopyPixels(pixels, SnapshotWidth * 4, 0);
            return pixels;
        }

        /// <summary>
        /// Gets the fraction of pixels that differ by more than the channel tolerance.
        /// </summary>
        /// <param name="expected">The expected BGRA pixels.</param>
        /// <param name="actual">The actual BGRA pixels.</param>
        /// <returns>The fraction of differing pixels (0..1).</returns>
        private static double GetDifferingPixelFraction(byte[] expected, byte[] actual)
        {
            var totalPixels = SnapshotWidth * SnapshotHeight;
            var differingPixels = 0;
            for (var i = 0; i < expected.Length; i += 4)
            {
                if (Math.Abs(expected[i] - actual[i]) > ChannelTolerance
                    || Math.Abs(expected[i + 1] - actual[i + 1]) > ChannelTolerance
                    || Math.Abs(expected[i + 2] - actual[i + 2]) > ChannelTolerance)
                {
                    differingPixels++;
                }
            }

            return (double)differingPixels / totalPixels;
        }
    }
}
