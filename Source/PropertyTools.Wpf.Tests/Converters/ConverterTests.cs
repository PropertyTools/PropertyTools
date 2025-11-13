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
            Assert.That(b.ToString(), Is.EqualTo(Brushes.Blue.ToString()));
        }

        [Test]
        public void BoolToVisibility_NotInverted_ReturnsCorrectResult()
        {
            var btv = new BoolToVisibilityConverter();
            Assert.That(btv.Convert(true, typeof(Visibility), null, null), Is.EqualTo(Visibility.Visible));
            Assert.That(btv.Convert(false, typeof(Visibility), null, null), Is.EqualTo(Visibility.Collapsed));
            Assert.That(btv.ConvertBack(Visibility.Visible, typeof(bool), null, null), Is.EqualTo(true));
            Assert.That(btv.ConvertBack(Visibility.Hidden, typeof(bool), null, null), Is.EqualTo(false));
            Assert.That(btv.ConvertBack(Visibility.Collapsed, typeof(bool), null, null), Is.EqualTo(false));

            btv.NotVisibleValue = Visibility.Hidden;
            Assert.That(btv.Convert(false, typeof(Visibility), null, null), Is.EqualTo(Visibility.Hidden));
            Assert.That(btv.ConvertBack(Visibility.Visible, typeof(bool), null, null), Is.EqualTo(true));
            Assert.That(btv.ConvertBack(Visibility.Hidden, typeof(bool), null, null), Is.EqualTo(false));
            Assert.That(btv.ConvertBack(Visibility.Collapsed, typeof(bool), null, null), Is.EqualTo(false));
        }

        [Test]
        public void BoolToVisibility_Inverted_ReturnsCorrectResult()
        {
            var btv = new BoolToVisibilityConverter { InvertVisibility = true };
            Assert.That(btv.Convert(false, typeof(Visibility), null, null), Is.EqualTo(Visibility.Visible));
            Assert.That(btv.Convert(true, typeof(Visibility), null, null), Is.EqualTo(Visibility.Collapsed));
            Assert.That(btv.ConvertBack(Visibility.Visible, typeof(bool), null, null), Is.EqualTo(false));
            Assert.That(btv.ConvertBack(Visibility.Hidden, typeof(bool), null, null), Is.EqualTo(true));

            btv.NotVisibleValue = Visibility.Hidden;
            Assert.That(btv.Convert(true, typeof(Visibility), null, null), Is.EqualTo(Visibility.Hidden));
            Assert.That(btv.ConvertBack(Visibility.Visible, typeof(bool), null, null), Is.EqualTo(false));
            Assert.That(btv.ConvertBack(Visibility.Hidden, typeof(bool), null, null), Is.EqualTo(true));
        }
    }
}