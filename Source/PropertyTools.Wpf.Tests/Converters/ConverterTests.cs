// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Windows;
    using System.Windows.Media;

    using NUnit.Framework;

    [TestFixture]
    public class ConverterTests
    {
        [Test]
        public void ColorToBrush_ValidColor_ReturnsCorrectBrush()
        {
            var c = new ColorToBrushConverter();
            var b = c.Convert(Colors.Blue, typeof(Brush), null, null);
            Assert.AreEqual(Brushes.Blue.ToString(), b.ToString());
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