// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComplexConverter.cs" company="PropertyTools">
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
//   Converts <see cref="Complex" /> instances to <see cref="string" /> instances.
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