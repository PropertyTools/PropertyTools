using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using PropertyEditorLibrary.Controls.ColorPicker;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Color to Brush value converter
    /// </summary>
    [ValueConversion(typeof(ColorWrapper), typeof(SolidColorBrush))]
    public class ColorWrapperToBrushConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Binding.DoNothing : ((ColorWrapper)value).Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new ColorWrapper((Color)value);
        }

        #endregion
    }
}