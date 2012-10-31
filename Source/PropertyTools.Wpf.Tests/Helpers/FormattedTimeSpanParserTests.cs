// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormattedTimeSpanParserTests.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
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
    public class FormattedTimeSpanParserTests
    {
        [Test]
        public void Test1()
        {
            ParseAndAssert(new TimeSpan(29, 14, 39, 12, 537), @"d h:mm:ss.fff", "29 14:39:12.537");
        }

        [Test]
        public void Test2()
        {
            ParseAndAssert(new TimeSpan(0, 14, 39, 12, 537), @"h:mm.ss.fff", "14:39.12.537");
        }

        [Test]
        public void Test3()
        {
            ParseAndAssert(new TimeSpan(0, 0, 39, 12, 537), @"m:ss.fff", "39:12.537");
        }

        [Test]
        public void Test4()
        {
            ParseAndAssert(new TimeSpan(0, 0, 39, 12, 0), @"m:ss", "39:12");
        }

        [Test]
        public void Test4b()
        {
            ParseAndAssert(new TimeSpan(0, 0, 39, 12, 0), @"m.ss", "39.12");
        }

        [Test]
        public void Test4c()
        {
            ParseAndAssert(new TimeSpan(0, 0, 39, 12, 0), @"m/ss", "39/12");
        }

        [Test]
        public void Test4d()
        {
            ParseAndAssert(new TimeSpan(0, 0, 9, 12, 0), @"m:ss", "Duration=9:12");
        }

        [Test]
        public void Test5()
        {
            ParseAndAssert(new TimeSpan(0, 14, 39, 0, 0), @"h:mm", "14:39");
        }

        [Test]
        public void Test6()
        {
            ParseAndAssert(new TimeSpan(0, 0, 0, 12, 537), @"s.fff", "12.537");
        }

        private static void ParseAndAssert(TimeSpan expectedValue, string formatString, string input)
        {
            var ftsp = new FormattedTimeSpanParser(formatString);
            var result = ftsp.Parse(input);
            Assert.AreEqual(expectedValue, result);
        }
    }
}