namespace DataGridDemo
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class MassValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Mass)
            {
                var v = (Mass)value;
                return v.ToString();
            }

            throw new InvalidOperationException($"Cannot convert {value?.GetType()} to {targetType}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            return Mass.Parse(s, culture);
        }
    }
}