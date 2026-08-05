// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindReplaceDialog.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for FindReplaceDialog.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo.Spreadsheet
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for FindReplaceDialog. A non-modal dialog is deliberately not used here —
    /// simplicity over a "stays open while you keep working" workflow.
    /// </summary>
    public partial class FindReplaceDialog : Window
    {
        private readonly SpreadsheetViewModel viewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindReplaceDialog" /> class.
        /// </summary>
        public FindReplaceDialog(SpreadsheetViewModel viewModel)
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

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.ReplaceInCurrentCell(this.FindTextBox.Text, this.ReplaceTextBox.Text, this.MatchCaseCheckBox.IsChecked == true);
            this.FindNext_Click(sender, e);
        }

        private void ReplaceAll_Click(object sender, RoutedEventArgs e)
        {
            var count = this.viewModel.ReplaceAll(this.FindTextBox.Text, this.ReplaceTextBox.Text, this.MatchCaseCheckBox.IsChecked == true);
            this.StatusTextBlock.Text = $"Replaced {count} occurrence(s).";
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
