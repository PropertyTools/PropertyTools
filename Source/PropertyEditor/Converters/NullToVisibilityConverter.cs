using System;
using System.Windows;
using System.Windows.Data;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Usage 'Converter={local:NullToVisibilityConverter}'
    /// </summary>
    [ValueConversion(typeof(Visibility), typeof(object))]
    public class NullToVisibilityConverter : SelfProvider, IValueConverter
    {
        public Visibility NullVisibility { get; set; }
        public Visibility NotNullVisibility { get; set; }

        public NullToVisibilityConverter()
        {
            NullVisibility = Visibility.Collapsed;
            NotNullVisibility = Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return NullVisibility;
            return NotNullVisibility;
        }

    }
}