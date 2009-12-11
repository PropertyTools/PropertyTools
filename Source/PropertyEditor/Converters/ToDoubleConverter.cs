using System;
using System.Windows.Data;
using System.Windows.Media;

namespace OpenControls
{
    /// <summary>
    /// Usage 'Converter={local:ToDoubleConverter}'
    /// </summary>
    [ValueConversion(typeof(object), typeof(double))]
    public class ToDoubleConverter : SelfProvider, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return 0;
            if (value is double)
                return (double) value;
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
//            if (targetType==typeof(double))
                return value;
//            return Binding.DoNothing;
        }
    }
}
