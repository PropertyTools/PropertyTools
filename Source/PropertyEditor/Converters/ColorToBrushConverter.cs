using System;
using System.Windows.Data;
using System.Windows.Media;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Color to Brush value converter
    /// </summary>
    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Brushes.Red;
            var color = (Color)value;
            return new SolidColorBrush(color);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((SolidColorBrush)value).Color;
        }
    }
}
