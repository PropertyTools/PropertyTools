// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateTimeToStringConverterTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using System.Globalization;
    using System.Windows;

    using NUnit.Framework;

    [TestFixture]
    public class DateTimeToStringConverterTests
    {
        [Test]
        public void Convert_ValidDateTimeAndFormatString_ReturnsFormattedDate()
        {
            var converter = new DateTimeToStringConverter();
            var culture = CultureInfo.GetCultureInfo("en-US");

            var result = converter.Convert(new DateTime(2014, 10, 4), typeof(string), "dd/MM/yyyy", culture);

            Assert.That(result, Is.EqualTo("04/10/2014"));
        }

        [Test]
        public void Convert_NullValue_ReturnsNull()
        {
            var converter = new DateTimeToStringConverter();

            var result = converter.Convert(null, typeof(string), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void ConvertBack_DdMmYyyyInputWithFormatString_ReturnsCorrectDate()
        {
            var converter = new DateTimeToStringConverter();
            var culture = CultureInfo.GetCultureInfo("en-US");

            var result = converter.ConvertBack("04/10/2014", typeof(DateTime), "dd/MM/yyyy", culture);

            Assert.That(result, Is.EqualTo(new DateTime(2014, 10, 4)));
        }

        [Test]
        public void ConvertBack_DdMmYyyyInputWithCompositeFormatString_ReturnsCorrectDate()
        {
            var converter = new DateTimeToStringConverter();
            var culture = CultureInfo.GetCultureInfo("en-US");

            var result = converter.ConvertBack("04/10/2014", typeof(DateTime), "{0:dd/MM/yyyy}", culture);

            Assert.That(result, Is.EqualTo(new DateTime(2014, 10, 4)));
        }

        [Test]
        public void ConvertBack_EmptyInputAndNullableDateTimeTarget_ReturnsNull()
        {
            var converter = new DateTimeToStringConverter();

            var result = converter.ConvertBack(string.Empty, typeof(DateTime?), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void ConvertBack_InvalidTargetType_ReturnsUnsetValue()
        {
            var converter = new DateTimeToStringConverter();

            var result = converter.ConvertBack("04/10/2014", typeof(TimeSpan), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            Assert.That(result, Is.EqualTo(DependencyProperty.UnsetValue));
        }
    }
}
