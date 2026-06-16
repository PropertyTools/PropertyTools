// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuantityTypeBindingRegressionTests.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Regression tests for issue #511: PropertyGrid silently discards keyboard input for
//   quantity-type (Length, Angle) properties on ICustomTypeDescriptor models.
//
//   Root cause: PropertyGridOperator.GetPropertyCollection() calls
//   ReplaceWithReflectionDescriptors() for every ICustomTypeDescriptor object.
//   When the ICustomTypeDescriptor-provided descriptor has PropertyType = Length but
//   the backing CLR property has PropertyType = double, the replacement substitutes a
//   double descriptor for the custom Length descriptor.  The resulting type mismatch
//   causes PropertyDescriptor.SetValue() to throw when the LengthConverter's
//   ConvertBack returns a Length — and because CreateBinding() sets
//   ValidatesOnExceptions = true, the exception is swallowed and the field reverts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Tests.PropertyGridNamespace
{
    using System;
    using System.ComponentModel;
    using System.Threading;

    using NUnit.Framework;

    using PropertyTools.DataAnnotations;
    using PropertyTools.Wpf;

    /// <summary>
    /// Regression tests for issue #511 (quantity-type TextBox fields reject keyboard input).
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class QuantityTypeBindingRegressionTests
    {
        // -----------------------------------------------------------------------
        // Minimal Length struct — mirrors the QuantityTypes pattern
        // (see https://github.com/QuantityTypes/QuantityTypes).
        // Only the code needed to reproduce the issue is included.
        // -----------------------------------------------------------------------

        /// <summary>
        /// Minimal Length quantity struct, copied from the QuantityTypes template.
        /// </summary>
        public struct Length
        {
            /// <summary>Initializes a new instance of the <see cref="Length"/> struct.</summary>
            /// <param name="value">Value in metres.</param>
            public Length(double value)
            {
                Value = value;
            }

            /// <summary>Gets the value in metres.</summary>
            public double Value { get; }
        }

        // -----------------------------------------------------------------------
        // Custom PropertyDescriptor: exposes 'double Radius' as 'Length Radius'
        // -----------------------------------------------------------------------

        private sealed class LengthPropertyDescriptor : PropertyDescriptor
        {
            public LengthPropertyDescriptor()
                : base(
                    "Radius",
                    new Attribute[]
                    {
                        new ConverterAttribute(typeof(FakeLengthConverter)),
                        new System.ComponentModel.CategoryAttribute("Geometry"),
                    })
            {
            }

            public override Type ComponentType => typeof(SectionModel);
            public override bool IsReadOnly => false;
            public override Type PropertyType => typeof(Length);

            public override bool CanResetValue(object component) => false;
            public override void ResetValue(object component) { }
            public override bool ShouldSerializeValue(object component) => false;

            public override object GetValue(object component)
                => new Length(((SectionModel)component).Radius);

            public override void SetValue(object component, object value)
                => ((SectionModel)component).Radius = ((Length)value).Value;
        }

        // Placeholder converter type referenced by the ConverterAttribute above.
        private class FakeLengthConverter : System.Windows.Data.IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                => value is Length l ? $"{l.Value} m" : System.Windows.DependencyProperty.UnsetValue;

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
                => value is string s && double.TryParse(s.Replace(" m", string.Empty).Trim(), out var d)
                    ? (object)new Length(d)
                    : System.Windows.DependencyProperty.UnsetValue;
        }

        // -----------------------------------------------------------------------
        // SectionModel: ICustomTypeDescriptor with CLR double Radius
        // -----------------------------------------------------------------------

        /// <summary>
        /// A model that implements <see cref="ICustomTypeDescriptor"/> and exposes its
        /// <c>double Radius</c> CLR property as a <see cref="Length"/> quantity via a
        /// custom <see cref="PropertyDescriptor"/>.
        /// </summary>
        private class SectionModel : ICustomTypeDescriptor
        {
            private static readonly PropertyDescriptorCollection CustomDescriptors =
                new PropertyDescriptorCollection(new PropertyDescriptor[] { new LengthPropertyDescriptor() });

            /// <summary>Gets or sets the backing raw radius value in metres.</summary>
            public double Radius { get; set; } = 10.0;

            AttributeCollection ICustomTypeDescriptor.GetAttributes() => AttributeCollection.Empty;
            string ICustomTypeDescriptor.GetClassName() => null;
            string ICustomTypeDescriptor.GetComponentName() => null;
            TypeConverter ICustomTypeDescriptor.GetConverter() => null;
            EventDescriptor ICustomTypeDescriptor.GetDefaultEvent() => null;
            PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty() => null;
            object ICustomTypeDescriptor.GetEditor(Type editorBaseType) => null;
            EventDescriptorCollection ICustomTypeDescriptor.GetEvents() => EventDescriptorCollection.Empty;
            EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes) => EventDescriptorCollection.Empty;
            PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties() => CustomDescriptors;
            PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes) => CustomDescriptors;
            object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd) => this;
        }

        // -----------------------------------------------------------------------
        // Testable operator (exposes protected GetPropertyCollection)
        // -----------------------------------------------------------------------

        private class TestableOperator : PropertyGridOperator
        {
            public PropertyDescriptorCollection GetPropertyCollectionPublic(object instance)
                => this.GetPropertyCollection(instance);
        }

        // -----------------------------------------------------------------------
        // Tests
        // -----------------------------------------------------------------------

        /// <summary>
        /// Verifies that <see cref="PropertyGridOperator.GetPropertyCollection"/> replaces the
        /// custom <see cref="Length"/> descriptor with the CLR <c>double</c> reflection descriptor
        /// when the model implements <see cref="ICustomTypeDescriptor"/>.
        /// This demonstrates the regression: after the replacement the PropertyGrid descriptor has
        /// <c>PropertyType == double</c> instead of the expected <c>PropertyType == Length</c>.
        /// </summary>
        /// <remarks>
        /// This test PASSES when the bug is present (i.e. the replacement occurs) and is expected
        /// to FAIL once the fix is applied and the custom descriptor is preserved.
        /// </remarks>
        [Test]
        public void GetPropertyCollection_ICustomTypeDescriptorWithLengthDescriptorBackedByDouble_DescriptorPropertyTypeIsDouble()
        {
            // Arrange
            var op = new TestableOperator();
            var model = new SectionModel();

            // Act
            var props = op.GetPropertyCollectionPublic(model);
            var pd = props["Radius"];

            // Assert — with the bug, ReplaceWithReflectionDescriptors has swapped in the CLR
            // double descriptor, so PropertyType is double instead of Length.
            Assert.That(pd, Is.Not.Null, "Descriptor for 'Radius' should exist");
            Assert.That(pd.PropertyType, Is.EqualTo(typeof(double)),
                "Bug reproduced: ReplaceWithReflectionDescriptors replaced the custom Length " +
                "descriptor with the CLR double descriptor.  After the fix this assertion should " +
                "change to Is.EqualTo(typeof(Length)).");
        }

        /// <summary>
        /// Verifies that calling <see cref="PropertyDescriptor.SetValue"/> with a <see cref="Length"/>
        /// value on the reflection-based <c>double</c> descriptor throws an exception.
        /// This is the mechanism that causes keyboard input to be silently discarded: the binding's
        /// <c>ValidatesOnExceptions = true</c> catches the exception and reverts the field.
        /// </summary>
        /// <remarks>
        /// This test PASSES when the bug is present.  After the fix the descriptor should have
        /// <c>PropertyType == Length</c> and <see cref="PropertyDescriptor.SetValue"/> should succeed.
        /// </remarks>
        [Test]
        public void SetValue_LengthValueOnReplacedDoubleDescriptor_ThrowsException()
        {
            // Arrange
            var op = new TestableOperator();
            var model = new SectionModel();
            var props = op.GetPropertyCollectionPublic(model);
            var pd = props["Radius"];

            // Pre-condition: the bug has swapped in the double descriptor
            Assume.That(pd.PropertyType, Is.EqualTo(typeof(double)),
                "Pre-condition: expect the bug to be present (reflection double descriptor).");

            var lengthValue = new Length(15.0);

            // Act & Assert — SetValue with a Length on a double descriptor throws.
            // This is exactly what happens during a PropertyGrid binding update: the
            // LengthConverter.ConvertBack returns a Length and the reflection descriptor's
            // SetValue rejects it, causing the value to silently revert to "10 m".
            Assert.Throws<Exception>(
                () => pd.SetValue(model, lengthValue),
                "SetValue should throw when a Length value is supplied to a double descriptor.");
        }
    }
}
