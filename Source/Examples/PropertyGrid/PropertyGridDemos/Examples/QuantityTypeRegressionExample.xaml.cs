// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuantityTypeRegressionExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Reproduces issue #511: PropertyGrid bindings break for ICustomTypeDescriptor models whose
//   ICustomTypeDescriptor-provided property descriptors have a different PropertyType from the
//   underlying CLR property (e.g. a CLR 'double Radius' exposed as a 'Length Radius' via a custom
//   PropertyDescriptor).  ReplaceWithReflectionDescriptors() substitutes the custom descriptor
//   with the reflection-based CLR descriptor, causing a type mismatch when the LengthConverter's
//   ConvertBack returns a Length and the reflection descriptor's SetValue expects a double.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using PropertyTools;
    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Interaction logic for QuantityTypeRegressionExample.
    /// Reproduces issue #511.
    /// </summary>
    [Example(
        "Quantity Type Regression (#511)",
        "Demonstrates the issue #511 regression: a SectionModel that implements ICustomTypeDescriptor and exposes a 'double Radius' CLR property as a 'Length Radius' via a custom PropertyDescriptor.  With the buggy CreateBinding() path, ReplaceWithReflectionDescriptors() replaces the custom Length descriptor with the CLR double descriptor, so the PropertyGrid shows a plain numeric spinbox instead of a 'X m' text box, and typing '15 m' is silently discarded.",
        Tags = new[] { "PropertyGrid", "Regression", "ICustomTypeDescriptor", "QuantityType" })]
    public partial class QuantityTypeRegressionExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuantityTypeRegressionExample" /> class.
        /// </summary>
        public QuantityTypeRegressionExample()
        {
            this.InitializeComponent();
        }
    }

    /// <summary>
    /// View-model for <see cref="QuantityTypeRegressionExample"/>.
    /// </summary>
    public class QuantityTypeRegressionExampleViewModel : Observable
    {
        /// <summary>
        /// Gets the section model shown in the PropertyGrid.
        /// </summary>
        public SectionModel SectionModel { get; } = new SectionModel();
    }

    // -----------------------------------------------------------------------
    // Minimal Length struct — a self-contained copy of the QuantityTypes
    // pattern (see https://github.com/QuantityTypes/QuantityTypes).
    // Only the code needed to reproduce the issue is included.
    // -----------------------------------------------------------------------

    /// <summary>
    /// Represents a length quantity (base unit: metre).
    /// Minimal copy of the QuantityTypes Length struct pattern for reproduction purposes.
    /// </summary>
    public struct Length : IFormattable
    {
        private readonly double value;

        /// <summary>Initializes a new instance of the <see cref="Length"/> struct.</summary>
        /// <param name="value">The value in metres.</param>
        public Length(double value)
        {
            this.value = value;
        }

        /// <summary>Gets the value in the base unit (metres).</summary>
        public double Value => this.value;

        /// <summary>Parses a string such as "10 m" and returns the corresponding <see cref="Length"/>.</summary>
        /// <param name="s">The string to parse.</param>
        /// <param name="provider">The format provider.</param>
        /// <returns>A <see cref="Length"/> with the parsed value.</returns>
        public static Length Parse(string s, IFormatProvider provider = null)
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            s = s.Trim();
            // Strip optional trailing unit token (e.g. "m", "mm", "cm")
            var spaceIndex = s.IndexOf(' ');
            var numericPart = spaceIndex >= 0 ? s.Substring(0, spaceIndex) : s;
            return new Length(double.Parse(numericPart, provider ?? CultureInfo.InvariantCulture));
        }

        /// <inheritdoc/>
        public override string ToString() => this.ToString(null, CultureInfo.CurrentCulture);

        /// <inheritdoc/>
        public string ToString(string format, IFormatProvider formatProvider)
            => this.value.ToString(format, formatProvider) + " m";
    }

    // -----------------------------------------------------------------------
    // IValueConverter: Length <-> string
    // -----------------------------------------------------------------------

    /// <summary>
    /// Converts between <see cref="Length"/> and its string representation (e.g. "10 m").
    /// </summary>
    [ValueConversion(typeof(Length), typeof(string))]
    public class LengthToStringConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Length length)
            {
                return length.ToString(null, culture);
            }

            return DependencyProperty.UnsetValue;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                try
                {
                    return Length.Parse(s, culture);
                }
                catch (FormatException)
                {
                    return DependencyProperty.UnsetValue;
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }

    // -----------------------------------------------------------------------
    // Custom PropertyDescriptor: presents 'double Radius' as 'Length Radius'
    // -----------------------------------------------------------------------

    /// <summary>
    /// A <see cref="PropertyDescriptor"/> that wraps a <c>double</c> CLR property on
    /// <see cref="SectionModel"/> and exposes it as a <see cref="Length"/> quantity.
    /// The PropertyGrid is expected to pick up the <see cref="ConverterAttribute"/> and
    /// display the value as "X m".
    /// </summary>
    internal sealed class LengthPropertyDescriptor : PropertyDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LengthPropertyDescriptor"/> class.
        /// </summary>
        public LengthPropertyDescriptor()
            : base(
                "Radius",
                new Attribute[]
                {
                    new ConverterAttribute(typeof(LengthToStringConverter)),
                    new CategoryAttribute("Geometry"),
                    new DisplayNameAttribute("Radius"),
                    new DescriptionAttribute("The radius of the section in metres."),
                })
        {
        }

        /// <inheritdoc/>
        public override Type ComponentType => typeof(SectionModel);

        /// <inheritdoc/>
        public override bool IsReadOnly => false;

        /// <inheritdoc/>
        public override Type PropertyType => typeof(Length);

        /// <inheritdoc/>
        public override bool CanResetValue(object component) => false;

        /// <inheritdoc/>
        public override void ResetValue(object component) { }

        /// <inheritdoc/>
        public override bool ShouldSerializeValue(object component) => false;

        /// <inheritdoc/>
        public override object GetValue(object component)
            => new Length(((SectionModel)component).Radius);

        /// <inheritdoc/>
        public override void SetValue(object component, object value)
            => ((SectionModel)component).Radius = ((Length)value).Value;
    }

    // -----------------------------------------------------------------------
    // SectionModel: ICustomTypeDescriptor whose ICustomTypeDescriptor-provided
    // descriptor has PropertyType = Length while the backing CLR property has
    // PropertyType = double.
    // -----------------------------------------------------------------------

    /// <summary>
    /// A model that implements <see cref="ICustomTypeDescriptor"/> and exposes its
    /// <c>double Radius</c> backing property as a <see cref="Length"/> quantity via a
    /// custom <see cref="PropertyDescriptor"/>.
    /// <para>
    /// This pattern reproduces the regression described in issue #511: after commit cf4106b,
    /// <c>PropertyGridOperator.ReplaceWithReflectionDescriptors</c> finds the CLR
    /// <c>double Radius</c> property and substitutes the custom <see cref="Length"/>
    /// descriptor with a reflection-based <c>double</c> descriptor.  As a result
    /// the PropertyGrid renders a plain numeric spinbox instead of a "X m" text box,
    /// and any Length value returned by the converter's ConvertBack is silently
    /// discarded because the double descriptor's SetValue rejects it.
    /// </para>
    /// </summary>
    public class SectionModel : ICustomTypeDescriptor, INotifyPropertyChanged
    {
        private static readonly PropertyDescriptorCollection CustomDescriptors =
            new PropertyDescriptorCollection(new PropertyDescriptor[] { new LengthPropertyDescriptor() });

        private double radius = 10.0;

        /// <summary>
        /// Gets or sets the raw radius value in metres (the CLR backing property).
        /// The ICustomTypeDescriptor exposes this as a <see cref="Length"/> quantity
        /// under the name "Radius".
        /// </summary>
        public double Radius
        {
            get => this.radius;
            set
            {
                if (this.radius != value)
                {
                    this.radius = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Radius)));
                }
            }
        }

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        // ---- ICustomTypeDescriptor ------------------------------------------

        AttributeCollection ICustomTypeDescriptor.GetAttributes() => AttributeCollection.Empty;
        string ICustomTypeDescriptor.GetClassName() => nameof(SectionModel);
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
}
