using System;
using System.Globalization;
using System.Windows.Data;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Enum to Boolean converter
    /// Usage 'Converter={StaticResource EnumToBooleanConverter}, ConverterParameter={x:Static value...}'
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumToBooleanConverter : IValueConverter
    {
        public Type EnumType { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Binding.DoNothing;
            string checkValue = value.ToString();
            string targetValue = parameter.ToString();
            return checkValue.Equals(targetValue, StringComparison.OrdinalIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Binding.DoNothing;
            try
            {
                bool boolValue = System.Convert.ToBoolean(value, culture);
                if (boolValue)
                    return Enum.Parse(EnumType, parameter.ToString());
            }
            catch (ArgumentException)
            {
            } // Ignore, just return DoNothing.
            catch (FormatException)
            {
            } // Ignore, just return DoNothing.
            return Binding.DoNothing;
        }
    }
}