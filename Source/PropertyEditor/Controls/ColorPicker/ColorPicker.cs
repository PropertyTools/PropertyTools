using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System;
using System.Reflection;

namespace OpenControls
{
    public class ColorPicker : Control, INotifyPropertyChanged
    {
        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

       /* public ColorPicker()
        {
                AddDefaultPalette();
        }*/
        public ObservableCollection<Color> Palette
        {
            get { return (ObservableCollection<Color>)GetValue(PaletteProperty); }
            set { SetValue(PaletteProperty, value); }
        }

        public static readonly DependencyProperty PaletteProperty =
            DependencyProperty.Register("Palette", typeof(ObservableCollection<Color>), typeof(ColorPicker), new UIPropertyMetadata(CreateDefaultPalette()));



        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(false));


        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker), new UIPropertyMetadata(Color.FromArgb(80, 255, 255, 0), SelectedColorChanged));

        private static void SelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).OnSelectedValueChanged();
        }

        private void OnSelectedValueChanged()
        {
            var hsv = ColorHelper.ColorToHsv(SelectedColor);
            _hue = hsv[0];
            _saturation = hsv[1];
            _brightness = hsv[2];
            OnPropertyChanged("Red");
            OnPropertyChanged("Green");
            OnPropertyChanged("Blue");
            OnPropertyChanged("Alpha");
            OnPropertyChanged("Hue");
            OnPropertyChanged("Saturation");
            OnPropertyChanged("Brightness");
            OnPropertyChanged("ColorName");
        }

        public string ColorName
        {
            get
            {
                var t = typeof (Colors);
                var fields = t.GetProperties(BindingFlags.Public | BindingFlags.Static);
                string best = "Custom";
                double bestDist = 30;
                foreach (var fi in fields)
                {
                    var c = (Color)fi.GetValue(null,null);
                    if (SelectedColor==c)
                        return fi.Name;
                    double d = ColorHelper.ColorDistance(SelectedColor, c);
                    if (d<bestDist)
                    {
                        best = fi.Name;
                        bestDist = d;
                    }
                }
                return best;
            }
        }
        public byte Red
        {
            get { return SelectedColor.R; }
            set { SelectedColor = Color.FromArgb(Alpha, value, Green, Blue); }
        }
        public byte Green
        {
            get { return SelectedColor.G; }
            set { SelectedColor = Color.FromArgb(Alpha, Red, value, Blue); }
        }
        public byte Blue
        {
            get { return SelectedColor.B; }
            set { SelectedColor = Color.FromArgb(Alpha, Red, Green, value); }
        }
        public byte Alpha
        {
            get { return SelectedColor.A; }
            set { SelectedColor = Color.FromArgb(value, Red, Green, Blue); }
        }

        private byte _hue;
        private byte _saturation;
        private byte _brightness;

        public byte Hue
        {
            get { return _hue; }
            set { SelectedColor = ColorHelper.HsvToColor(value,Saturation,Brightness); }
        }
        public byte Saturation
        {
            get { return _saturation; }
            set { SelectedColor = ColorHelper.HsvToColor(Hue, value, Brightness); }
        }
        public byte Brightness
        {
            get { return _brightness; }
            set { SelectedColor = ColorHelper.HsvToColor(Hue, Saturation, value); }
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public static ObservableCollection<Color> CreateDefaultPalette()
        {
            var Palette = new ObservableCollection<Color>();
            Palette.Add(Colors.White);
            Palette.Add(Color.FromRgb(192, 192, 192));
            Palette.Add(Color.FromRgb(128, 128, 128));
            Palette.Add(Color.FromRgb(64, 64, 64));
            Palette.Add(Color.FromRgb(0, 0, 0));

            // Add a rainbow of colors
            int N = 32 - 5;
            for (int i = 0; i < N; i++)
            {
                double H = 0.8*i/(N - 1);
                var c = ColorHelper.HsvToColor(H, 1.0, 1.0);
                Palette.Add(c);
            }
            return Palette;
        }
    }
}
