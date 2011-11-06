// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringCollectionConverter.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;

    /// <summary>
    /// The string list converter.
    /// </summary>
    [TypeConverter(typeof(IList<string>))]
    public class StringListConverter : TypeConverter
    {
        #region Constants and Fields

        /// <summary>
        /// The splitter chars.
        /// </summary>
        private static readonly char[] SplitterChars = ",;".ToCharArray();

        #endregion

        #region Public Methods

        /// <summary>
        /// The can convert from.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="sourceType">
        /// The source type.
        /// </param>
        /// <returns>
        /// The can convert from.
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
        /// The convert from.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="culture">
        /// The culture.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The convert from.
        /// </returns>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            if (s != null)
            {
                var sc = new List<string>();
                foreach (var item in s.Split(SplitterChars))
                {
                    sc.Add(item);
                }

                return sc;
            }

            return base.ConvertFrom(context, culture, value);
        }

        #endregion
    }
}