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
            Assert.AreEqual("01", string.Format(f, "{0:DD}", span));
            Assert.AreEqual("1", string.Format(f, "{0:D}", span));
            Assert.AreEqual("1 07", string.Format(f, "{0:D hh}", span));
            Assert.AreEqual("01 07 03 02", string.Format(f, "{0:DD hh mm ss}", span));
        }
    }
}
