using NUnit.Framework;
using OpenControls;
using System.Windows.Media;

namespace PropertyEditorTests
{
    [TestFixture]
    public class ColorHelperTests
    {
        [Test]
        public void ChangeAlpha_ValidColor_ReturnsCorrectValue()
        {
            Assert.AreEqual("#7FE6E6FA", ColorHelper.ColorToHex(ColorHelper.ChangeAlpha(Colors.Lavender, 127)));
        }

        [Test]
        public void ColorToHex_ValidColors_ReturnsCorrectString()
        {
            Assert.AreEqual("#FF0000FF",ColorHelper.ColorToHex(Colors.Blue));
            Assert.AreEqual("#FF008000", ColorHelper.ColorToHex(Colors.Green));
        }
        [Test]
        public void HexToColor_ValidColors_ReturnsCorrectColor()
        {
            Assert.AreEqual(Colors.Blue,ColorHelper.HexToColor("#FF0000FF"));
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
        public void ColorDistance_ValidColors_ReturnsCorrectDistance()
        {
            Assert.AreEqual(277, ColorHelper.ColorDistance(Colors.Blue,Colors.LightBlue),1);
        }

        [Test]
        public void HueDistance_ValidColors_ReturnsCorrectDistance()
        {
            Assert.AreEqual(33, ColorHelper.HueDistance(Colors.Blue, Colors.LightBlue), 1);
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
            var hsv = ColorHelper.ColorToHsv(Colors.Red);
            Assert.AreEqual(0, hsv[0]);
            Assert.AreEqual(255, hsv[1]);
            Assert.AreEqual(255, hsv[2]);

            hsv = ColorHelper.ColorToHsv(Colors.Orange);
            Assert.AreEqual(27, hsv[0],"hue");
            Assert.AreEqual(255, hsv[1],"sat");
            Assert.AreEqual(255, hsv[2],"value");

            hsv = ColorHelper.ColorToHsv(Colors.Brown);
            Assert.AreEqual(0, hsv[0], "hue");
            Assert.AreEqual(190, hsv[1], "sat");
            Assert.AreEqual(165, hsv[2], "value");
        }

        [Test]
        public void HsvToColor_ValidColors_ReturnsCorrectColor()
        {
            Assert.AreEqual(Colors.Red, ColorHelper.HsvToColor(0, 255, 255),"Red");
          //  Assert.AreEqual(Colors.Orange, ColorHelper.HsvToColor(27, 255, 255),"Orange");
            Assert.AreEqual(Colors.Brown, ColorHelper.HsvToColor(0, 190, 165),"Brown");
        }


    }
}
