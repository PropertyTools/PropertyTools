// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecalculationEngineTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Globalization;

    using SpreadsheetDemo.Spreadsheet.Model;

    using NUnit.Framework;

    /// <summary>
    /// Exercises <c>RecalculationEngine</c> and <c>DependencyGraph</c> together through
    /// <see cref="Sheet" />, since recalculation order and cycle detection only make sense against a
    /// real sheet of formula cells.
    /// </summary>
    [TestFixture]
    public class RecalculationEngineTests
    {
        private Sheet sheet;

        [SetUp]
        public void SetUp()
        {
            this.sheet = new Sheet("Sheet1", 20, 20) { Culture = CultureInfo.InvariantCulture };
        }

        private double GetNumber(int row, int column)
        {
            return this.sheet.GetValue(new CellAddress(row, column)).AsNumber();
        }

        [Test]
        public void Chain_ABC_EditingRootRecalculatesEveryDependent()
        {
            // A1 = 1, A2 = A1+1, A3 = A2+1: a three-cell chain down column A.
            this.sheet.SetCellText(new CellAddress(0, 0), "1");
            this.sheet.SetCellText(new CellAddress(1, 0), "=A1+1");
            this.sheet.SetCellText(new CellAddress(2, 0), "=A2+1");

            Assert.That(this.GetNumber(1, 0), Is.EqualTo(2));
            Assert.That(this.GetNumber(2, 0), Is.EqualTo(3));

            this.sheet.SetCellText(new CellAddress(0, 0), "10");

            Assert.That(this.GetNumber(1, 0), Is.EqualTo(11));
            Assert.That(this.GetNumber(2, 0), Is.EqualTo(12));
        }

        [Test]
        public void Chain_ThroughRange_RecalculatesSumWhenSourceCellChanges()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "1");
            this.sheet.SetCellText(new CellAddress(1, 0), "2");
            this.sheet.SetCellText(new CellAddress(2, 0), "=SUM(A1:A2)");

            Assert.That(this.GetNumber(2, 0), Is.EqualTo(3));

            this.sheet.SetCellText(new CellAddress(0, 0), "100");

            Assert.That(this.GetNumber(2, 0), Is.EqualTo(102));
        }

        [Test]
        public void SelfReference_BecomesCircularError()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "=A1+1");

            var value = this.sheet.GetValue(new CellAddress(0, 0));
            Assert.That(value.IsError, Is.True);
            Assert.That(value.Error, Is.EqualTo(CellError.Circular));
        }

        [Test]
        public void DirectCycle_TwoCells_BothBecomeCircularError()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "=B1"); // A1
            this.sheet.SetCellText(new CellAddress(0, 1), "=A1"); // B1, closes the cycle

            Assert.That(this.sheet.GetValue(new CellAddress(0, 0)).Error, Is.EqualTo(CellError.Circular));
            Assert.That(this.sheet.GetValue(new CellAddress(0, 1)).Error, Is.EqualTo(CellError.Circular));
        }

        [Test]
        public void IndirectCycle_ThreeCells_AllBecomeCircularError()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "=B1+1"); // A1
            this.sheet.SetCellText(new CellAddress(0, 1), "=C1+1"); // B1
            this.sheet.SetCellText(new CellAddress(0, 2), "=A1+1"); // C1, closes the cycle

            Assert.That(this.sheet.GetValue(new CellAddress(0, 0)).Error, Is.EqualTo(CellError.Circular));
            Assert.That(this.sheet.GetValue(new CellAddress(0, 1)).Error, Is.EqualTo(CellError.Circular));
            Assert.That(this.sheet.GetValue(new CellAddress(0, 2)).Error, Is.EqualTo(CellError.Circular));
        }

        [Test]
        public void BreakingACycle_ByEditingOneCell_RecoversNormalValues()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "=B1"); // A1
            this.sheet.SetCellText(new CellAddress(0, 1), "=A1"); // B1, cycle

            // Breaking the cycle: B1 no longer refers back to A1.
            this.sheet.SetCellText(new CellAddress(0, 1), "5");

            Assert.That(this.sheet.GetValue(new CellAddress(0, 1)).AsNumber(), Is.EqualTo(5));
            Assert.That(this.sheet.GetValue(new CellAddress(0, 0)).AsNumber(), Is.EqualTo(5));
        }

        [Test]
        public void UnrelatedCellNotInCycle_RecalculatesNormallyAlongsideACycle()
        {
            // A self-referencing cycle at A1, and a completely unrelated chain at F1/F2.
            this.sheet.SetCellText(new CellAddress(0, 0), "=A1+1");
            this.sheet.SetCellText(new CellAddress(0, 5), "1"); // F1
            this.sheet.SetCellText(new CellAddress(1, 5), "=F1+1"); // F2

            Assert.That(this.sheet.GetValue(new CellAddress(0, 0)).Error, Is.EqualTo(CellError.Circular));
            Assert.That(this.GetNumber(1, 5), Is.EqualTo(2));
        }

        [Test]
        public void DeferRecalculation_ValueDoesNotUpdateUntilScopeDisposed()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "1");

            using (this.sheet.DeferRecalculation())
            {
                this.sheet.SetCellText(new CellAddress(0, 0), "2");

                // Recalculation is deferred, so even the directly edited cell has not been
                // recomputed yet - this is the documented trade-off of DeferRecalculation.
                Assert.That(this.GetNumber(0, 0), Is.EqualTo(1));
            }

            Assert.That(this.GetNumber(0, 0), Is.EqualTo(2));
        }

        [Test]
        public void DeferRecalculation_DependentSettlesAfterScopeDisposed()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "1");
            this.sheet.SetCellText(new CellAddress(1, 0), "=A1+1");
            Assert.That(this.GetNumber(1, 0), Is.EqualTo(2));

            using (this.sheet.DeferRecalculation())
            {
                this.sheet.SetCellText(new CellAddress(0, 0), "10");
            }

            Assert.That(this.GetNumber(1, 0), Is.EqualTo(11));
        }

        [Test]
        public void NoOpEdit_SameNumericValue_DoesNotRaisePropertyChangedOnDependent()
        {
            this.sheet.SetCellText(new CellAddress(0, 0), "1");
            this.sheet.SetCellText(new CellAddress(1, 0), "=A1+1");
            var dependent = this.sheet.GetCell(new CellAddress(1, 0));

            var valueChanged = false;
            dependent.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Cell.Value))
                {
                    valueChanged = true;
                }
            };

            // Re-typing the same content still marks A1 dirty and recalculates B1, but B1's
            // recomputed value (2) is unchanged, so Cell.SetValueCore should suppress the event.
            this.sheet.SetCellText(new CellAddress(0, 0), "1");

            Assert.That(valueChanged, Is.False);
        }
    }
}
