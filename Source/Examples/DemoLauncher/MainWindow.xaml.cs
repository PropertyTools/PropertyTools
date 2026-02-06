// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DemoLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string[] commandLineArgs;
        private List<ExampleInfo> allExamples;
        private ObservableCollection<ExampleInfo> filteredExamples;
        private ExampleInfo selectedExample;
        private string filterText;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow() : this(Array.Empty<string>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public MainWindow(string[] args)
        {
            this.commandLineArgs = args;
            this.InitializeComponent();
            this.DataContext = this;
            this.Loaded += MainWindow_Loaded;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the filtered examples.
        /// </summary>
        public ObservableCollection<ExampleInfo> FilteredExamples
        {
            get => this.filteredExamples;
            set
            {
                this.filteredExamples = value;
                this.OnPropertyChanged();
                this.OnPropertyChanged(nameof(StatusText));
            }
        }

        /// <summary>
        /// Gets or sets the selected example.
        /// </summary>
        public ExampleInfo SelectedExample
        {
            get => this.selectedExample;
            set
            {
                this.selectedExample = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the filter text.
        /// </summary>
        public string FilterText
        {
            get => this.filterText;
            set
            {
                this.filterText = value;
                this.OnPropertyChanged();
                this.ApplyFilter();
            }
        }

        /// <summary>
        /// Gets the status text.
        /// </summary>
        public string StatusText
        {
            get
            {
                var filteredCount = this.FilteredExamples?.Count ?? 0;
                var totalCount = this.allExamples?.Count ?? 0;
                
                if (string.IsNullOrWhiteSpace(this.FilterText))
                {
                    return $"Showing {totalCount} example(s)";
                }
                else
                {
                    return $"Showing {filteredCount} of {totalCount} example(s)";
                }
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.LoadExamples();
            this.ProcessCommandLineArguments();
        }

        private void LoadExamples()
        {
            try
            {
                this.allExamples = ExampleDiscovery.DiscoverExamples();
                this.FilteredExamples = new ObservableCollection<ExampleInfo>(this.allExamples);
                
                if (this.FilteredExamples.Count > 0)
                {
                    this.SelectedExample = this.FilteredExamples[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading examples: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ProcessCommandLineArguments()
        {
            if (this.commandLineArgs == null || this.commandLineArgs.Length == 0)
            {
                return;
            }

            var firstArg = this.commandLineArgs[0];

            // Check for capture mode: --capture [outputFolder]
            if (firstArg.Equals("--capture", StringComparison.OrdinalIgnoreCase))
            {
                // If there's a second argument, use it as the output folder
                var outputFolder = this.commandLineArgs.Length > 1 ? this.commandLineArgs[1] : null;
                this.CaptureAllExamples(outputFolder);
                return;
            }

            // Check for: ExampleName --capture [filename]
            if (this.commandLineArgs.Length >= 2 && 
                this.commandLineArgs[1].Equals("--capture", StringComparison.OrdinalIgnoreCase))
            {
                var exampleName = firstArg;
                var outputFilename = this.commandLineArgs.Length > 2 ? this.commandLineArgs[2] : null;
                this.CaptureSingleExample(exampleName, outputFilename);
                return;
            }

            // Try to find and launch the specified example (by title, type name, or full type name)
            var example = this.allExamples?.FirstOrDefault(e => 
                e.Title.Equals(firstArg, StringComparison.OrdinalIgnoreCase) ||
                e.Type.Name.Equals(firstArg, StringComparison.OrdinalIgnoreCase) ||
                e.Type.FullName.Equals(firstArg, StringComparison.OrdinalIgnoreCase));

            if (example != null)
            {
                example.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show($"Example '{firstArg}' not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ApplyFilter()
        {
            if (this.allExamples == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(this.FilterText))
            {
                this.FilteredExamples = new ObservableCollection<ExampleInfo>(this.allExamples);
            }
            else
            {
                var filter = this.FilterText.ToLowerInvariant();
                var filtered = this.allExamples.Where(e =>
                    e.Title.ToLowerInvariant().Contains(filter) ||
                    (e.Description != null && e.Description.ToLowerInvariant().Contains(filter)) ||
                    e.Tags.Any(t => t.ToLowerInvariant().Contains(filter)) ||
                    e.AssemblyName.ToLowerInvariant().Contains(filter)).ToList();

                this.FilteredExamples = new ObservableCollection<ExampleInfo>(filtered);
            }

            // Try to keep selection or select first item
            if (this.FilteredExamples.Contains(this.SelectedExample))
            {
                // Keep current selection
            }
            else if (this.FilteredExamples.Count > 0)
            {
                this.SelectedExample = this.FilteredExamples[0];
            }
            else
            {
                this.SelectedExample = null;
            }
        }

        private void LaunchSelectedExample()
        {
            if (this.SelectedExample != null)
            {
                try
                {
                    this.SelectedExample.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error launching example: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CaptureAllExamples(string outputFolder = null)
        {
            if (this.allExamples == null || this.allExamples.Count == 0)
            {
                MessageBox.Show("No examples found to capture.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                return;
            }

            // Use provided folder or default to "Screenshots"
            var outputDir = string.IsNullOrWhiteSpace(outputFolder)
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots")
                : Path.IsPathRooted(outputFolder)
                    ? outputFolder
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outputFolder);

            Directory.CreateDirectory(outputDir);

            var capturedCount = 0;
            foreach (var example in this.allExamples)
            {
                try
                {
                    var window = example.CreateInstance();
                    window.Show();
                    
                    // Allow window to render using async/await approach
                    window.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);
                    window.Dispatcher.Invoke(async () =>
                    {
                        await System.Threading.Tasks.Task.Delay(500);
                    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);

                    // Capture screenshot
                    var fileName = $"{example.AssemblyName}_{example.Type.Name}.png";
                    var filePath = Path.Combine(outputDir, fileName);
                    CaptureWindow(window, filePath);

                    window.Close();
                    capturedCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error capturing {example.Title}: {ex.Message}");
                }
            }

            MessageBox.Show($"Captured {capturedCount} example(s) to {outputDir}", "Capture Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void CaptureSingleExample(string exampleName, string outputFilename = null)
        {
            // Find the example by title, type name, or full type name
            var example = this.allExamples?.FirstOrDefault(e =>
                e.Title.Equals(exampleName, StringComparison.OrdinalIgnoreCase) ||
                e.Type.Name.Equals(exampleName, StringComparison.OrdinalIgnoreCase) ||
                e.Type.FullName.Equals(exampleName, StringComparison.OrdinalIgnoreCase));

            if (example == null)
            {
                MessageBox.Show($"Example '{exampleName}' not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
                return;
            }

            try
            {
                var window = example.CreateInstance();
                window.Show();

                // Allow window to render
                window.Dispatcher.Invoke(() => { }, System.Windows.Threading.DispatcherPriority.Render);
                window.Dispatcher.Invoke(async () =>
                {
                    await System.Threading.Tasks.Task.Delay(500);
                }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);

                // Determine output filename
                string filePath;
                if (string.IsNullOrWhiteSpace(outputFilename))
                {
                    // Default filename
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{example.Type.Name}.png");
                }
                else if (Path.IsPathRooted(outputFilename))
                {
                    // Absolute path provided
                    filePath = outputFilename;
                }
                else
                {
                    // Relative path provided
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, outputFilename);
                }

                // Ensure directory exists
                var directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                CaptureWindow(window, filePath);
                window.Close();

                MessageBox.Show($"Captured example to {filePath}", "Capture Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error capturing example: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.Close();
        }

        private static void CaptureWindow(Window window, string filePath)
        {
            var bounds = new Rect(window.RenderSize);
            var renderBitmap = new RenderTargetBitmap(
                (int)bounds.Width,
                (int)bounds.Height,
                96.0,
                96.0,
                PixelFormats.Default);

            var visual = new DrawingVisual();
            using (var context = visual.RenderOpen())
            {
                var brush = new VisualBrush(window);
                context.DrawRectangle(brush, null, bounds);
            }

            renderBitmap.Render(visual);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

            using (var stream = File.Create(filePath))
            {
                encoder.Save(stream);
            }
        }

        private void LaunchButton_Click(object sender, RoutedEventArgs e)
        {
            this.LaunchSelectedExample();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            this.LoadExamples();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ExamplesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.LaunchSelectedExample();
        }

        private void ExamplesListBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.LaunchSelectedExample();
                e.Handled = true;
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Converts a list of tags to a string for display.
    /// </summary>
    public class TagsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<string> tags && tags.Any())
            {
                return "Tags: " + string.Join(", ", tags);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts null to boolean for binding.
    /// </summary>
    public class NullToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts empty string to Visibility.
    /// </summary>
    public class EmptyStringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.IsNullOrWhiteSpace(value as string) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts empty collection to Visibility.
    /// </summary>
    public class EmptyCollectionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<string> collection)
            {
                return collection.Any() ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
