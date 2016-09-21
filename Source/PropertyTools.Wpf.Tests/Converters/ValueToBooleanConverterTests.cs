namespace PropertyTools.Wpf.Tests
{
    using System.Windows;

    using NUnit.Framework;

    [TestFixture]
    public class ValueToBooleanConverterTests
    {
        [Test]
        public void TrueWhenEqual()
        {
            var c = new ValueToBooleanConverter();
            Assert.AreEqual(true, c.Convert(true, typeof(bool), true, null));
            Assert.AreEqual(false, c.Convert(false, typeof(bool), true, null));
            Assert.AreEqual(false, c.Convert(true, typeof(bool), false, null));
            Assert.AreEqual(true, c.Convert(false, typeof(bool), false, null));
            Assert.AreEqual(false, c.Convert(null, typeof(bool), false, null));
            Assert.AreEqual(false, c.Convert(true, typeof(bool), null, null));
            Assert.AreEqual(true, c.Convert(null, typeof(bool), null, null));
            Assert.AreEqual(false, c.Convert(null, typeof(bool), 1, null));
            Assert.AreEqual(true, c.Convert(1, typeof(bool), 1, null));

            Assert.AreEqual(1, c.ConvertBack(true, typeof(int), 1, null));
            Assert.AreEqual(DependencyProperty.UnsetValue, c.ConvertBack(false, typeof(int), 1, null));
            Assert.AreEqual(DependencyProperty.UnsetValue, c.ConvertBack(null, typeof(int), 1, null));
            Assert.AreEqual(null, c.ConvertBack(true, typeof(object), null, null));
        }

        [Test]
        public void FalseWhenEqual()
        {
            var c = new ValueToBooleanConverter(false);
            Assert.AreEqual(false, c.Convert(true, typeof(bool), true, null));
            Assert.AreEqual(true, c.Convert(false, typeof(bool), true, null));
            Assert.AreEqual(true, c.Convert(true, typeof(bool), false, null));
            Assert.AreEqual(false, c.Convert(false, typeof(bool), false, null));
            Assert.AreEqual(true, c.Convert(null, typeof(bool), false, null));
            Assert.AreEqual(true, c.Convert(true, typeof(bool), null, null));
            Assert.AreEqual(false, c.Convert(null, typeof(bool), null, null));
            Assert.AreEqual(true, c.Convert(null, typeof(bool), 1, null));
            Assert.AreEqual(false, c.Convert(1, typeof(bool), 1, null));

            Assert.AreEqual(1, c.ConvertBack(false, typeof(int), 1, null));
            Assert.AreEqual(DependencyProperty.UnsetValue, c.ConvertBack(true, typeof(int), 1, null));
            Assert.AreEqual(DependencyProperty.UnsetValue, c.ConvertBack(null, typeof(int), 1, null));
            Assert.AreEqual(null, c.ConvertBack(false, typeof(object), null, null));
        }
    }
}