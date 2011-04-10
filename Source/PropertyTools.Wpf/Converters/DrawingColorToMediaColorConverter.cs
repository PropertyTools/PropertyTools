using System;
using System.Windows.Data;
using System.Windows.Media;

namespace PropertyTools.Wpf
{
    /// <summary>
	/// System.Windows.Media.Color to System.Drawing.Color value converter
    /// </summary>
	[ValueConversion( typeof( System.Windows.Media.Color), typeof( System.Drawing.Color ) )]
	public class DrawingColorToMediaColorConverter : IValueConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			// System.Windows.Media.Color --> System.Drawing.Color
			Color c = (Color)value;
			return System.Drawing.Color.FromArgb( c.A, c.R, c.G, c.B );
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			// System.Drawing.Color -->  System.Windows.Media.Color
			System.Drawing.Color c = (System.Drawing.Color)value;
			return Color.FromArgb( c.A, c.R, c.G, c.B );
        }
    }
}