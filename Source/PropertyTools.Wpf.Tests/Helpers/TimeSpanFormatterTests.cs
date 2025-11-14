// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanFormatterTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class TimeSpanFormatterTests
    {
        [Test]
        public void Format_Days_ReturnsCorrectValue()
        {
            var f = new TimeSpanFormatter();
            var span = new TimeSpan(1, 7, 3, 2);
            Assert.That(string.Format(f, "{0:DD}", span), Is.EqualTo("01"));
            Assert.That(string.Format(f, "{0:D}", span), Is.EqualTo("1"));
            Assert.That(string.Format(f, "{0:D hh}", span), Is.EqualTo("1 07"));
            Assert.That(string.Format(f, "{0:DD hh mm ss}", span), Is.EqualTo("01 07 03 02"));
        }
    }
}