using System;
using System.Windows.Data;
using System.Windows.Media;

namespace OpenControls
{
    /// <summary>
    /// Usage 'Converter={local:ColorToBrushConverter}'
    /// </summary>
    [ValueConversion(typeof(Color), typeof(SolidColorBrush))]
    public class ColorToBrushConverter : SelfProvider, IValueConverter
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
