// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FontFamilyNameConverterTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media;

    using NUnit.Framework;

    [TestFixture]
    public class FontFamilyNameConverterTests
    {
        private static FontFamily CreateFontFamily()
        {
            var fontFamily = new FontFamily();
            fontFamily.FamilyNames[XmlLanguage.GetLanguage("en-us")] = "SimSun";
            fontFamily.FamilyNames[XmlLanguage.GetLanguage("zh-cn")] = "宋体";
            return fontFamily;
        }

        [Test]
        public void Convert_ChineseCulture_ReturnsLocalizedName()
        {
            var converter = new FontFamilyNameConverter();
            var result = converter.Convert(CreateFontFamily(), typeof(string), null, new CultureInfo("zh-CN"));
            Assert.That(result, Is.EqualTo("宋体"));
        }

        [Test]
        public void Convert_EnglishCulture_ReturnsEnglishName()
        {
            var converter = new FontFamilyNameConverter();
            var result = converter.Convert(CreateFontFamily(), typeof(string), null, new CultureInfo("en-US"));
            Assert.That(result, Is.EqualTo("SimSun"));
        }

        [Test]
        public void Convert_CultureWithoutLocalizedName_ReturnsEnglishName()
        {
            var converter = new FontFamilyNameConverter();
            var result = converter.Convert(CreateFontFamily(), typeof(string), null, new CultureInfo("nb-NO"));
            Assert.That(result, Is.EqualTo("SimSun"));
        }

        [Test]
        public void Convert_InvariantCulture_ReturnsEnglishName()
        {
            var converter = new FontFamilyNameConverter();
            var result = converter.Convert(CreateFontFamily(), typeof(string), null, CultureInfo.InvariantCulture);
            Assert.That(result, Is.EqualTo("SimSun"));
        }

        [Test]
        public void Convert_SpecificCultureWithNeutralName_ReturnsNeutralCultureName()
        {
            var fontFamily = new FontFamily();
            fontFamily.FamilyNames[XmlLanguage.GetLanguage("en-us")] = "Example";
            fontFamily.FamilyNames[XmlLanguage.GetLanguage("de")] = "Beispiel";
            var converter = new FontFamilyNameConverter();
            var result = converter.Convert(fontFamily, typeof(string), null, new CultureInfo("de-DE"));
            Assert.That(result, Is.EqualTo("Beispiel"));
        }

        [Test]
        public void Convert_NonFontFamilyValue_ReturnsValue()
        {
            var converter = new FontFamilyNameConverter();
            Assert.That(converter.Convert("Arial", typeof(string), null, CultureInfo.InvariantCulture), Is.EqualTo("Arial"));
            Assert.That(converter.Convert(null, typeof(string), null, CultureInfo.InvariantCulture), Is.Null);
        }

        [Test]
        public void GetLocalizedName_NullFontFamily_ReturnsNull()
        {
            Assert.That(FontFamilyNameConverter.GetLocalizedName(null, CultureInfo.InvariantCulture), Is.Null);
        }

        [Test]
        public void ConvertBack_AnyValue_ReturnsUnsetValue()
        {
            var converter = new FontFamilyNameConverter();
            var result = converter.ConvertBack("宋体", typeof(FontFamily), null, CultureInfo.InvariantCulture);
            Assert.That(result, Is.EqualTo(DependencyProperty.UnsetValue));
        }
    }
}
