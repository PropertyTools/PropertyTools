using System;
using System.Windows.Data;

namespace PropertyTools.Wpf
{
    // todo: this was included for the slidable properties - does not work

    /// <summary>
    /// Convert any object to double (if possible)
    /// </summary>
    [ValueConversion(typeof(object), typeof(double))]
    public class ToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return 0;
            
            // Explicit unboxing

            var t = value.GetType();
            if (t == typeof(int))
                return (int)value;
            if (t == typeof(long))
                return (long)value;
            if (t == typeof(byte))
                return (byte)value;
            if (t == typeof(double))
                return (double)value;
            if (t == typeof(float))
                return (float)value;

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
