namespace DataGridDemo
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class MassValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (Mass)value;
            return v.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            return Mass.Parse(s, culture);
        }
    }
}