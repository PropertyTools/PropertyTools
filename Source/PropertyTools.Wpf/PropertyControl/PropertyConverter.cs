// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyConverter.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents a property converter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Windows.Data;

    /// <summary>
    /// Provides an <see cref="IValueConverter"/> for a specified type.
    /// </summary>
    /// <remarks>
    /// PropertyConverters can be registered in the <see cref="PropertyControl"/>.Converters collection.
    /// </remarks>
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
        /// <returns> The target type. </returns>
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
        /// <param name="type">
        /// The type.
        /// </param>
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