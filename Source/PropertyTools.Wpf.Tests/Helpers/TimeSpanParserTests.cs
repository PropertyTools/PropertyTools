// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeSpanParserTests.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
            Assert.AreEqual(TimeSpan.FromDays(7), TimeSpanParser.Parse("7"));
            Assert.AreEqual(TimeSpan.FromDays(7), TimeSpanParser.Parse("07"));
            Assert.AreEqual(TimeSpan.FromDays(7.5), TimeSpanParser.Parse("7.5"));
            Assert.AreEqual(TimeSpan.FromDays(7.5), TimeSpanParser.Parse("7,5"));
            Assert.AreEqual(TimeSpan.FromDays(7), TimeSpanParser.Parse("7d"));
            Assert.AreEqual(TimeSpan.FromDays(7), TimeSpanParser.Parse("7 d"));
            Assert.AreEqual(TimeSpan.FromDays(7), TimeSpanParser.Parse("07d"));
            Assert.AreEqual(TimeSpan.FromDays(7.5), TimeSpanParser.Parse("7.5d"));
            Assert.AreEqual(TimeSpan.FromDays(7.5), TimeSpanParser.Parse("7,5d"));
            Assert.AreEqual(TimeSpan.FromDays(7), TimeSpanParser.Parse("3d+4d"));
            Assert.AreEqual(TimeSpan.FromDays(7), TimeSpanParser.Parse("6d 24h"));
        }

        [Test]
        public void Parse_Hours_ReturnsCorrectValue()
        {
            Assert.AreEqual(TimeSpan.FromHours(7), TimeSpanParser.Parse("0:7:0:0"));
            Assert.AreEqual(TimeSpan.FromHours(7), TimeSpanParser.Parse("0:07:0:0"));
            Assert.AreEqual(TimeSpan.FromHours(7), TimeSpanParser.Parse("7h"));
            Assert.AreEqual(TimeSpan.FromHours(7), TimeSpanParser.Parse("7 h"));
            Assert.AreEqual(TimeSpan.FromHours(7), TimeSpanParser.Parse("07h"));
            Assert.AreEqual(TimeSpan.FromHours(7.5), TimeSpanParser.Parse("7.5h"));
            Assert.AreEqual(TimeSpan.FromHours(7.5), TimeSpanParser.Parse("7,5h"));
            Assert.AreEqual(TimeSpan.FromHours(7), TimeSpanParser.Parse("3h+4h"));
            Assert.AreEqual(TimeSpan.FromHours(7), TimeSpanParser.Parse("6h 60m"));
        }

        [Test]
        public void Parse_Minutes_ReturnsCorrectValue()
        {
            Assert.AreEqual(TimeSpan.FromMinutes(7), TimeSpanParser.Parse("0:0:7:0"));
            Assert.AreEqual(TimeSpan.FromMinutes(7), TimeSpanParser.Parse("0:0:07:0"));
            Assert.AreEqual(TimeSpan.FromMinutes(7), TimeSpanParser.Parse("7m"));
            Assert.AreEqual(TimeSpan.FromMinutes(7), TimeSpanParser.Parse("7 m"));
            Assert.AreEqual(TimeSpan.FromMinutes(7), TimeSpanParser.Parse("7'"));
            Assert.AreEqual(TimeSpan.FromMinutes(7), TimeSpanParser.Parse("07m"));
            Assert.AreEqual(TimeSpan.FromMinutes(7.5), TimeSpanParser.Parse("7.5m"));
            Assert.AreEqual(TimeSpan.FromMinutes(7.5), TimeSpanParser.Parse("7,5m"));
            Assert.AreEqual(TimeSpan.FromMinutes(7), TimeSpanParser.Parse("3m+4m"));
            Assert.AreEqual(TimeSpan.FromMinutes(7), TimeSpanParser.Parse("6m 60s"));
        }

        [Test]
        public void Parse_Seconds_ReturnsCorrectValue()
        {
            Assert.AreEqual(TimeSpan.FromSeconds(7), TimeSpanParser.Parse("0:0:0:7"));
            Assert.AreEqual(TimeSpan.FromSeconds(7), TimeSpanParser.Parse("0:0:0:07"));
            Assert.AreEqual(TimeSpan.FromSeconds(7), TimeSpanParser.Parse("7s"));
            Assert.AreEqual(TimeSpan.FromSeconds(7), TimeSpanParser.Parse("7\""));
            Assert.AreEqual(TimeSpan.FromSeconds(7), TimeSpanParser.Parse("7 s"));
            Assert.AreEqual(TimeSpan.FromSeconds(7), TimeSpanParser.Parse("07s"));
            Assert.AreEqual(TimeSpan.FromSeconds(7.5), TimeSpanParser.Parse("7.5s"));
            Assert.AreEqual(TimeSpan.FromSeconds(7.5), TimeSpanParser.Parse("7,5s"));
            Assert.AreEqual(TimeSpan.FromSeconds(7), TimeSpanParser.Parse("3s+4s"));
        }
    }
}