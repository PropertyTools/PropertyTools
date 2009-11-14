using System;
using System.Windows.Data;
using System.Windows.Media;

namespace OpenControls
{
    /// <summary>
    /// Usage 'Converter={local:ColorToHexConverter}'
    /// </summary>
    [ValueConversion(typeof(Color), typeof(string))]
    public class ColorToHexConverter : SelfProvider, IValueConverter
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
