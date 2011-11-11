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
            var c=(System.Drawing.Color)value;
            return System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
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

    [TypeConverter(typeof(MassConverter))]
    public struct Mass
    {
        public double Value { get; set; }

        public static Mass Parse(string s)
        {
            s = s.Replace("kg", "").Trim();
            double value = double.Parse(s);
            return new Mass { Value = value };
        }

        public override string ToString()
        {
            return Value + " kg";
        }
    }

    public class MassConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
    }

    [TypeConverter(typeof(LengthConverter))]
    public struct Length
    {
        public double Value { get; set; }

        internal static Length Parse(string s)
        {
            s = s.Replace("m", "").Trim();
            double value = double.Parse(s);
            return new Length { Value = value };
        }

        public override string ToString()
        {
            return Value + " m";
        }
    }

    public class LengthConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string)) return true;
            return base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var s = (string)value;
                return Length.Parse(s);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}