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