// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides an IValueConverter for a specified type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Windows.Data;

    /// <summary>
    /// Provides an <see cref="IValueConverter" /> for a specified type.
    /// </summary>
    /// <remarks>PropertyConverters can be registered in the <see cref="PropertyGrid" />.Converters collection.</remarks>
    public class PropertyConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyConverter" /> class.
        /// </summary>
        public PropertyConverter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyConverter" /> class.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="converter">The converter.</param>
        /// <param name="convertNullables">Convert nullable objects if set to <c>true</c>.</param>
        public PropertyConverter(Type propertyType, IValueConverter converter, bool convertNullables = true)
        {
            this.PropertyType = propertyType;
            this.Converter = converter;
            this.ConvertNullables = convertNullables;
        }

        /// <summary>
        /// Gets or sets template for this type.
        /// </summary>
        public IValueConverter Converter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to convert nullable objects.
        /// </summary>
        /// <value><c>true</c> if nullable objects should be converted; otherwise, <c>false</c>.</value>
        public bool ConvertNullables { get; set; }

        /// <summary>
        /// Gets or sets the type to edit.
        /// </summary>
        public Type PropertyType { get; set; }

        /// <summary>
        /// Gets the target type of the converter.
        /// </summary>
        /// <returns>
        /// The target type.
        /// </returns>
        public Type GetTargetType()
        {
            foreach (var a in TypeDescriptor.GetAttributes(this.Converter.GetType()))
            {
                var vca = a as ValueConversionAttribute;
                if (vca != null)
                {
                    return vca.TargetType;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified type is assignable to the EditedType.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// <c>true</c> if the specified type is assignable; otherwise, <c>false</c> .
        /// </returns>
        public bool IsAssignable(Type type)
        {
            if (this.ConvertNullables)
            {
                type = Nullable.GetUnderlyingType(type) ?? type;
            }

            return this.PropertyType.IsAssignableFrom(type);
        }
    }
}