// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestConverterAttribute.cs" company="PropertyTools">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
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
// --------------------------------------------------------------------------------------------------------------------
namespace TestLibrary
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Data;

    using PropertyTools.DataAnnotations;

    public class TestConverterAttribute : TestBase
    {
        [Category("IValueConverter")]
        [Converter(typeof(RadiansToDegreesConverter))]
        public double Angle { get; set; }

        [Converter(typeof(IntToBoolConverter))]
        public int Value { get; set; }

        [Converter(typeof(ColorConverter))]
        [Description("System.Drawing.Color")]
        public System.Drawing.Color Color { get; set; }

        [Category("TypeConverter")]
        public Mass Mass { get; set; }

        public Length Length { get; set; }

        public TestConverterAttribute()
        {
            Angle = Math.PI;
        }

        public override string ToString()
        {
            return "Converter attribute";
        }
    }

    [ValueConversion(typeof(System.Drawing.Color), typeof(System.Windows.Media.Color))]
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;
            var c = (System.Drawing.Color)value;
            return System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;
            var c = (System.Windows.Media.Color)value;
            return System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);
        }
    }

    [ValueConversion(typeof(double), typeof(double))]
    public class RadiansToDegreesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double v = (double)value;
            return v / Math.PI * 180;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = System.Convert.ToDouble(value);
            return v / 180 * Math.PI;
        }
    }

    [ValueConversion(typeof(int), typeof(bool))]
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int v = (int)value;
            return v != 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = System.Convert.ToBoolean(value);
            return v ? 1 : 0;
        }
    }
}