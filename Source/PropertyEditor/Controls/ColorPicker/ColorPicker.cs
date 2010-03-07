using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System;
using System.Reflection;

namespace PropertyEditorLibrary
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
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker),
            new FrameworkPropertyMetadata(Color.FromArgb(80, 255, 255, 0), 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                SelectedColorChanged));

        private static void SelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).OnSelectedValueChanged();
        }

        private void OnSelectedValueChanged()
        {
            var hsv = ColorHelper.ColorToHsvBytes(SelectedColor);
            hue = hsv[0];
            saturation = hsv[1];
            brightness = hsv[2];
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
                // todo: localize...
                if (SelectedColor.A == 0)
                    return "Transparent";
                var t = typeof(Colors);
                var fields = t.GetProperties(BindingFlags.Public | BindingFlags.Static);
                string best = "Custom";
                double bestDist = 30;
                // find the color that is closest
                foreach (var fi in fields)
                {
                    var c = (Color)fi.GetValue(null, null);
                    if (SelectedColor == c)
                        return fi.Name;
                    double d = ColorHelper.ColorDifference(SelectedColor, c);
                    if (d < bestDist)
                    {
                        best = "~ " + fi.Name; // 'kind of'
                        bestDist = d;
                    }
                }
                if (SelectedColor.A < 255)
                {
                    return String.Format("{0}, {1:0} %", best, SelectedColor.A / 2.55);
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

        private byte hue;
        private byte saturation;
        private byte brightness;

        public byte Hue
        {
            get { return hue; }
            set { SelectedColor = ColorHelper.HsvToColor(value, Saturation, Brightness); }
        }
        public byte Saturation
        {
            get { return saturation; }
            set { SelectedColor = ColorHelper.HsvToColor(Hue, value, Brightness); }
        }
        public byte Brightness
        {
            get { return brightness; }
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
            var palette = new ObservableCollection<Color>();
            palette.Add(Colors.White);
            palette.Add(Color.FromRgb(192, 192, 192));
            palette.Add(Color.FromRgb(128, 128, 128));
            palette.Add(Color.FromRgb(64, 64, 64));
            palette.Add(Color.FromRgb(0, 0, 0));

            // Add a rainbow of colors
            int N = 32 - 5;
            for (int i = 0; i < N; i++)
            {
                double h = 0.8 * i / (N - 1);
                var c = ColorHelper.HsvToColor(h, 1.0, 1.0);
                palette.Add(c);
            }
            return palette;
        }
    }
}
