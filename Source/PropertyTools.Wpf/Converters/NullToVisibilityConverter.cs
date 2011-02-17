using System;
using System.Windows;
using System.Windows.Data;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Null to Visibility converter
    /// </summary>
    [ValueConversion(typeof(Visibility), typeof(object))]
    public class NullToVisibilityConverter : IValueConverter
    {
        public Visibility NullVisibility { get; set; }
        public Visibility NotNullVisibility { get; set; }

        public NullToVisibilityConverter()
        {
            NullVisibility = Visibility.Collapsed;
            NotNullVisibility = Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return NullVisibility;
            return NotNullVisibility;
        }

    }
}