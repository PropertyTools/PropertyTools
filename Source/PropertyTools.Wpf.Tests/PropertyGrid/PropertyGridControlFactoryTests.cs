// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyGridControlFactoryTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests.PropertyGridNamespace
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Markup;
    using System.Windows.Media;

    using NUnit.Framework;

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class PropertyGridControlFactoryTests
    {
        [Test]
        public void CreateDefaultControl_AutoUpdateFloatingPointProperty_AllowsEnteringDecimalSeparator()
        {
            // Arrange
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            var model = new FloatingPointModel();
            var propertyDescriptor = TypeDescriptor.GetProperties(model).Find(nameof(FloatingPointModel.Value), false);
            var property = new PropertyItem(propertyDescriptor, TypeDescriptor.GetProperties(model))
            {
                AutoUpdateText = true
            };
            var factory = new TestablePropertyGridControlFactory();

            try
            {
                // Act
                var control = (TextBoxEx)factory.CreateDefaultControlForTesting(property);
                control.DataContext = model;
                control.Text = "1";
                control.Text = "1.";
                var valueAfterDecimalSeparator = model.Value;
                control.Text = "1.5";

                // Assert
                Assert.That(valueAfterDecimalSeparator, Is.EqualTo(1d));
                Assert.That(control.Text, Is.EqualTo("1.5"));
                Assert.That(model.Value, Is.EqualTo(1.5d));
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void CreateFontFamilyControl_UICultureDiffersFromElementLanguage_DisplaysLocalizedName(bool previewFonts)
        {
            // Arrange
            var originalUICulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-CN");
            var fontFamily = new FontFamily();
            fontFamily.FamilyNames[XmlLanguage.GetLanguage("en-us")] = "SimSun";
            fontFamily.FamilyNames[XmlLanguage.GetLanguage("zh-cn")] = "宋体";

            try
            {
                var property = CreateFontProperty(new FontModel(), previewFonts);
                var factory = new TestablePropertyGridControlFactory();

                // Act
                var control = (ComboBox)factory.CreateFontFamilyControlForTesting(property);
                Assert.That(control.ItemTemplate, Is.Not.Null);
                var textBlock = (TextBlock)control.ItemTemplate.LoadContent();
                textBlock.Language = XmlLanguage.GetLanguage("en-US");
                textBlock.DataContext = fontFamily;

                // Assert
                Assert.That(textBlock.Text, Is.EqualTo("宋体"));
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = originalUICulture;
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void CreateFontFamilyControl_SelectItem_UpdatesProperty(bool previewFonts)
        {
            // Arrange
            var model = new FontModel();
            var property = CreateFontProperty(model, previewFonts);
            var factory = new TestablePropertyGridControlFactory();
            var control = (ComboBox)factory.CreateFontFamilyControlForTesting(property);
            control.DataContext = model;
            Assume.That(control.Items.Count, Is.GreaterThan(0));
            var expected = control.Items[control.Items.Count - 1];

            // Act
            control.SelectedItem = expected;

            // Assert
            Assert.That(model.Font, Is.SameAs(expected));
        }

        private static PropertyItem CreateFontProperty(FontModel model, bool previewFonts)
        {
            var properties = TypeDescriptor.GetProperties(model);
            var propertyDescriptor = properties.Find(nameof(FontModel.Font), false);
            return new PropertyItem(propertyDescriptor, properties)
            {
                PreviewFonts = previewFonts,
                FontSize = 12,
                FontWeight = 400
            };
        }

        private class TestablePropertyGridControlFactory : PropertyGridControlFactory
        {
            public FrameworkElement CreateDefaultControlForTesting(PropertyItem property)
            {
                return this.CreateDefaultControl(property);
            }

            public FrameworkElement CreateFontFamilyControlForTesting(PropertyItem property)
            {
                return this.CreateFontFamilyControl(property);
            }
        }

        private class FloatingPointModel
        {
            public double Value { get; set; }
        }

        private class FontModel
        {
            public FontFamily Font { get; set; }
        }
    }
}
