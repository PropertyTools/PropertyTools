using System;
using System.Windows;
using System.Windows.Data;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// null to bool valueconverter
    /// </summary>
    [ValueConversion(typeof(bool), typeof(object))]
    public class NullToBoolConverter : SelfProvider, IValueConverter
    {
        public bool NullValue { get; set; }

        public NullToBoolConverter()
        {
            NullValue = true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return NullValue;
            return !NullValue;
        }

    }
}