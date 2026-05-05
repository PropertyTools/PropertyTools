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
    /// The fix in TreeListBox.InsertItem uses non-localized exception signals (TargetSite.Name
    /// and StackTrace) so it works regardless of the system language.
    /// </summary>
    public partial class NonEnglishCultureWindow : Window
    {
        private readonly CultureInfo originalUICulture;

        public NonEnglishCultureWindow()
        {
            this.originalUICulture = Thread.CurrentThread.CurrentUICulture;

            // Simulate a non-English system by switching the thread UI culture to German.
            // The culture is restored in OnClosed (normal path) or via the try/catch below
            // if construction fails, so the UI thread culture is never left in the wrong state.
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");

            try
            {
                InitializeComponent();
                DataContext = new MainViewModel();
            }
            catch
            {
                Thread.CurrentThread.CurrentUICulture = this.originalUICulture;
                throw;
            }
        }

        protected override void OnClosed(System.EventArgs e)
        {
            base.OnClosed(e);

            // Restore the original culture when the window is closed.
            Thread.CurrentThread.CurrentUICulture = this.originalUICulture;
        }
    }
}
