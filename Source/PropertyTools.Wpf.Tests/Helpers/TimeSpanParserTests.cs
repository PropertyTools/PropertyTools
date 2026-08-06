// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanParserTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class TimeSpanParserTests
    {
        [Test]
        public void Parse_Days_ReturnsCorrectValue()
        {
            Assert.That(TimeSpanParser.Parse("7"), Is.EqualTo(TimeSpan.FromDays(7)));
            Assert.That(TimeSpanParser.Parse("07"), Is.EqualTo(TimeSpan.FromDays(7)));
            Assert.That(TimeSpanParser.Parse("7.5"), Is.EqualTo(TimeSpan.FromDays(7.5)));
            Assert.That(TimeSpanParser.Parse("7,5"), Is.EqualTo(TimeSpan.FromDays(7.5)));
            Assert.That(TimeSpanParser.Parse("7d"), Is.EqualTo(TimeSpan.FromDays(7)));
            Assert.That(TimeSpanParser.Parse("7 d"), Is.EqualTo(TimeSpan.FromDays(7)));
            Assert.That(TimeSpanParser.Parse("07d"), Is.EqualTo(TimeSpan.FromDays(7)));
            Assert.That(TimeSpanParser.Parse("7.5d"), Is.EqualTo(TimeSpan.FromDays(7.5)));
            Assert.That(TimeSpanParser.Parse("7,5d"), Is.EqualTo(TimeSpan.FromDays(7.5)));
            Assert.That(TimeSpanParser.Parse("3d+4d"), Is.EqualTo(TimeSpan.FromDays(7)));
            Assert.That(TimeSpanParser.Parse("6d 24h"), Is.EqualTo(TimeSpan.FromDays(7)));
        }

        [Test]
        public void Parse_Hours_ReturnsCorrectValue()
        {
            Assert.That(TimeSpanParser.Parse("0:7:0:0"), Is.EqualTo(TimeSpan.FromHours(7)));
            Assert.That(TimeSpanParser.Parse("0:07:0:0"), Is.EqualTo(TimeSpan.FromHours(7)));
            Assert.That(TimeSpanParser.Parse("7h"), Is.EqualTo(TimeSpan.FromHours(7)));
            Assert.That(TimeSpanParser.Parse("7 h"), Is.EqualTo(TimeSpan.FromHours(7)));
            Assert.That(TimeSpanParser.Parse("07h"), Is.EqualTo(TimeSpan.FromHours(7)));
            Assert.That(TimeSpanParser.Parse("7.5h"), Is.EqualTo(TimeSpan.FromHours(7.5)));
            Assert.That(TimeSpanParser.Parse("7,5h"), Is.EqualTo(TimeSpan.FromHours(7.5)));
            Assert.That(TimeSpanParser.Parse("3h+4h"), Is.EqualTo(TimeSpan.FromHours(7)));
            Assert.That(TimeSpanParser.Parse("6h 60m"), Is.EqualTo(TimeSpan.FromHours(7)));
        }

        [Test]
        public void Parse_Minutes_ReturnsCorrectValue()
        {
            Assert.That(TimeSpanParser.Parse("0:0:7:0"), Is.EqualTo(TimeSpan.FromMinutes(7)));
            Assert.That(TimeSpanParser.Parse("0:0:07:0"), Is.EqualTo(TimeSpan.FromMinutes(7)));
            Assert.That(TimeSpanParser.Parse("7m"), Is.EqualTo(TimeSpan.FromMinutes(7)));
            Assert.That(TimeSpanParser.Parse("7 m"), Is.EqualTo(TimeSpan.FromMinutes(7)));
            Assert.That(TimeSpanParser.Parse("7'"), Is.EqualTo(TimeSpan.FromMinutes(7)));
            Assert.That(TimeSpanParser.Parse("07m"), Is.EqualTo(TimeSpan.FromMinutes(7)));
            Assert.That(TimeSpanParser.Parse("7.5m"), Is.EqualTo(TimeSpan.FromMinutes(7.5)));
            Assert.That(TimeSpanParser.Parse("7,5m"), Is.EqualTo(TimeSpan.FromMinutes(7.5)));
            Assert.That(TimeSpanParser.Parse("3m+4m"), Is.EqualTo(TimeSpan.FromMinutes(7)));
            Assert.That(TimeSpanParser.Parse("6m 60s"), Is.EqualTo(TimeSpan.FromMinutes(7)));
        }

        [Test]
        public void Parse_Seconds_ReturnsCorrectValue()
        {
            Assert.That(TimeSpanParser.Parse("0:0:0:7"), Is.EqualTo(TimeSpan.FromSeconds(7)));
            Assert.That(TimeSpanParser.Parse("0:0:0:07"), Is.EqualTo(TimeSpan.FromSeconds(7)));
            Assert.That(TimeSpanParser.Parse("7s"), Is.EqualTo(TimeSpan.FromSeconds(7)));
            Assert.That(TimeSpanParser.Parse("7\""), Is.EqualTo(TimeSpan.FromSeconds(7)));
            Assert.That(TimeSpanParser.Parse("7 s"), Is.EqualTo(TimeSpan.FromSeconds(7)));
            Assert.That(TimeSpanParser.Parse("07s"), Is.EqualTo(TimeSpan.FromSeconds(7)));
            Assert.That(TimeSpanParser.Parse("7.5s"), Is.EqualTo(TimeSpan.FromSeconds(7.5)));
            Assert.That(TimeSpanParser.Parse("7,5s"), Is.EqualTo(TimeSpan.FromSeconds(7.5)));
            Assert.That(TimeSpanParser.Parse("3s+4s"), Is.EqualTo(TimeSpan.FromSeconds(7)));
        }

        [Test]
        public void TryParse_InvalidText_ReturnsFalse()
        {
            TimeSpan result;

            var success = TimeSpanParser.TryParse("not-a-time-span", out result);

            Assert.That(success, Is.False);
            Assert.That(result, Is.EqualTo(default(TimeSpan)));
        }

        [Test]
        public void Parse_CommaDecimal_ReturnsCorrectValue()
        {
            var result = TimeSpanParser.Parse("7,5h");

            Assert.That(result, Is.EqualTo(TimeSpan.FromHours(7.5)));
        }

        [Test]
        public void Parse_ColonFormat_ReturnsCorrectValue()
        {
            var result = TimeSpanParser.Parse("0:07:00");

            Assert.That(result, Is.EqualTo(TimeSpan.FromMinutes(7)));
        }

        [Test]
        public void Parse_InvalidText_ThrowsFormatException()
        {
            var exception = Assert.Throws<FormatException>(() => TimeSpanParser.Parse("not-a-time-span"));

            Assert.That(exception.Message, Does.Contain("not-a-time-span"));
        }

        [Test]
        public void Parse_FormatString_ReturnsCorrectValue()
        {
            var result = TimeSpanParser.Parse("91:12", "mm:ss");

            Assert.That(result, Is.EqualTo(TimeSpan.FromMinutes(91).Add(TimeSpan.FromSeconds(12))));
        }
    }
}