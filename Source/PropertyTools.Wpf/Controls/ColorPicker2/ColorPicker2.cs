using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PropertyTools.Wpf
{

    /// <summary>
    /// Represents a control that lets the user pick a color.
    /// </summary>
    public class ColorPicker2 : Control
    {
        static ColorPicker2()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker2), new FrameworkPropertyMetadata(typeof(ColorPicker2)));
        }

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker2),
            new FrameworkPropertyMetadata(Color.FromArgb(0, 0, 0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}
