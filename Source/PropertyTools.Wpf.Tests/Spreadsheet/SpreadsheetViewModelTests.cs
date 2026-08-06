// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpreadsheetViewModelTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.IO;

    using SpreadsheetDemo.Spreadsheet;
    using SpreadsheetDemo.Spreadsheet.Model;

    using NUnit.Framework;

    using PropertyTools.Wpf;

    [TestFixture]
    public class SpreadsheetViewModelTests
    {
        [Test]
        public void Constructor_Default_Creates100By26Sheet()
        {
            var viewModel = new SpreadsheetViewModel();

            Assert.That(viewModel.Sheet.RowCount, Is.EqualTo(100));
            Assert.That(viewModel.Sheet.ColumnCount, Is.EqualTo(26));
            Assert.That(viewModel.Rows.Count, Is.EqualTo(100));
        }

        [Test]
        public void RowHeaders_MatchOneBasedRowNumbers()
        {
            var viewModel = new SpreadsheetViewModel(new Sheet("Sheet1", 3, 3));

            Assert.That(viewModel.RowHeaders, Is.EqualTo(new[] { "1", "2", "3" }));
        }

        [Test]
        public void ColumnHeaders_MatchColumnNames()
        {
            var viewModel = new SpreadsheetViewModel(new Sheet("Sheet1", 3, 28));

            Assert.That(viewModel.ColumnHeaders[0], Is.EqualTo("A"));
            Assert.That(viewModel.ColumnHeaders[25], Is.EqualTo("Z"));
            Assert.That(viewModel.ColumnHeaders[26], Is.EqualTo("AA"));
            Assert.That(viewModel.ColumnHeaders[27], Is.EqualTo("AB"));
        }

        [Test]
        public void ControlFactory_IsSpreadsheetControlFactory()
        {
            var viewModel = new SpreadsheetViewModel();

            Assert.That(viewModel.ControlFactory, Is.InstanceOf<SpreadsheetControlFactory>());
        }

        [Test]
        public void CurrentCell_Default_IsA1()
        {
            var viewModel = new SpreadsheetViewModel(new Sheet("Sheet1", 5, 5));

            Assert.That(viewModel.CurrentCell, Is.EqualTo(new CellRef(0, 0)));
        }

        [Test]
        public void SelectionCell_Default_IsA1()
        {
            var viewModel = new SpreadsheetViewModel(new Sheet("Sheet1", 5, 5));

            Assert.That(viewModel.SelectionCell, Is.EqualTo(new CellRef(0, 0)));
        }

        [Test]
        public void CurrentCell_Set_CollapsesSelectionCellToTheSameCell()
        {
            var viewModel = new SpreadsheetViewModel(new Sheet("Sheet1", 5, 5)) { SelectionCell = new CellRef(3, 3) };

            viewModel.CurrentCell = new CellRef(1, 1);

            Assert.That(viewModel.SelectionCell, Is.EqualTo(new CellRef(1, 1)));
        }

        [Test]
        public void SelectionCell_SetIndependentlyOfCurrentCell_DoesNotMoveCurrentCell()
        {
            var viewModel = new SpreadsheetViewModel(new Sheet("Sheet1", 5, 5)) { CurrentCell = new CellRef(0, 0) };

            viewModel.SelectionCell = new CellRef(2, 3);

            Assert.That(viewModel.CurrentCell, Is.EqualTo(new CellRef(0, 0)));
            Assert.That(viewModel.SelectionCell, Is.EqualTo(new CellRef(2, 3)));
        }

        [Test]
        public void CurrentCellText_Set_EditsTheUnderlyingCell()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(1, 2) };

            viewModel.CurrentCellText = "42";

            Assert.That(sheet.GetValue(new CellAddress(1, 2)), Is.EqualTo(CellValue.FromNumber(42)));
        }

        [Test]
        public void CurrentCellText_SetWithRangeSelected_EditsEveryCellInTheRange()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(1, 1) };

            viewModel.CurrentCellText = "42";

            Assert.That(sheet.GetValue(new CellAddress(0, 0)), Is.EqualTo(CellValue.FromNumber(42)));
            Assert.That(sheet.GetValue(new CellAddress(0, 1)), Is.EqualTo(CellValue.FromNumber(42)));
            Assert.That(sheet.GetValue(new CellAddress(1, 0)), Is.EqualTo(CellValue.FromNumber(42)));
            Assert.That(sheet.GetValue(new CellAddress(1, 1)), Is.EqualTo(CellValue.FromNumber(42)));
        }

        [Test]
        public void CurrentCellText_Get_ReflectsTheUnderlyingCell()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            sheet.SetCellText(new CellAddress(0, 0), "hello");
            var viewModel = new SpreadsheetViewModel(sheet);

            Assert.That(viewModel.CurrentCellText, Is.EqualTo("hello"));
        }

        [Test]
        public void CurrentCellText_AfterMovingCurrentCell_ReflectsTheNewCell()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            sheet.SetCellText(new CellAddress(0, 0), "a");
            sheet.SetCellText(new CellAddress(0, 1), "b");
            var viewModel = new SpreadsheetViewModel(sheet);

            viewModel.CurrentCell = new CellRef(0, 1);

            Assert.That(viewModel.CurrentCellText, Is.EqualTo("b"));
        }

        [Test]
        public void IsCurrentCellBold_Set_UpdatesCellStyle()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0) };

            viewModel.IsCurrentCellBold = true;

            Assert.That(sheet.GetCell(new CellAddress(0, 0)).Style.Bold, Is.True);
            Assert.That(viewModel.IsCurrentCellBold, Is.True);
        }

        [Test]
        public void SetCurrentCellAlignment_UpdatesCellStyle()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0) };

            viewModel.SetCurrentCellAlignment(CellHorizontalAlignment.Right);

            Assert.That(sheet.GetCell(new CellAddress(0, 0)).Style.HorizontalAlignment, Is.EqualTo(CellHorizontalAlignment.Right));
        }

        [Test]
        public void IsCurrentCellBold_SetWithRangeSelected_AppliesToEveryCellInTheRange()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(1, 1) };

            viewModel.IsCurrentCellBold = true;

            Assert.That(sheet.GetCell(new CellAddress(0, 0)).Style.Bold, Is.True);
            Assert.That(sheet.GetCell(new CellAddress(0, 1)).Style.Bold, Is.True);
            Assert.That(sheet.GetCell(new CellAddress(1, 0)).Style.Bold, Is.True);
            Assert.That(sheet.GetCell(new CellAddress(1, 1)).Style.Bold, Is.True);
        }

        [Test]
        public void IsCurrentCellBold_SetWithRangeSelected_PreservesEachCellsOtherStyling()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            sheet.SetCellStyle(new CellAddress(0, 1), CellStyle.Default.WithItalic(true));
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(0, 1) };

            viewModel.IsCurrentCellBold = true;

            Assert.That(sheet.GetCell(new CellAddress(0, 1)).Style.Italic, Is.True);
            Assert.That(sheet.GetCell(new CellAddress(0, 1)).Style.Bold, Is.True);
        }

        [Test]
        public void SetCurrentCellAlignment_WithRangeSelected_AppliesToEveryCellInTheRange()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(0, 2) };

            viewModel.SetCurrentCellAlignment(CellHorizontalAlignment.Right);

            Assert.That(sheet.GetCell(new CellAddress(0, 0)).Style.HorizontalAlignment, Is.EqualTo(CellHorizontalAlignment.Right));
            Assert.That(sheet.GetCell(new CellAddress(0, 1)).Style.HorizontalAlignment, Is.EqualTo(CellHorizontalAlignment.Right));
            Assert.That(sheet.GetCell(new CellAddress(0, 2)).Style.HorizontalAlignment, Is.EqualTo(CellHorizontalAlignment.Right));
        }

        [Test]
        public void SelectionReferenceText_SingleCellSelected_ReturnsA1StyleReference()
        {
            var viewModel = new SpreadsheetViewModel(new Sheet("Sheet1", 5, 5)) { CurrentCell = new CellRef(1, 1) };

            Assert.That(viewModel.SelectionReferenceText, Is.EqualTo("B2"));
        }

        [Test]
        public void SelectionReferenceText_RangeSelected_ReturnsRangeReference()
        {
            var viewModel = new SpreadsheetViewModel(new Sheet("Sheet1", 5, 5))
            {
                CurrentCell = new CellRef(0, 0),
                SelectionCell = new CellRef(1, 1)
            };

            Assert.That(viewModel.SelectionReferenceText, Is.EqualTo("A1:B2"));
        }

        [Test]
        public void SelectionReferenceText_SetToSingleCell_MovesSelectionThere()
        {
            var viewModel = new SpreadsheetViewModel(new Sheet("Sheet1", 5, 5));

            viewModel.SelectionReferenceText = "C3";

            Assert.That(viewModel.CurrentCell, Is.EqualTo(new CellRef(2, 2)));
            Assert.That(viewModel.SelectionCell, Is.EqualTo(new CellRef(2, 2)));
        }

        [Test]
        public void SelectionReferenceText_SetToRange_SelectsTheWholeRange()
        {
            var viewModel = new SpreadsheetViewModel(new Sheet("Sheet1", 5, 5));

            viewModel.SelectionReferenceText = "A1:B2";

            Assert.That(viewModel.CurrentCell, Is.EqualTo(new CellRef(0, 0)));
            Assert.That(viewModel.SelectionCell, Is.EqualTo(new CellRef(1, 1)));
        }

        [Test]
        public void SelectionReferenceText_SetToMalformedText_ThrowsFormatExceptionAndLeavesSelectionUnchanged()
        {
            var viewModel = new SpreadsheetViewModel(new Sheet("Sheet1", 5, 5)) { CurrentCell = new CellRef(0, 0) };

            Assert.That(() => viewModel.SelectionReferenceText = "not a reference", Throws.TypeOf<FormatException>());
            Assert.That(viewModel.CurrentCell, Is.EqualTo(new CellRef(0, 0)));
        }

        [Test]
        public void SelectionReferenceText_SetOutsideSheetBounds_ThrowsArgumentOutOfRangeException()
        {
            var viewModel = new SpreadsheetViewModel(new Sheet("Sheet1", 5, 5)) { CurrentCell = new CellRef(0, 0) };

            Assert.That(() => viewModel.SelectionReferenceText = "Z99", Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(viewModel.CurrentCell, Is.EqualTo(new CellRef(0, 0)));
        }

        [Test]
        public void InsertSum_SingleCellSelected_DoesNothingAndReturnsFalse()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0) };

            var inserted = viewModel.InsertSum();

            Assert.That(inserted, Is.False);
        }

        [Test]
        public void InsertSum_TallerThanWideRange_InsertsSumBelow()
        {
            var sheet = new Sheet("Sheet1", 10, 10) { Culture = System.Globalization.CultureInfo.InvariantCulture };
            sheet.SetCellText(new CellAddress(0, 0), "1");
            sheet.SetCellText(new CellAddress(1, 0), "2");
            sheet.SetCellText(new CellAddress(2, 0), "3");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(2, 0) };

            var inserted = viewModel.InsertSum();

            Assert.That(inserted, Is.True);
            Assert.That(viewModel.CurrentCell, Is.EqualTo(new CellRef(3, 0)));
            Assert.That(sheet.GetCell(new CellAddress(3, 0)).Content.IsFormula, Is.True);
            Assert.That(sheet.GetValue(new CellAddress(3, 0)), Is.EqualTo(CellValue.FromNumber(6)));
        }

        [Test]
        public void InsertSum_WiderThanTallRange_InsertsSumToTheRight()
        {
            var sheet = new Sheet("Sheet1", 10, 10) { Culture = System.Globalization.CultureInfo.InvariantCulture };
            sheet.SetCellText(new CellAddress(0, 0), "1");
            sheet.SetCellText(new CellAddress(0, 1), "2");
            sheet.SetCellText(new CellAddress(0, 2), "3");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(0, 2) };

            var inserted = viewModel.InsertSum();

            Assert.That(inserted, Is.True);
            Assert.That(viewModel.CurrentCell, Is.EqualTo(new CellRef(0, 3)));
            Assert.That(sheet.GetValue(new CellAddress(0, 3)), Is.EqualTo(CellValue.FromNumber(6)));
        }

        [Test]
        public void InsertSum_NoRoomBelow_DoesNothingAndReturnsFalse()
        {
            var sheet = new Sheet("Sheet1", 2, 5);
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(1, 0) };

            var inserted = viewModel.InsertSum();

            Assert.That(inserted, Is.False);
        }

        [Test]
        public void InsertSum_RectangularRange_InsertsSumBelowEachColumnAndRightOfEachRow()
        {
            var sheet = new Sheet("Sheet1", 10, 10) { Culture = System.Globalization.CultureInfo.InvariantCulture };
            sheet.SetCellText(new CellAddress(0, 0), "1");
            sheet.SetCellText(new CellAddress(0, 1), "2");
            sheet.SetCellText(new CellAddress(1, 0), "3");
            sheet.SetCellText(new CellAddress(1, 1), "4");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(1, 1) };

            var inserted = viewModel.InsertSum();

            Assert.That(inserted, Is.True);
            // Below each column (row 2): column 0 = 1+3 = 4, column 1 = 2+4 = 6.
            Assert.That(sheet.GetValue(new CellAddress(2, 0)), Is.EqualTo(CellValue.FromNumber(4)));
            Assert.That(sheet.GetValue(new CellAddress(2, 1)), Is.EqualTo(CellValue.FromNumber(6)));
            // To the right of each row (column 2): row 0 = 1+2 = 3, row 1 = 3+4 = 7.
            Assert.That(sheet.GetValue(new CellAddress(0, 2)), Is.EqualTo(CellValue.FromNumber(3)));
            Assert.That(sheet.GetValue(new CellAddress(1, 2)), Is.EqualTo(CellValue.FromNumber(7)));
        }

        [Test]
        public void InsertSum_RectangularRangeInsertingMultipleSums_DoesNotMoveCurrentCell()
        {
            var sheet = new Sheet("Sheet1", 10, 10);
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(1, 1) };

            viewModel.InsertSum();

            Assert.That(viewModel.CurrentCell, Is.EqualTo(new CellRef(0, 0)));
        }

        [Test]
        public void IncreaseDecimalPlaces_FromGeneral_SetsOneDecimalPlace()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0) };

            viewModel.IncreaseDecimalPlaces();

            Assert.That(sheet.GetCell(new CellAddress(0, 0)).Style.FormatString, Is.EqualTo("0.0"));
        }

        [Test]
        public void IncreaseDecimalPlaces_AppliesAcrossTheWholeSelection()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(0, 1) };

            viewModel.IncreaseDecimalPlaces();

            Assert.That(sheet.GetCell(new CellAddress(0, 0)).Style.FormatString, Is.EqualTo("0.0"));
            Assert.That(sheet.GetCell(new CellAddress(0, 1)).Style.FormatString, Is.EqualTo("0.0"));
        }

        [Test]
        public void DecreaseDecimalPlaces_FromTwoDecimals_RemovesOneDigit()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            sheet.SetCellStyle(new CellAddress(0, 0), CellStyle.Default.WithFormat("0.00"));
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0) };

            viewModel.DecreaseDecimalPlaces();

            Assert.That(sheet.GetCell(new CellAddress(0, 0)).Style.FormatString, Is.EqualTo("0.0"));
        }

        [Test]
        public void DecreaseDecimalPlaces_FromZeroDecimals_StaysAtZero()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0) };

            viewModel.DecreaseDecimalPlaces();

            Assert.That(sheet.GetCell(new CellAddress(0, 0)).Style.FormatString, Is.EqualTo("0"));
        }

        [Test]
        public void SetCurrentCellFormat_AppliesToSelection()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(1, 0) };

            viewModel.SetCurrentCellFormat("yyyy-MM-dd");

            Assert.That(sheet.GetCell(new CellAddress(0, 0)).Style.FormatString, Is.EqualTo("yyyy-MM-dd"));
            Assert.That(sheet.GetCell(new CellAddress(1, 0)).Style.FormatString, Is.EqualTo("yyyy-MM-dd"));
        }

        [Test]
        public void SetCurrentCellFormat_DurationPreset_FormatsDisplayText()
        {
            var sheet = new Sheet("Sheet1", 5, 5) { Culture = System.Globalization.CultureInfo.InvariantCulture };
            sheet.SetCellText(new CellAddress(0, 0), "1:05:03");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0) };

            viewModel.SetCurrentCellFormat("m:ss");

            Assert.That(sheet.GetCell(new CellAddress(0, 0)).DisplayText, Is.EqualTo("65:03"));
        }

        [Test]
        public void SortSelection_SingleColumnAscending_SortsValues()
        {
            var sheet = new Sheet("Sheet1", 5, 5) { Culture = System.Globalization.CultureInfo.InvariantCulture };
            sheet.SetCellText(new CellAddress(0, 0), "3");
            sheet.SetCellText(new CellAddress(1, 0), "1");
            sheet.SetCellText(new CellAddress(2, 0), "2");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(2, 0) };

            viewModel.SortSelection(ascending: true);

            Assert.That(sheet.GetValue(new CellAddress(0, 0)), Is.EqualTo(CellValue.FromNumber(1)));
            Assert.That(sheet.GetValue(new CellAddress(1, 0)), Is.EqualTo(CellValue.FromNumber(2)));
            Assert.That(sheet.GetValue(new CellAddress(2, 0)), Is.EqualTo(CellValue.FromNumber(3)));
        }

        [Test]
        public void SortSelection_SingleColumnDescending_SortsValuesInReverse()
        {
            var sheet = new Sheet("Sheet1", 5, 5) { Culture = System.Globalization.CultureInfo.InvariantCulture };
            sheet.SetCellText(new CellAddress(0, 0), "1");
            sheet.SetCellText(new CellAddress(1, 0), "3");
            sheet.SetCellText(new CellAddress(2, 0), "2");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(2, 0) };

            viewModel.SortSelection(ascending: false);

            Assert.That(sheet.GetValue(new CellAddress(0, 0)), Is.EqualTo(CellValue.FromNumber(3)));
            Assert.That(sheet.GetValue(new CellAddress(1, 0)), Is.EqualTo(CellValue.FromNumber(2)));
            Assert.That(sheet.GetValue(new CellAddress(2, 0)), Is.EqualTo(CellValue.FromNumber(1)));
        }

        [Test]
        public void SortSelection_MultiColumnRange_KeepsRowsTogetherKeyedByLeftmostColumn()
        {
            var sheet = new Sheet("Sheet1", 5, 5) { Culture = System.Globalization.CultureInfo.InvariantCulture };
            sheet.SetCellText(new CellAddress(0, 0), "b");
            sheet.SetCellText(new CellAddress(0, 1), "2");
            sheet.SetCellText(new CellAddress(1, 0), "a");
            sheet.SetCellText(new CellAddress(1, 1), "1");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(1, 1) };

            viewModel.SortSelection(ascending: true);

            Assert.That(sheet.GetValue(new CellAddress(0, 0)), Is.EqualTo(CellValue.FromText("a")));
            Assert.That(sheet.GetValue(new CellAddress(0, 1)), Is.EqualTo(CellValue.FromNumber(1)));
            Assert.That(sheet.GetValue(new CellAddress(1, 0)), Is.EqualTo(CellValue.FromText("b")));
            Assert.That(sheet.GetValue(new CellAddress(1, 1)), Is.EqualTo(CellValue.FromNumber(2)));
        }

        [Test]
        public void SortSelection_SingleWideRow_SortsCellsLeftToRight()
        {
            var sheet = new Sheet("Sheet1", 5, 5) { Culture = System.Globalization.CultureInfo.InvariantCulture };
            sheet.SetCellText(new CellAddress(0, 0), "3");
            sheet.SetCellText(new CellAddress(0, 1), "1");
            sheet.SetCellText(new CellAddress(0, 2), "2");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(0, 2) };

            viewModel.SortSelection(ascending: true);

            Assert.That(sheet.GetValue(new CellAddress(0, 0)), Is.EqualTo(CellValue.FromNumber(1)));
            Assert.That(sheet.GetValue(new CellAddress(0, 1)), Is.EqualTo(CellValue.FromNumber(2)));
            Assert.That(sheet.GetValue(new CellAddress(0, 2)), Is.EqualTo(CellValue.FromNumber(3)));
        }

        [Test]
        public void SortSelection_SingleCellSelected_DoesNothing()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            sheet.SetCellText(new CellAddress(0, 0), "5");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0) };

            Assert.That(() => viewModel.SortSelection(ascending: true), Throws.Nothing);
            Assert.That(sheet.GetValue(new CellAddress(0, 0)), Is.EqualTo(CellValue.FromNumber(5)));
        }

        [Test]
        public void FindNext_TextOnlyInFormula_FindsTheCell()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            sheet.SetCellText(new CellAddress(2, 0), "=A1+1");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0) };

            var found = viewModel.FindNext("A1+1", matchCase: false);

            Assert.That(found, Is.True);
            Assert.That(viewModel.CurrentCell, Is.EqualTo(new CellRef(2, 0)));
        }

        [Test]
        public void FindNext_MatchAfterCurrentCell_MovesCurrentCellAndReturnsTrue()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            sheet.SetCellText(new CellAddress(2, 0), "needle");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0) };

            var found = viewModel.FindNext("needle", matchCase: false);

            Assert.That(found, Is.True);
            Assert.That(viewModel.CurrentCell, Is.EqualTo(new CellRef(2, 0)));
        }

        [Test]
        public void FindNext_MatchWithAPreviousRangeSelected_CollapsesSelectionToTheFoundCell()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            sheet.SetCellText(new CellAddress(2, 0), "needle");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0), SelectionCell = new CellRef(4, 4) };

            viewModel.FindNext("needle", matchCase: false);

            Assert.That(viewModel.SelectionCell, Is.EqualTo(new CellRef(2, 0)));
        }

        [Test]
        public void FindNext_NoMatch_ReturnsFalseAndDoesNotMoveCurrentCell()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(1, 1) };

            var found = viewModel.FindNext("needle", matchCase: false);

            Assert.That(found, Is.False);
            Assert.That(viewModel.CurrentCell, Is.EqualTo(new CellRef(1, 1)));
        }

        [Test]
        public void FindNext_WrapsAroundFromLastCellToFirst()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            sheet.SetCellText(new CellAddress(0, 0), "needle");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(4, 4) };

            var found = viewModel.FindNext("needle", matchCase: false);

            Assert.That(found, Is.True);
            Assert.That(viewModel.CurrentCell, Is.EqualTo(new CellRef(0, 0)));
        }

        [Test]
        public void ReplaceInCurrentCell_MatchingText_ReplacesAndReturnsTrue()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            sheet.SetCellText(new CellAddress(0, 0), "hello world");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0) };

            var replaced = viewModel.ReplaceInCurrentCell("world", "there", matchCase: false);

            Assert.That(replaced, Is.True);
            Assert.That(sheet.GetValue(new CellAddress(0, 0)).AsText(), Is.EqualTo("hello there"));
        }

        [Test]
        public void ReplaceInCurrentCell_FormulaCell_DoesNotReplace()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            sheet.SetCellText(new CellAddress(0, 0), "=A1");
            var viewModel = new SpreadsheetViewModel(sheet) { CurrentCell = new CellRef(0, 0) };

            var replaced = viewModel.ReplaceInCurrentCell("A1", "B1", matchCase: false);

            Assert.That(replaced, Is.False);
        }

        [Test]
        public void ReplaceAll_MultipleCells_ReplacesInEachAndReturnsCount()
        {
            var sheet = new Sheet("Sheet1", 5, 5);
            sheet.SetCellText(new CellAddress(0, 0), "cat");
            sheet.SetCellText(new CellAddress(1, 0), "category");
            sheet.SetCellText(new CellAddress(2, 0), "dog");
            var viewModel = new SpreadsheetViewModel(sheet);

            var count = viewModel.ReplaceAll("cat", "hat", matchCase: false);

            Assert.That(count, Is.EqualTo(2));
            Assert.That(sheet.GetValue(new CellAddress(0, 0)).AsText(), Is.EqualTo("hat"));
            Assert.That(sheet.GetValue(new CellAddress(1, 0)).AsText(), Is.EqualTo("hategory"));
        }

        [Test]
        public void New_ReplacesSheetWithFreshOneAndClearsModifiedFlag()
        {
            var viewModel = new SpreadsheetViewModel();
            viewModel.CurrentCellText = "42";
            Assert.That(viewModel.Workbook.IsModified, Is.True);
            var originalSheet = viewModel.Sheet;

            viewModel.New();

            Assert.That(viewModel.Sheet, Is.Not.SameAs(originalSheet));
            Assert.That(viewModel.Workbook.IsModified, Is.False);
            Assert.That(viewModel.CurrentCellText, Is.Empty);
        }

        [Test]
        public void EditingCurrentCellText_MarksWorkbookModified()
        {
            var viewModel = new SpreadsheetViewModel();

            viewModel.CurrentCellText = "42";

            Assert.That(viewModel.Workbook.IsModified, Is.True);
        }

        [Test]
        public void ExportCsv_ThenImportCsv_RoundTripsValuesAndDoesNotSetFilePath()
        {
            var path = Path.GetTempFileName();
            try
            {
                var viewModel = new SpreadsheetViewModel { CurrentCell = new CellRef(0, 0) };
                viewModel.CurrentCellText = "hello";

                viewModel.ExportCsv(path);
                Assert.That(viewModel.Workbook.FilePath, Is.Null);

                viewModel.ImportCsv(path);

                Assert.That(viewModel.Workbook.FilePath, Is.Null);
                Assert.That(viewModel.Sheet.GetValue(new CellAddress(0, 0)).AsText(), Is.EqualTo("hello"));
            }
            finally
            {
                File.Delete(path);
            }
        }
    }
}
