using System;
using System.Windows;
using System.Windows.Data;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Bool to Visibility value converter
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter :IValueConverter
    {
        public bool InvertVisibility { get; set; }
        public Visibility NotVisibleValue { get; set; }

        public BoolToVisibilityConverter()
        {
            InvertVisibility = false;
            NotVisibleValue = Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((value is Visibility) && (((Visibility)value) == Visibility.Visible)) ? !InvertVisibility : InvertVisibility;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Visibility.Visible;

            bool visible=true;
            if (value is bool)
            {
                visible = (bool)value;
            }
            else if (value is bool?)
            {
                var nullable = (bool?)value;
                visible = nullable.HasValue ? nullable.Value : false;
            }


            if (InvertVisibility)
                visible = !visible;

            return visible ? Visibility.Visible : NotVisibleValue;
        }

    }
}