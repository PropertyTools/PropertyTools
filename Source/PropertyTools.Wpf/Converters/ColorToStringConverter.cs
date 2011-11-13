// -----------------------------------------------------------------------
// <copyright file="ColorToStringConverter.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Converts <see cref="Color"/> instances to <see cref="string"/> instances..
    /// </summary>
    [ValueConversion(typeof(Color), typeof(string))]
    public class ColorToStringConverter : IValueConverter
    {
        private static Dictionary<string, Color> colors;
        public static Dictionary<string, Color> ColorMap
        {
            get
            {
                if (colors == null)
                {
                    colors = new Dictionary<string, Color>();
                    var t = typeof(Colors);
                    var fields = t.GetProperties(BindingFlags.Public | BindingFlags.Static);
                    foreach (var fi in fields)
                    {
                        var c = (Color)fi.GetValue(null, null);
                        colors.Add(fi.Name, c);
                    }
                    colors.Add("Undefined", ColorHelper.UndefinedColor);
                    colors.Add("Automatic", ColorHelper.Automatic);
                }
                return colors;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            var color = (Color)value;

            //if (ShowAsHex)
            //                    return ColorHelper.ColorToHex(color);

            string nearestColor = null;
            double nearestDist = 30;

            // find the color that is closest
            foreach (var kvp in ColorMap)
            {
                if (color == kvp.Value)
                {
                    return kvp.Key;
                }

                double d = ColorHelper.ColorDifference(color, kvp.Value);
                if (d < nearestDist)
                {
                    nearestColor = "~ " + kvp.Key; // 'kind of'
                    nearestDist = d;
                }
            }

            if (nearestColor == null)
            {
                return ColorHelper.ColorToHex(color);
            }

            if (color.A < 255)
            {
                return String.Format("{0}, {1:0} %", nearestColor, color.A / 2.55);
            }

            return nearestColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            if (s == null) return Binding.DoNothing;
            Color color;

            if (ColorMap.TryGetValue(s, out color))
            {
                return color;
            }

            var c = ColorHelper.HexToColor(s);
            if (c != ColorHelper.UndefinedColor)
                return c;

            return Binding.DoNothing;
        }
    }
}
