using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System;
using System.Reflection;
using System.Windows.Threading;
using System.Windows.Input;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// ColorPicker
    /// todo: 
    /// - close popup when clicking outside...
    /// - localize strings...
    /// - more palettes
    /// </summary>
    public class ColorPicker : Control, INotifyPropertyChanged
    {
        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

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
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(false, IsDropDownOpenChanged));

        private static void IsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).IsDropDownOpenChanged();
        }

        private void IsDropDownOpenChanged()
        {
            // turn off picking when drop down is closed
            if (!IsDropDownOpen && IsPicking)
                IsPicking = false;
        }

        /// <summary>
        /// Gets or sets if picking colors from the screen is active.
        /// Use the 'SHIFT' button to select colors when this mode is active.
        /// </summary>
        public bool IsPicking
        {
            get { return (bool)GetValue(IsPickingProperty); }
            set { SetValue(IsPickingProperty, value); }
        }

        public static readonly DependencyProperty IsPickingProperty =
            DependencyProperty.Register("IsPicking", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(false, IsPickingChanged));

        private static void IsPickingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).IsPickingChanged();
        }

        private DispatcherTimer pickingTimer;
        private void IsPickingChanged()
        {
            if (IsPicking && pickingTimer == null)
            {
                pickingTimer = new DispatcherTimer();
                pickingTimer.Interval = TimeSpan.FromMilliseconds(100);
                pickingTimer.Tick += Pick;
                pickingTimer.Start();
            }
            if (!IsPicking && pickingTimer != null)
            {
                pickingTimer.Tick -= Pick;
                pickingTimer.Stop();
                pickingTimer = null;
            }
        }

        private void Pick(object sender, EventArgs e)
        {
            bool isShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            if (!isShiftDown)
                return;

            try
            {
                var pt = CaptureScreenshot.GetMouseScreenPosition();
                var bmp = CaptureScreenshot.Capture(new Rect(pt, new Size(1, 1)));
                byte[] pixels = new byte[4];
                bmp.CopyPixels(pixels, 4, 0);
                SelectedColor = Color.FromArgb(0xFF, pixels[2], pixels[1], pixels[0]);
            }
            catch (Exception)
            {

            }
        }



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

        private bool updateHSV = true;

        private void OnSelectedValueChanged()
        {
            if (updateHSV)
            {
                var hsv = ColorHelper.ColorToHsvBytes(SelectedColor);
                hue = hsv[0];
                saturation = hsv[1];
                brightness = hsv[2];
                OnPropertyChanged("Hue");
                OnPropertyChanged("Saturation");
                OnPropertyChanged("Brightness");
            }
            OnPropertyChanged("Red");
            OnPropertyChanged("Green");
            OnPropertyChanged("Blue");
            OnPropertyChanged("Alpha");
            OnPropertyChanged("ColorName");
            OnPropertyChanged("AlphaGradient");
            OnPropertyChanged("SaturationGradient");
            OnPropertyChanged("BrightnessGradient");
        }

        public Brush AlphaGradient
        {
            get
            {
                return new LinearGradientBrush(Colors.Transparent, Color.FromRgb(Red, Green, Blue), 0);
            }
        }

        public Brush SaturationGradient
        {
            get
            {
                return new LinearGradientBrush(ColorHelper.HsvToColor(Hue, 0, Brightness), ColorHelper.HsvToColor(Hue, 255, Brightness), 0);
            }
        }

        public Brush BrightnessGradient
        {
            get
            {
                return new LinearGradientBrush(ColorHelper.HsvToColor(Hue, Saturation, 0), ColorHelper.HsvToColor(Hue, Saturation, 255), 0);
            }
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
            set
            {
                updateHSV = false;
                SelectedColor = ColorHelper.HsvToColor(value, Saturation, Brightness);
                hue = value;
                OnPropertyChanged("Hue");
            }
        }
        public byte Saturation
        {
            get { return saturation; }
            set
            {
                updateHSV = false;
                SelectedColor = ColorHelper.HsvToColor(Hue, value, Brightness);
                updateHSV = true;
                saturation = value;
                OnPropertyChanged("Saturation");
            }
        }
        public byte Brightness
        {
            get { return brightness; }
            set
            {
                updateHSV = false;
                SelectedColor = ColorHelper.HsvToColor(Hue, Saturation, value);
                updateHSV = true;
                brightness = value;
                OnPropertyChanged("Brightness");
            }
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

            // standard colors
            palette.Add(Colors.Transparent);
            palette.Add(Colors.White);
            palette.Add(Colors.Silver);
            palette.Add(Colors.Gray);
            palette.Add(Colors.DarkSlateGray);
            palette.Add(Colors.Black);

            palette.Add(Colors.Firebrick);
            palette.Add(Colors.Red);
            palette.Add(Colors.Gold);
            palette.Add(Colors.Yellow);
            palette.Add(Colors.YellowGreen);
            palette.Add(Colors.SeaGreen);
            palette.Add(Colors.DeepSkyBlue);
            palette.Add(Colors.DarkCyan);
            palette.Add(Colors.MidnightBlue);
            palette.Add(Colors.DarkOrchid);

            // Add a rainbow of colors
            /*int N = 32 - 5;
            for (int i = 0; i < N; i++)
            {
                double h = 0.8 * i / (N - 1);
                var c = ColorHelper.HsvToColor(h, 1.0, 1.0);
                palette.Add(c);
            }*/
            return palette;
        }
    }
}
