namespace PropertyTools.Wpf.Tests
{
    using System.Windows.Media;

    using NUnit.Framework;

    [TestFixture]
    public class TypeHelperTests
    {
        [Test]
        public void FindBiggestCommonType_UniformList_ReturnsCorrectType()
        {
            var brushes = new[] { new SolidColorBrush(), new SolidColorBrush() };
            Assert.AreEqual(typeof(SolidColorBrush), TypeHelper.FindBiggestCommonType(brushes));
        }
        [Test]
        public void FindBiggestCommonType_MixedList_ReturnsBaseType()
        {
            var brushes = new Brush[] { new SolidColorBrush(), new LinearGradientBrush() };
            Assert.AreEqual(typeof(Brush), TypeHelper.FindBiggestCommonType(brushes));
        }
        [Test]
        public void FindBiggestCommonType_ListOfObjects_ReturnsBaseType()
        {
            var brushes = new object[] { new SolidColorBrush(), new LinearGradientBrush() };
            Assert.AreEqual(typeof(Brush), TypeHelper.FindBiggestCommonType(brushes));
        }
    }
}