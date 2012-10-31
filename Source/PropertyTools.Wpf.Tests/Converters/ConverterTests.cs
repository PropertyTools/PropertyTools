// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterTests.cs" company="PropertyTools">
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
    using System.Windows;

    using NUnit.Framework;

    [TestFixture]
    public class ConverterTests
    {
        [Test]
        public void ColorToBrush_ValidColor_ReturnsCorrectBrush()
        {
            // todo
        }

        [Test]
        public void BoolToVisibility_NotInverted_ReturnsCorrectResult()
        {
            var btv = new BoolToVisibilityConverter();
            Assert.AreEqual(Visibility.Visible, btv.Convert(true, typeof(Visibility), null, null));
            Assert.AreEqual(Visibility.Collapsed, btv.Convert(false, typeof(Visibility), null, null));
            Assert.AreEqual(true, btv.ConvertBack(Visibility.Visible, typeof(bool), null, null));
            Assert.AreEqual(false, btv.ConvertBack(Visibility.Hidden, typeof(bool), null, null));
            Assert.AreEqual(false, btv.ConvertBack(Visibility.Collapsed, typeof(bool), null, null));

            btv.NotVisibleValue = Visibility.Hidden;
            Assert.AreEqual(Visibility.Hidden, btv.Convert(false, typeof(Visibility), null, null));
            Assert.AreEqual(true, btv.ConvertBack(Visibility.Visible, typeof(bool), null, null));
            Assert.AreEqual(false, btv.ConvertBack(Visibility.Hidden, typeof(bool), null, null));
            Assert.AreEqual(false, btv.ConvertBack(Visibility.Collapsed, typeof(bool), null, null));
        }

        [Test]
        public void BoolToVisibility_Inverted_ReturnsCorrectResult()
        {
            var btv = new BoolToVisibilityConverter { InvertVisibility = true };
            Assert.AreEqual(Visibility.Visible, btv.Convert(false, typeof(Visibility), null, null));
            Assert.AreEqual(Visibility.Collapsed, btv.Convert(true, typeof(Visibility), null, null));
            Assert.AreEqual(false, btv.ConvertBack(Visibility.Visible, typeof(bool), null, null));
            Assert.AreEqual(true, btv.ConvertBack(Visibility.Hidden, typeof(bool), null, null));

            btv.NotVisibleValue = Visibility.Hidden;
            Assert.AreEqual(Visibility.Hidden, btv.Convert(true, typeof(Visibility), null, null));
            Assert.AreEqual(false, btv.ConvertBack(Visibility.Visible, typeof(bool), null, null));
            Assert.AreEqual(true, btv.ConvertBack(Visibility.Hidden, typeof(bool), null, null));
        }
    }
}