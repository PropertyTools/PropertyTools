// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CellValueTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;

    using SpreadsheetDemo.Spreadsheet.Model;

    using NUnit.Framework;

    [TestFixture]
    public class CellValueTests
    {
        [Test]
        public void Empty_DefaultValue_IsEmpty()
        {
            var value = CellValue.Empty;

            Assert.That(value.IsEmpty, Is.True);
            Assert.That(value.Type, Is.EqualTo(CellValueType.Empty));
        }

        [Test]
        public void FromText_NullOrEmpty_ReturnsEmpty()
        {
            Assert.That(CellValue.FromText(null).IsEmpty, Is.True);
            Assert.That(CellValue.FromText(string.Empty).IsEmpty, Is.True);
        }

        [Test]
        public void FromNumber_ValidNumber_AsNumberReturnsIt()
        {
            var value = CellValue.FromNumber(3.14);

            Assert.That(value.Type, Is.EqualTo(CellValueType.Number));
            Assert.That(value.AsNumber(), Is.EqualTo(3.14));
        }

        [Test]
        public void FromBoolean_True_AsBooleanReturnsTrue()
        {
            var value = CellValue.FromBoolean(true);

            Assert.That(value.Type, Is.EqualTo(CellValueType.Boolean));
            Assert.That(value.AsBoolean(), Is.True);
        }

        [Test]
        public void FromDateTime_RoundTrips_AsDateTimeReturnsSameDate()
        {
            var date = new DateTime(2026, 8, 5, 13, 30, 0);

            var value = CellValue.FromDateTime(date);

            Assert.That(value.Type, Is.EqualTo(CellValueType.DateTime));
            Assert.That(value.AsDateTime(), Is.EqualTo(date));
        }

        [Test]
        public void FromError_DivideByZero_ErrorPropertyReturnsIt()
        {
            var value = CellValue.FromError(CellError.DivideByZero);

            Assert.That(value.IsError, Is.True);
            Assert.That(value.Error, Is.EqualTo(CellError.DivideByZero));
        }

        [Test]
        public void AsNumber_TextValue_Throws()
        {
            var value = CellValue.FromText("hello");

            Assert.That(() => value.AsNumber(), Throws.InvalidOperationException);
        }

        [Test]
        public void AsText_NumberValue_Throws()
        {
            var value = CellValue.FromNumber(1);

            Assert.That(() => value.AsText(), Throws.InvalidOperationException);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void TryGetNumber_Boolean_ReturnsZeroOrOne(bool input)
        {
            var value = CellValue.FromBoolean(input);

            var result = value.TryGetNumber(out var number);

            Assert.That(result, Is.True);
            Assert.That(number, Is.EqualTo(input ? 1 : 0));
        }

        [Test]
        public void TryGetNumber_NumericText_ParsesUsingInvariantCulture()
        {
            var value = CellValue.FromText("42.5");

            var result = value.TryGetNumber(out var number);

            Assert.That(result, Is.True);
            Assert.That(number, Is.EqualTo(42.5));
        }

        [Test]
        public void TryGetNumber_NonNumericText_ReturnsFalse()
        {
            var value = CellValue.FromText("hello");

            var result = value.TryGetNumber(out _);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetNumber_Empty_ReturnsZero()
        {
            var result = CellValue.Empty.TryGetNumber(out var number);

            Assert.That(result, Is.True);
            Assert.That(number, Is.EqualTo(0));
        }

        [Test]
        public void TryGetText_Error_ReturnsFalse()
        {
            var value = CellValue.FromError(CellError.Value);

            var result = value.TryGetText(out _);

            Assert.That(result, Is.False);
        }

        [Test]
        public void TryGetText_Boolean_ReturnsTrueOrFalseText()
        {
            var result = CellValue.FromBoolean(true).TryGetText(out var text);

            Assert.That(result, Is.True);
            Assert.That(text, Is.EqualTo("TRUE"));
        }

        [Test]
        public void ToObject_Empty_ReturnsNull()
        {
            Assert.That(CellValue.Empty.ToObject(), Is.Null);
        }

        [Test]
        public void ToObject_Number_ReturnsBoxedDouble()
        {
            Assert.That(CellValue.FromNumber(2.5).ToObject(), Is.EqualTo(2.5));
        }

        [Test]
        public void ToObject_Error_ReturnsDisplayText()
        {
            Assert.That(CellValue.FromError(CellError.DivideByZero).ToObject(), Is.EqualTo("#DIV/0!"));
        }

        [Test]
        public void Equals_SameNumber_ReturnsTrue()
        {
            var a = CellValue.FromNumber(1.5);
            var b = CellValue.FromNumber(1.5);

            Assert.That(a.Equals(b), Is.True);
            Assert.That(a == b, Is.True);
            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
        }

        [Test]
        public void Equals_DifferentType_ReturnsFalse()
        {
            var a = CellValue.FromNumber(1);
            var b = CellValue.FromText("1");

            Assert.That(a.Equals(b), Is.False);
            Assert.That(a != b, Is.True);
        }

        [Test]
        public void Equals_SameText_ReturnsTrue()
        {
            var a = CellValue.FromText("hello");
            var b = CellValue.FromText("hello");

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void ToString_Error_ReturnsDisplayText()
        {
            var value = CellValue.FromError(CellError.Reference);

            Assert.That(value.ToString(), Is.EqualTo("#REF!"));
        }

        [Test]
        public void ToString_Empty_ReturnsEmptyString()
        {
            Assert.That(CellValue.Empty.ToString(), Is.EqualTo(string.Empty));
        }

        [Test]
        public void FromDuration_AsDuration_RoundTrips()
        {
            var value = CellValue.FromDuration(new TimeSpan(1, 30, 15));

            Assert.That(value.Type, Is.EqualTo(CellValueType.Duration));
            Assert.That(value.AsDuration(), Is.EqualTo(new TimeSpan(1, 30, 15)));
        }

        [Test]
        public void Duration_AsNumber_ReturnsTotalDays()
        {
            var value = CellValue.FromDuration(TimeSpan.FromHours(12));

            Assert.That(value.AsNumber(), Is.EqualTo(0.5));
        }

        [Test]
        public void Duration_TryGetNumber_ReturnsTotalDays()
        {
            var value = CellValue.FromDuration(TimeSpan.FromHours(6));

            Assert.That(value.TryGetNumber(out var number), Is.True);
            Assert.That(number, Is.EqualTo(0.25));
        }

        [Test]
        public void Duration_AsText_Throws()
        {
            var value = CellValue.FromDuration(TimeSpan.FromMinutes(90));

            Assert.That(() => value.AsText(), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Duration_Equals_SameSpan_IsTrue()
        {
            var a = CellValue.FromDuration(TimeSpan.FromMinutes(90));
            var b = CellValue.FromDuration(TimeSpan.FromHours(1.5));

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void Duration_DoesNotEqualNumberWithSameDayValue()
        {
            var duration = CellValue.FromDuration(TimeSpan.FromDays(1));
            var number = CellValue.FromNumber(1);

            Assert.That(duration.Equals(number), Is.False);
        }

        [Test]
        public void CompareTo_SameTypeNumbers_ComparesNumerically()
        {
            Assert.That(CellValue.FromNumber(1).CompareTo(CellValue.FromNumber(2)), Is.LessThan(0));
            Assert.That(CellValue.FromNumber(2).CompareTo(CellValue.FromNumber(1)), Is.GreaterThan(0));
            Assert.That(CellValue.FromNumber(1).CompareTo(CellValue.FromNumber(1)), Is.EqualTo(0));
        }

        [Test]
        public void CompareTo_SameTypeText_ComparesCaseInsensitively()
        {
            Assert.That(CellValue.FromText("a").CompareTo(CellValue.FromText("B")), Is.LessThan(0));
        }

        [TestCase("Empty", "Number")]
        [TestCase("Number", "Text")]
        [TestCase("Text", "Boolean")]
        public void CompareTo_DifferentTypes_OrdersEmptyBeforeNumberBeforeTextBeforeBoolean(string lesser, string greater)
        {
            CellValue Make(string kind) => kind switch
            {
                "Empty" => CellValue.Empty,
                "Number" => CellValue.FromNumber(1),
                "Text" => CellValue.FromText("a"),
                "Boolean" => CellValue.FromBoolean(true),
                _ => throw new System.ArgumentException(kind)
            };

            Assert.That(Make(lesser).CompareTo(Make(greater)), Is.LessThan(0));
            Assert.That(Make(greater).CompareTo(Make(lesser)), Is.GreaterThan(0));
        }

        [Test]
        public void CompareTo_DurationVsText_OrdersDurationBeforeText()
        {
            var duration = CellValue.FromDuration(TimeSpan.FromMinutes(1));
            var text = CellValue.FromText("a");

            Assert.That(duration.CompareTo(text), Is.LessThan(0));
        }

        [Test]
        public void CompareTo_DurationVsDuration_ComparesNumerically()
        {
            var shorter = CellValue.FromDuration(TimeSpan.FromMinutes(1));
            var longer = CellValue.FromDuration(TimeSpan.FromMinutes(2));

            Assert.That(shorter.CompareTo(longer), Is.LessThan(0));
        }
    }
}
