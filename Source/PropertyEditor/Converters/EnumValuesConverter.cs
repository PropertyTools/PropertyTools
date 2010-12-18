using System;
using System.Windows.Data;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Converts an Enum to a list of the enum type values
    /// </summary>
    [ValueConversion(typeof(Enum), typeof(string[]))]
    public class EnumValuesConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Enum.GetValues(value.GetType());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
