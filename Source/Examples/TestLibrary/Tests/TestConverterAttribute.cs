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