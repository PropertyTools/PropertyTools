// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MassConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.ComponentModel;

    public class MassConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            var s = value as string;
            if (s != null)
            {
                return Mass.Parse(s, culture);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}