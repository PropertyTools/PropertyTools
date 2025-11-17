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
            Assert.That(c.Convert(true, typeof(bool), true, null), Is.EqualTo(true));
            Assert.That(c.Convert(false, typeof(bool), true, null), Is.EqualTo(false));
            Assert.That(c.Convert(true, typeof(bool), false, null), Is.EqualTo(false));
            Assert.That(c.Convert(false, typeof(bool), false, null), Is.EqualTo(true));
            Assert.That(c.Convert(null, typeof(bool), false, null), Is.EqualTo(false));
            Assert.That(c.Convert(true, typeof(bool), null, null), Is.EqualTo(false));
            Assert.That(c.Convert(null, typeof(bool), null, null), Is.EqualTo(true));
            Assert.That(c.Convert(null, typeof(bool), 1, null), Is.EqualTo(false));
            Assert.That(c.Convert(1, typeof(bool), 1, null), Is.EqualTo(true));

            Assert.That(c.ConvertBack(true, typeof(int), 1, null), Is.EqualTo(1));
            Assert.That(c.ConvertBack(false, typeof(int), 1, null), Is.EqualTo(DependencyProperty.UnsetValue));
            Assert.That(c.ConvertBack(null, typeof(int), 1, null), Is.EqualTo(DependencyProperty.UnsetValue));
            Assert.That(c.ConvertBack(true, typeof(object), null, null), Is.EqualTo(null));
        }

        [Test]
        public void FalseWhenEqual()
        {
            var c = new ValueToBooleanConverter(false);
            Assert.That(c.Convert(true, typeof(bool), true, null), Is.EqualTo(false));
            Assert.That(c.Convert(false, typeof(bool), true, null), Is.EqualTo(true));
            Assert.That(c.Convert(true, typeof(bool), false, null), Is.EqualTo(true));
            Assert.That(c.Convert(false, typeof(bool), false, null), Is.EqualTo(false));
            Assert.That(c.Convert(null, typeof(bool), false, null), Is.EqualTo(true));
            Assert.That(c.Convert(true, typeof(bool), null, null), Is.EqualTo(true));
            Assert.That(c.Convert(null, typeof(bool), null, null), Is.EqualTo(false));
            Assert.That(c.Convert(null, typeof(bool), 1, null), Is.EqualTo(true));
            Assert.That(c.Convert(1, typeof(bool), 1, null), Is.EqualTo(false));

            Assert.That(c.ConvertBack(false, typeof(int), 1, null), Is.EqualTo(1));
            Assert.That(c.ConvertBack(true, typeof(int), 1, null), Is.EqualTo(DependencyProperty.UnsetValue));
            Assert.That(c.ConvertBack(null, typeof(int), 1, null), Is.EqualTo(DependencyProperty.UnsetValue));
            Assert.That(c.ConvertBack(false, typeof(object), null, null), Is.EqualTo(null));
        }
    }
}