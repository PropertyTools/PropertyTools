// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeHelperTests.cs" company="PropertyTools">
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