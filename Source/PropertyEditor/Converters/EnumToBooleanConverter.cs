using System;
using System.Globalization;
using System.Windows.Data;

namespace OpenControls
{
    /// <summary>
    /// Usage 'Converter={local:EnumToBooleanConverter}'
    /// </summary>
    [ValueConversion(typeof (Enum), typeof (bool))]
    public class EnumToBooleanConverter : SelfProvider, IValueConverter
    {
        #region IValueConverter Members

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
                    return Enum.Parse(targetType, parameter.ToString());
            }
            catch (ArgumentException)
            {
            } // Ignore, just return DoNothing.
            catch (FormatException)
            {
            } // Ignore, just return DoNothing.
            return Binding.DoNothing;
        }

        #endregion
    }
}