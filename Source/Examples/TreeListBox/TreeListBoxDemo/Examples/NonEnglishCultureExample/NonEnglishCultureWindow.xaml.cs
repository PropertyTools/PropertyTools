// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NonEnglishCultureWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Globalization;
using System.Threading;
using System.Windows;

namespace TreeListBoxDemo.Examples.NonEnglishCultureExample
{
    /// <summary>
    /// Interaction logic for NonEnglishCultureWindow.xaml.
    /// Demonstrates that the TreeListBox "Height must be non-negative" workaround (#38, #142)
    /// works correctly even when the thread UI culture is set to a non-English locale.
    /// The fix in TreeListBox.InsertItem temporarily switches to InvariantCulture before
    /// calling Items.Insert so that the ArgumentException message is always in English,
    /// enabling a reliable message comparison regardless of the system language.
    /// </summary>
    public partial class NonEnglishCultureWindow : Window
    {
        private readonly CultureInfo originalUICulture;

        public NonEnglishCultureWindow()
        {
            this.originalUICulture = Thread.CurrentThread.CurrentUICulture;

            // Simulate a non-English system by switching the thread UI culture to German.
            // On a real German Windows installation the runtime resource strings (including
            // WPF exception messages) would be in German, which broke the old message-based check.
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");

            InitializeComponent();
            DataContext = new MainViewModel();
        }

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);

            // Restore the original culture when the window is closed.
            Thread.CurrentThread.CurrentUICulture = this.originalUICulture;
        }
    }
}
