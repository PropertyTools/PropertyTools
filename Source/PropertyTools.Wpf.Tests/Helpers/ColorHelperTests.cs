// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorHelperTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Windows.Media;

    using NUnit.Framework;

    [TestFixture]
    public class ColorHelperTests
    {
        [Test]
        public void ChangeAlpha_ValidColor_ReturnsCorrectValue()
        {
            Assert.That(ColorHelper.ColorToHex(ColorHelper.ChangeAlpha(Colors.Lavender, 127)), Is.EqualTo("#7FE6E6FA"));
        }

        [Test]
        public void Interpolate_ValidColors_ReturnsCorrectValue()
        {
            Assert.That(ColorHelper.ColorToHex(ColorHelper.Interpolate(Colors.Green, Colors.Blue, 0.3)), Is.EqualTo("#FF00594C"));
        }

        [Test]
        public void Complementary_ValidColors_ReturnsCorrectValue()
        {
            // https://en.wikipedia.org/wiki/Complementary_color
            /*   Assert.AreEqual(Colors.Green, ColorHelper.Complementary(Colors.Red),"Red");
               Assert.AreEqual(Colors.Red, ColorHelper.Complementary(Colors.Green),"Green");
               Assert.AreEqual(Colors.Orange, ColorHelper.Complementary(Colors.Blue),"Blue");
               Assert.AreEqual(Colors.Blue, ColorHelper.Complementary(Colors.Orange),"Orange");
               Assert.AreEqual(Colors.Purple, ColorHelper.Complementary(Colors.Yellow),"Yellow");
               Assert.AreEqual(Colors.Yellow, ColorHelper.Complementary(Colors.Purple),"Purple");*/
        }

        [Test]
        public void ColorToHex_ValidColors_ReturnsCorrectString()
        {
            Assert.That(ColorHelper.ColorToHex(Colors.Blue), Is.EqualTo("#FF0000FF"));
            Assert.That(ColorHelper.ColorToHex(Colors.Green), Is.EqualTo("#FF008000"));
        }
        [Test]
        public void HexToColor_ValidColors_ReturnsCorrectColor()
        {
            Assert.That(ColorHelper.HexToColor("#FF0000FF"), Is.EqualTo(Colors.Blue));
            Assert.That(ColorHelper.HexToColor("ff008000"), Is.EqualTo(Colors.Green));
        }

        [Test]
        public void HexToColor_InvalidColors_ReturnsUndefined()
        {
            Assert.That(ColorHelper.HexToColor("#FFFG00FF"), Is.EqualTo(ColorHelper.UndefinedColor));
            Assert.That(ColorHelper.HexToColor("#FFFG00F"), Is.EqualTo(ColorHelper.UndefinedColor));
            Assert.That(ColorHelper.HexToColor("-1"), Is.EqualTo(ColorHelper.UndefinedColor));
        }

        [Test]
        public void ColorDifference_ValidColors_ReturnsCorrectDistance()
        {
            Assert.That(ColorHelper.ColorDifference(Colors.Blue, Colors.LightBlue), Is.EqualTo(1.08).Within(0.01));
        }

        [Test]
        public void HueDifference_ValidColors_ReturnsCorrectDistance()
        {
            Assert.That(ColorHelper.HueDifference(Colors.Blue, Colors.LightBlue), Is.EqualTo(0.125).Within(0.001));
        }

        [Test]
        public void UIntToColor_ValidColors_Success()
        {
            Assert.That(ColorHelper.UIntToColor(0xFFFF0000), Is.EqualTo(Colors.Red));
            Assert.That(ColorHelper.ColorToUint(Colors.Red), Is.EqualTo(0xFFFF0000));
        }

        [Test]
        public void ColorToHsv_ValidColors_ReturnsCorrectValues()
        {
            var hsv = ColorHelper.ColorToHsvBytes(Colors.Red);
            Assert.That(hsv[0], Is.EqualTo(0));
            Assert.That(hsv[1], Is.EqualTo(255));
            Assert.That(hsv[2], Is.EqualTo(255));

            hsv = ColorHelper.ColorToHsvBytes(Colors.Orange);
            Assert.That(hsv[0], Is.EqualTo(27), "hue");
            Assert.That(hsv[1], Is.EqualTo(255), "sat");
            Assert.That(hsv[2], Is.EqualTo(255), "value");

            hsv = ColorHelper.ColorToHsvBytes(Colors.Brown);
            Assert.That(hsv[0], Is.EqualTo(0), "hue");
            Assert.That(hsv[1], Is.EqualTo(190), "sat");
            Assert.That(hsv[2], Is.EqualTo(165), "value");
        }

        [Test]
        public void HsvToColor_ValidColors_ReturnsCorrectColor()
        {
            Assert.That(ColorHelper.HsvToColor(0, 255, 255), Is.EqualTo(Colors.Red), "Red");
            //  Assert.That(ColorHelper.HsvToColor(27, 255, 255), Is.EqualTo(Colors.Orange), "Orange");
            Assert.That(ColorHelper.HsvToColor(0, 190, 165), Is.EqualTo(Colors.Brown), "Brown");
        }

    }
}