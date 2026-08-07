// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindDialog.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for FindDialog.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo.Spreadsheet
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for FindDialog. Shown non-modally so the user can keep working in the grid
    /// while it stays open; the window's own title bar close button dismisses it.
    /// </summary>
    public partial class FindDialog : Window
    {
        private readonly SpreadsheetViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindDialog" /> class.
        /// </summary>
        public FindDialog(SpreadsheetViewModel viewModel)
        {
            this.InitializeComponent();
            this.viewModel = viewModel;
            this.Loaded += (s, e) =>
            {
                this.FindTextBox.Focus();
                this.FindTextBox.SelectAll();
            };
        }

        private void FindNext_Click(object sender, RoutedEventArgs e)
        {
            var found = this.viewModel.FindNext(this.FindTextBox.Text, this.MatchCaseCheckBox.IsChecked == true);
            this.StatusTextBlock.Text = found ? string.Empty : "No more matches found.";
        }
    }
}
