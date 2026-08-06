// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanToStringConverterTests.cs" company="PropertyTools">
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
    public class TimeSpanToStringConverterTests
    {
        [Test]
        public void ConvertBack_InvalidInput_ReturnsUnsetValue()
        {
            var converter = new TimeSpanToStringConverter();

            var result = converter.ConvertBack("not-a-time-span", typeof(TimeSpan), null, CultureInfo.InvariantCulture);

            Assert.That(result, Is.EqualTo(DependencyProperty.UnsetValue));
        }

        [Test]
        public void ConvertBack_FormattedInput_ReturnsParsedTimeSpan()
        {
            var converter = new TimeSpanToStringConverter();

            var result = converter.ConvertBack("91:12", typeof(TimeSpan), "mm:ss", CultureInfo.InvariantCulture);

            Assert.That(result, Is.EqualTo(TimeSpan.FromMinutes(91).Add(TimeSpan.FromSeconds(12))));
        }
    }
}
