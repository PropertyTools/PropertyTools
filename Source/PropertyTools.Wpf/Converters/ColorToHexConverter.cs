using System;
using System.Windows.Data;
using System.Windows.Media;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Color to Hex string value converter
    /// </summary>
    [ValueConversion(typeof(Color), typeof(string))]
    public class ColorToHexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            var color = (Color)value;
            return ColorHelper.ColorToHex(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ColorHelper.HexToColor((string)value);
        }
    }
}
