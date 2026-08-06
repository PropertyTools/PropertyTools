// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpreadsheetExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for SpreadsheetExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using DataGridDemo.Spreadsheet;
    using DataGridDemo.Spreadsheet.Model;

    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for SpreadsheetExample.
    /// </summary>
    public partial class SpreadsheetExample
    {
        private const string FileFilter = "Spreadsheet files (*.ptsheet)|*.ptsheet|All files (*.*)|*.*";

        private readonly SpreadsheetViewModel viewModel = new SpreadsheetViewModel();

        private FindDialog findDialog;

        private ReplaceDialog replaceDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetExample" /> class.
        /// </summary>
        public SpreadsheetExample()
        {
            this.InitializeComponent();
            this.DataContext = this.viewModel;
            this.Closing += this.SpreadsheetExample_Closing;
        }

        /// <summary>
        /// Asks the user to discard, save, or cancel when there are unsaved changes.
        /// </summary>
        /// <returns><c>true</c> if it is OK to proceed (changes were saved or the user chose to discard them).</returns>
        private bool ConfirmDiscardChanges()
        {
            if (!this.viewModel.Workbook.IsModified)
            {
                return true;
            }

            var result = MessageBox.Show(
                this,
                "Do you want to save the changes you made?",
                "Spreadsheet",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Warning);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    return this.Save();
                case MessageBoxResult.No:
                    return true;
                default:
                    return false;
            }
        }

        private void SpreadsheetExample_Closing(object sender, CancelEventArgs e)
        {
            if (!this.ConfirmDiscardChanges())
            {
                e.Cancel = true;
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (this.ConfirmDiscardChanges())
            {
                this.viewModel.New();
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            if (!this.ConfirmDiscardChanges())
            {
                return;
            }

            var dialog = new OpenFileDialog { Filter = FileFilter };
            if (dialog.ShowDialog(this) != true)
            {
                return;
            }

            try
            {
                this.viewModel.Open(dialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not open the file.\n\n" + ex.Message, "Spreadsheet", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            this.Save();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            this.SaveAs();
        }

        /// <summary>
        /// Saves to the workbook's current file, or prompts for a file if it has none yet.
        /// </summary>
        /// <returns><c>true</c> if the save succeeded (or there was nothing to save).</returns>
        private bool Save()
        {
            return this.viewModel.Workbook.FilePath == null ? this.SaveAs() : this.TrySave(this.viewModel.Workbook.FilePath);
        }

        private bool SaveAs()
        {
            var dialog = new SaveFileDialog { Filter = FileFilter, FileName = this.viewModel.Workbook.FilePath };
            return dialog.ShowDialog(this) == true && this.TrySave(dialog.FileName);
        }

        private bool TrySave(string path)
        {
            try
            {
                this.viewModel.Save(path);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not save the file.\n\n" + ex.Message, "Spreadsheet", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void Find_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.ShowFindDialog();
        }

        private void Find_Click(object sender, RoutedEventArgs e)
        {
            this.ShowFindDialog();
        }

        private void Replace_Click(object sender, RoutedEventArgs e)
        {
            this.ShowReplaceDialog();
        }

        private void ShowFindDialog()
        {
            if (this.findDialog == null)
            {
                this.findDialog = new FindDialog(this.viewModel) { Owner = this };
                this.findDialog.Closed += (s, e) => this.findDialog = null;
                this.findDialog.Show();
            }
            else
            {
                this.findDialog.Activate();
            }
        }

        private void ShowReplaceDialog()
        {
            if (this.replaceDialog == null)
            {
                this.replaceDialog = new ReplaceDialog(this.viewModel) { Owner = this };
                this.replaceDialog.Closed += (s, e) => this.replaceDialog = null;
                this.replaceDialog.Show();
            }
            else
            {
                this.replaceDialog.Activate();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AlignLeft_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellAlignment(CellHorizontalAlignment.Left);
        }

        private void AlignCenter_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellAlignment(CellHorizontalAlignment.Center);
        }

        private void AlignRight_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellAlignment(CellHorizontalAlignment.Right);
        }

        private void NameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            this.NameBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty)?.UpdateSource();
            Keyboard.Focus(this.SheetGrid);
            e.Handled = true;
        }

        private void FormulaBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            this.FormulaBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty)?.UpdateSource();
            Keyboard.Focus(this.SheetGrid);
            e.Handled = true;
        }
    }
}
