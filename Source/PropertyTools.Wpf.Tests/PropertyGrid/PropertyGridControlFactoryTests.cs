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

        private class TestablePropertyGridControlFactory : PropertyGridControlFactory
        {
            public FrameworkElement CreateDefaultControlForTesting(PropertyItem property)
            {
                return this.CreateDefaultControl(property);
            }
        }

        private class FloatingPointModel
        {
            public double Value { get; set; }
        }
    }
}
