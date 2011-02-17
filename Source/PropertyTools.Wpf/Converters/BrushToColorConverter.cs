using System;
using System.Windows.Data;
using System.Windows.Media;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Brush to Color value converter
    /// </summary>
    [ValueConversion(typeof(Brush), typeof(Color))]
    public class BrushToColorConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Brushes.Red;
            var color = (Color)value;
            return new SolidColorBrush(color);
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var scb = value as SolidColorBrush;
            if (scb != null)
                return scb.Color;

            // todo: other brush types??

            return Color.FromArgb(0, 0, 0, 0);
        }
    }
}