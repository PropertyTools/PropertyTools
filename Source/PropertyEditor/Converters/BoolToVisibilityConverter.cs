using System;
using System.Windows;
using System.Windows.Data;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Usage 'Converter={local:BoolToVisibilityConverter}'
    /// </summary>
    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class BoolToVisibilityConverter : SelfProvider, IValueConverter
    {
        public bool InvertVisibility
        { get; set; }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Visible;

            bool visible = (bool)value;

            if (InvertVisibility)
                visible = !visible;

            if (visible)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Hidden;
            }
        }

    }
}