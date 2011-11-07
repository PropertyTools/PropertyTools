namespace TestLibrary
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using PropertyTools.DataAnnotations;

    public class TestConverterAttribute : TestBase
    {
        [Converter(typeof(RadiansToDegreesConverter))]
        public double Angle { get; set; }

        public TestConverterAttribute()
        {
            Angle = Math.PI;
        }

        public override string ToString()
        {
            return "Converter attribute";
        }
    }

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
}