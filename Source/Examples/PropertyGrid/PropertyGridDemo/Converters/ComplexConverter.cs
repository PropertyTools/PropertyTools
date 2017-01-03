// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexConverter.cs" company="PropertyTools">
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
    public class ComplexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Complex)
            {
                var c = (Complex)value;
                if (targetType == typeof(string))
                {
                    return c.ToString(culture);
                }
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                var c = (string)value;
                if (targetType == typeof(Complex))
                {
                    try
                    {
                        c = c.Trim("()".ToCharArray());
                        int i = c.IndexOf(", ", StringComparison.InvariantCultureIgnoreCase);
                        if (i > 0)
                        {
                            var realPart = c.Substring(0, i);
                            var imagPart = c.Substring(i + 2);
                            var real = double.Parse(realPart, culture);
                            var imag = double.Parse(imagPart, culture);
                            return new Complex(real, imag);
                        }

                        return new Complex(double.Parse(c, culture), 0);
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