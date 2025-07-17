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
            Assert.AreEqual("#7FE6E6FA", ColorHelper.ColorToHex(ColorHelper.ChangeAlpha(Colors.Lavender, 127)));
        }

        [Test]
        public void Interpolate_ValidColors_ReturnsCorrectValue()
        {
            Assert.AreEqual("#FF00594C", ColorHelper.ColorToHex(ColorHelper.Interpolate(Colors.Green, Colors.Blue, 0.3)));
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
            Assert.AreEqual("#FF0000FF", ColorHelper.ColorToHex(Colors.Blue));
            Assert.AreEqual("#FF008000", ColorHelper.ColorToHex(Colors.Green));
        }
        [Test]
        public void HexToColor_ValidColors_ReturnsCorrectColor()
        {
            Assert.AreEqual(Colors.Blue, ColorHelper.HexToColor("#FF0000FF"));
            Assert.AreEqual(Colors.Green, ColorHelper.HexToColor("ff008000"));
        }

        [Test]
        public void HexToColor_InvalidColors_ReturnsUndefined()
        {
            Assert.AreEqual(ColorHelper.UndefinedColor, ColorHelper.HexToColor("#FFFG00FF"));
            Assert.AreEqual(ColorHelper.UndefinedColor, ColorHelper.HexToColor("#FFFG00F"));
            Assert.AreEqual(ColorHelper.UndefinedColor, ColorHelper.HexToColor("-1"));
        }

        [Test]
        public void ColorDifference_ValidColors_ReturnsCorrectDistance()
        {
            Assert.AreEqual(1.08, ColorHelper.ColorDifference(Colors.Blue, Colors.LightBlue), 0.01);
        }

        [Test]
        public void HueDifference_ValidColors_ReturnsCorrectDistance()
        {
            Assert.AreEqual(0.125, ColorHelper.HueDifference(Colors.Blue, Colors.LightBlue), 0.001);
        }

        [Test]
        public void UIntToColor_ValidColors_Success()
        {
            Assert.AreEqual(Colors.Red, ColorHelper.UIntToColor(0xFFFF0000));
            Assert.AreEqual(0xFFFF0000, ColorHelper.ColorToUint(Colors.Red));
        }

        [Test]
        public void ColorToHsv_ValidColors_ReturnsCorrectValues()
        {
            var hsv = ColorHelper.ColorToHsvBytes(Colors.Red);
            Assert.AreEqual(0, hsv[0]);
            Assert.AreEqual(255, hsv[1]);
            Assert.AreEqual(255, hsv[2]);

            hsv = ColorHelper.ColorToHsvBytes(Colors.Orange);
            Assert.AreEqual(27, hsv[0], "hue");
            Assert.AreEqual(255, hsv[1], "sat");
            Assert.AreEqual(255, hsv[2], "value");

            hsv = ColorHelper.ColorToHsvBytes(Colors.Brown);
            Assert.AreEqual(0, hsv[0], "hue");
            Assert.AreEqual(190, hsv[1], "sat");
            Assert.AreEqual(165, hsv[2], "value");
        }

        [Test]
        public void HsvToColor_ValidColors_ReturnsCorrectColor()
        {
            Assert.AreEqual(Colors.Red, ColorHelper.HsvToColor(0, 255, 255), "Red");
            //  Assert.AreEqual(Colors.Orange, ColorHelper.HsvToColor(27, 255, 255),"Orange");
            Assert.AreEqual(Colors.Brown, ColorHelper.HsvToColor(0, 190, 165), "Brown");
        }

    }
}