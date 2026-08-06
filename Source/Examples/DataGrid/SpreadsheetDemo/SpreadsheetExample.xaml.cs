// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpreadsheetExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for SpreadsheetExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SpreadsheetDemo
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using SpreadsheetDemo.Spreadsheet;
    using SpreadsheetDemo.Spreadsheet.Model;

    using Microsoft.Win32;

    /// <summary>
    /// Interaction logic for SpreadsheetExample.
    /// </summary>
    public partial class SpreadsheetExample
    {
        /// <summary>
        /// Bound to Ctrl+B, toggling <see cref="SpreadsheetViewModel.IsCurrentCellBold" />.
        /// </summary>
        public static readonly RoutedCommand BoldCommand = new RoutedCommand();

        /// <summary>
        /// Bound to Ctrl+I, toggling <see cref="SpreadsheetViewModel.IsCurrentCellItalic" />.
        /// </summary>
        public static readonly RoutedCommand ItalicCommand = new RoutedCommand();

        /// <summary>
        /// Bound to Ctrl+L, the Format menu's "Align Left".
        /// </summary>
        public static readonly RoutedCommand AlignLeftCommand = new RoutedCommand();

        /// <summary>
        /// Bound to Ctrl+E (Excel's convention, since Ctrl+C is already Copy), the Format menu's
        /// "Align Center".
        /// </summary>
        public static readonly RoutedCommand AlignCenterCommand = new RoutedCommand();

        /// <summary>
        /// Bound to Ctrl+R, the Format menu's "Align Right".
        /// </summary>
        public static readonly RoutedCommand AlignRightCommand = new RoutedCommand();

        /// <summary>
        /// Bound to Ctrl+], the Format menu's "Increase Decimal".
        /// </summary>
        public static readonly RoutedCommand IncreaseDecimalCommand = new RoutedCommand();

        /// <summary>
        /// Bound to Ctrl+[, the Format menu's "Decrease Decimal".
        /// </summary>
        public static readonly RoutedCommand DecreaseDecimalCommand = new RoutedCommand();

        /// <summary>
        /// Bound to Ctrl+Shift+A, the Data menu's "Sort Ascending". Excel has no fixed default
        /// shortcut for sorting (it's a ribbon/menu action there), so this is this app's own choice.
        /// </summary>
        public static readonly RoutedCommand SortAscendingCommand = new RoutedCommand();

        /// <summary>
        /// Bound to Ctrl+Shift+D, the Data menu's "Sort Descending".
        /// </summary>
        public static readonly RoutedCommand SortDescendingCommand = new RoutedCommand();

        private const string FileFilter = "Spreadsheet files (*.ptsheet)|*.ptsheet|All files (*.*)|*.*";

        private const string CsvFileFilter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

        /// <summary>
        /// The default column width, in pixels. Set in code (not XAML) because
        /// <see cref="System.Windows.GridLength" /> has no simple constant syntax, and applied before
        /// <see cref="DataContext" /> is set so it's in place before the grid's <c>ItemsSource</c>
        /// binding resolves and generates columns from it (see
        /// <see cref="Spreadsheet.SpreadsheetDataGridOperator" /> for why the grid's own
        /// <c>DefaultColumnWidth</c> otherwise has no effect).
        /// </summary>
        private const double DefaultColumnWidth = 140;

        private readonly SpreadsheetViewModel viewModel = new SpreadsheetViewModel();

        private FindDialog findDialog;

        private ReplaceDialog replaceDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetExample" /> class.
        /// </summary>
        public SpreadsheetExample()
        {
            this.InitializeComponent();
            this.SheetGrid.DefaultColumnWidth = new GridLength(DefaultColumnWidth);
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

        private void ImportCsv_Click(object sender, RoutedEventArgs e)
        {
            if (!this.ConfirmDiscardChanges())
            {
                return;
            }

            var dialog = new OpenFileDialog { Filter = CsvFileFilter };
            if (dialog.ShowDialog(this) != true)
            {
                return;
            }

            try
            {
                this.viewModel.ImportCsv(dialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not import the file.\n\n" + ex.Message, "Spreadsheet", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportCsv_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog { Filter = CsvFileFilter };
            if (dialog.ShowDialog(this) != true)
            {
                return;
            }

            try
            {
                this.viewModel.ExportCsv(dialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not export the file.\n\n" + ex.Message, "Spreadsheet", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void Replace_Executed(object sender, ExecutedRoutedEventArgs e)
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

        private void Bold_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.IsCurrentCellBold = !this.viewModel.IsCurrentCellBold;
        }

        private void Italic_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.IsCurrentCellItalic = !this.viewModel.IsCurrentCellItalic;
        }

        private void AlignLeft_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellAlignment(CellHorizontalAlignment.Left);
        }

        private void AlignCenter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellAlignment(CellHorizontalAlignment.Center);
        }

        private void AlignRight_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellAlignment(CellHorizontalAlignment.Right);
        }

        private void Sum_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.InsertSum();
        }

        private void IncreaseDecimal_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.IncreaseDecimalPlaces();
        }

        private void DecreaseDecimal_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.DecreaseDecimalPlaces();
        }

        private void NumberFormatGeneral_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellFormat(null);
        }

        private void NumberFormatNumber_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellFormat("0.00");
        }

        private void NumberFormatInteger_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellFormat("0");
        }

        private void DateFormatShort_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellFormat("yyyy-MM-dd");
        }

        private void DateFormatLong_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellFormat("dddd, MMMM d, yyyy");
        }

        private void DateFormatTime_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellFormat("HH:mm:ss");
        }

        private void DateFormatDateTime_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellFormat("yyyy-MM-dd HH:mm");
        }

        private void DurationFormatHms_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellFormat("h:mm:ss");
        }

        private void DurationFormatHhMmSs_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellFormat("hh:mm:ss");
        }

        private void DurationFormatMs_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellFormat("m:ss");
        }

        private void DurationFormatMmSs_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SetCurrentCellFormat("mm:ss");
        }

        private void SortAscending_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SortSelection(ascending: true);
        }

        private void SortDescending_Click(object sender, RoutedEventArgs e)
        {
            this.viewModel.SortSelection(ascending: false);
        }

        private void IncreaseDecimal_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.IncreaseDecimalPlaces();
        }

        private void DecreaseDecimal_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.DecreaseDecimalPlaces();
        }

        private void SortAscending_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.SortSelection(ascending: true);
        }

        private void SortDescending_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.viewModel.SortSelection(ascending: false);
        }

        private void NameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            // UpdateSource() catches exceptions from the SelectionReferenceText setter itself
            // (ValidatesOnExceptions="True" in the binding) and reports them as a validation error
            // instead of throwing, so an invalid reference can never crash or corrupt the selection.
            this.NameBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty)?.UpdateSource();
            if (System.Windows.Controls.Validation.GetHasError(this.NameBox))
            {
                this.NameBox.SelectAll();
                e.Handled = true;
                return;
            }

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
