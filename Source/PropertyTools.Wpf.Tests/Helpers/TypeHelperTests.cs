namespace PropertyTools.Wpf.Tests
{
    using System.Collections.Generic;
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

        private enum TestEnum
        {
            Test1,

            Test2,

            Test3
        }

        [Test]
        public void GetEnumType_Enum_ReturnsInt()
        {
            Assert.AreEqual(typeof(TestEnum), TypeHelper.GetEnumType(typeof(TestEnum)));
        }

        [Test]
        public void GetEnumType_NullableEnum_ReturnsInt()
        {
            Assert.AreEqual(typeof(TestEnum), TypeHelper.GetEnumType(typeof(TestEnum?)));
        }

        [Test]
        public void GetItemType_ListInt_ReturnsInt()
        {
            Assert.AreEqual(typeof(int), TypeHelper.GetItemType(new List<int>()));
        }

        [Test]
        public void GetItemType_ArrayOfNullableDouble_ReturnsNullableDouble()
        {
            Assert.AreEqual(typeof(double?), TypeHelper.GetItemType(new double?[5]));
        }

        [Test]
        public void GetItemType_Dictionary_ReturnsKeyValuePair()
        {
            Assert.AreEqual(typeof(KeyValuePair<int, double>), TypeHelper.GetItemType(new Dictionary<int, double>()));
        }

        [Test]
        public void GetItemType_Null_ReturnsNull()
        {
            Assert.AreEqual(null, TypeHelper.GetItemType(null));
        }
    }
}