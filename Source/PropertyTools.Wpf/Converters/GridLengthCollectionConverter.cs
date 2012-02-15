// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GridLengthCollectionConverter.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// The grid length list converter.
    /// </summary>
    public class GridLengthListConverter : TypeConverter
    {
        #region Constants and Fields

        /// <summary>
        ///   The splitter chars.
        /// </summary>
        private static readonly char[] SplitterChars = ",;".ToCharArray();

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.
        /// </param>
        /// <param name="sourceType">
        /// A <see cref="T:System.Type"/> that represents the type you want to convert from.
        /// </param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">
        /// An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.
        /// </param>
        /// <param name="culture">
        /// The <see cref="T:System.Globalization.CultureInfo"/> to use as the current culture.
        /// </param>
        /// <param name="value">
        /// The <see cref="T:System.Object"/> to convert.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.Object"/> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// The conversion cannot be performed. 
        /// </exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s != null)
            {
                var glc = new GridLengthConverter();
                var c = new List<GridLength>();
                foreach (var item in s.Split(SplitterChars))
                {
                    c.Add((GridLength)glc.ConvertFrom(item));
                }

                return c;
            }

            return base.ConvertFrom(context, culture, value);
        }

        #endregion
    }
}