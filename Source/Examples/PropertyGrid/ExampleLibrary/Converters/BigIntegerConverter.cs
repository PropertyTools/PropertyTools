// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BigIntegerConverter.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Converts Complex instances to string instances.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Globalization;
    using System.Numerics;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Converts <see cref="Complex" /> instances to <see cref="string" /> instances.
    /// </summary>
    [ValueConversion(typeof(Complex), typeof(string))]
    public class BigIntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BigInteger)
            {
                var c = (BigInteger)value;
                if (targetType == typeof(string))
                {
                    return c.ToString(culture);
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            if (s != null)
            {
                if (targetType == typeof(BigInteger))
                {
                    try
                    {
                        return BigInteger.Parse(s, culture);
                    }
                    catch
                    {
                        return DependencyProperty.UnsetValue;
                    }
                }
            }

            return DependencyProperty.UnsetValue;
        }
    }
}