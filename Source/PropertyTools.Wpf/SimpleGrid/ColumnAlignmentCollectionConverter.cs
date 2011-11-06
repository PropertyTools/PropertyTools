// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColumnAlignmentCollectionConverter.cs" company="PropertyTools">
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
    /// The column alignment list converter.
    /// </summary>
    public class ColumnAlignmentListConverter : TypeConverter
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
                var c = new List<HorizontalAlignment>();
                foreach (var item in s.Split(SplitterChars))
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var itemL = item.ToLower();
                        if (itemL.StartsWith("l"))
                        {
                            c.Add(HorizontalAlignment.Left);
                            continue;
                        }

                        if (itemL.StartsWith("r"))
                        {
                            c.Add(HorizontalAlignment.Right);
                            continue;
                        }

                        if (itemL.StartsWith("s"))
                        {
                            c.Add(HorizontalAlignment.Stretch);
                            continue;
                        }
                    }

                    c.Add(HorizontalAlignment.Center);
                }

                return c;
            }

            return base.ConvertFrom(context, culture, value);
        }

        #endregion
    }
}