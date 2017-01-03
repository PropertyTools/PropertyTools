// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConverterAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class ConverterAttributeExample : Example
    {
        public ConverterAttributeExample()
        {
            this.Angle = Math.PI;
        }

        [Category("IValueConverter")]
        [Converter(typeof(RadiansToDegreesConverter))]
        public double Angle { get; set; }

        [Converter(typeof(IntToBoolConverter))]
        public int Value { get; set; }

        [Converter(typeof(ColorConverter))]
        [Description("System.Drawing.Color")]
        public System.Drawing.Color Color { get; set; }

        [Description("The value converter is registered in the property control factory.")]
        public Length Length { get; set; }

        [Category("TypeConverter")]
        [Description("The type converter is registered by the TypeConverterAttribute on the Mass type.")]
        public Mass Mass { get; set; }
    }

    [ValueConversion(typeof(System.Drawing.Color), typeof(System.Windows.Media.Color))]
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

            var c = (System.Drawing.Color)value;
            return System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return DependencyProperty.UnsetValue;
            }

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
            try
            {
                var v = System.Convert.ToDouble(value);
                return v / 180 * Math.PI;
            }
            catch (FormatException)
            {
                return DependencyProperty.UnsetValue;
            }
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