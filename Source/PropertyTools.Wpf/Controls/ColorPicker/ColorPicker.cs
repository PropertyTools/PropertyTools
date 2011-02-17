using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// ColorPicker control
    /// </summary>
    public partial class ColorPicker : Control, INotifyPropertyChanged
    {
        // todo: 
        // - localize strings...
        // - more palettes
        // - persist palette - in static list, and static load/save methods?
        //   - the user can also bind the Palette and do the persisting 
        // - 'automatic' color? 'IncludeAutoColor' dependency property?

        public static readonly DependencyProperty ShowAsHexProperty =
            DependencyProperty.Register("ShowAsHex", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(false));

        public static readonly DependencyProperty PaletteProperty =
            DependencyProperty.Register("Palette", typeof(ObservableCollection<Color>), typeof(ColorPicker),
                                        new UIPropertyMetadata(CreateDefaultPalette()));

        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(ColorPicker),
                                        new UIPropertyMetadata(false, IsDropDownOpenChanged, CoerceIsDropDownOpen));

        public static readonly DependencyProperty IsPickingProperty =
            DependencyProperty.Register("IsPicking", typeof(bool), typeof(ColorPicker),
                                        new UIPropertyMetadata(false, IsPickingChanged));

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker),
                                        new FrameworkPropertyMetadata(Color.FromArgb(80, 255, 255, 0),
                                                                      FrameworkPropertyMetadataOptions.
                                                                          BindsTwoWayByDefault,
                                                                      SelectedColorChanged));

        public static readonly DependencyProperty TabStripPlacementProperty =
            DependencyProperty.Register("TabStripPlacement", typeof(Dock), typeof(ColorPicker),
                                        new UIPropertyMetadata(Dock.Bottom));


        private byte brightness;
        private byte hue;
        private DispatcherTimer pickingTimer;
        private byte saturation;
        private bool updateHSV = true;
        private bool updateHexValue = true;

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker),
                                                     new FrameworkPropertyMetadata(typeof(ColorPicker)));
            EventManager.RegisterClassHandler(typeof(ColorPicker), Mouse.MouseDownEvent, new MouseButtonEventHandler(OnMouseButtonDown), true);
            EventManager.RegisterClassHandler(typeof(ColorPicker), GotFocusEvent, new RoutedEventHandler(OnGotFocus));
        }

        private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var picker = (ColorPicker)sender;

            if (!picker.IsKeyboardFocusWithin)
            {
                picker.Focus();
            }

            e.Handled = true;
            if ((Mouse.Captured == picker) && (e.OriginalSource == picker))
            {
                picker.CloseDropDown();
            }
        }

        private void CloseDropDown()
        {
            if (this.IsDropDownOpen)
            {
                this.ClearValue(IsDropDownOpenProperty);
                if (this.IsDropDownOpen)
                {
                    this.IsDropDownOpen = false;
                }
            }
        }

        private void ToggleDropDown()
        {
            if (this.IsDropDownOpen)
            {
                this.CloseDropDown();
            }
            else
            {
                this.IsDropDownOpen = true;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            var handled = false;
            switch (e.Key)
            {
                case Key.F4:
                    this.ToggleDropDown();
                    handled = true;
                    break;
                case Key.Enter:
                    this.CloseDropDown();
                    handled = true;
                    break;
                case Key.Escape:
                    this.CloseDropDown();
                    handled = true;
                    break;
            }

            e.Handled = handled;
        }

        private static void OnGotFocus(object sender, RoutedEventArgs e)
        {
            var picker = (ColorPicker)sender;
            if (!e.Handled && picker.staticColorList != null)
            {
                if (e.OriginalSource == picker)
                {
                    picker.staticColorList.Focus();
                    e.Handled = true;
                }
                else if (e.OriginalSource == picker.staticColorList)
                {
                    picker.staticColorList.Focus();
                }
            }
        }

        private ListBox staticColorList;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            staticColorList = Template.FindName("PART_StaticColorList", this) as ListBox;
        }


        public ColorPicker()
        {
            Loaded += ColorPicker_Loaded;
            mouseEvent = new MouseButtonEventHandler(PaletteList_MouseUp);
        }

        public Dock TabStripPlacement
        {
            get { return (Dock)GetValue(TabStripPlacementProperty); }
            set { SetValue(TabStripPlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether show as color names as hex strings.
        /// </summary>
        /// <value><c>true</c> if show as hex; otherwise, <c>false</c>.</value>
        public bool ShowAsHex
        {
            get { return (bool)GetValue(ShowAsHexProperty); }
            set { SetValue(ShowAsHexProperty, value); }
        }

        public ObservableCollection<Color> Palette
        {
            get { return (ObservableCollection<Color>)GetValue(PaletteProperty); }
            set { SetValue(PaletteProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the color picker popup is open.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this popup is open; otherwise, <c>false</c>.
        /// </value>
        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
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

        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        /// <value>The color of the selected.</value>
        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        public Brush AlphaGradient
        {
            get { return new LinearGradientBrush(Colors.Transparent, Color.FromRgb(Red, Green, Blue), 0); }
        }

        public Brush SaturationGradient
        {
            get
            {
                return new LinearGradientBrush(ColorHelper.HsvToColor(Hue, 0, Brightness),
                                               ColorHelper.HsvToColor(Hue, 255, Brightness), 0);
            }
        }

        public Brush BrightnessGradient
        {
            get
            {
                return new LinearGradientBrush(ColorHelper.HsvToColor(Hue, Saturation, 0),
                                               ColorHelper.HsvToColor(Hue, Saturation, 255), 0);
            }
        }

        public string ColorName
        {
            get
            {
                if (ShowAsHex)
                    return ColorHelper.ColorToHex(SelectedColor);
                Type t = typeof(Colors);
                PropertyInfo[] fields = t.GetProperties(BindingFlags.Public | BindingFlags.Static);
                string nearestColor = "Custom";
                double nearestDist = 30;
                // find the color that is closest
                foreach (PropertyInfo fi in fields)
                {
                    var c = (Color)fi.GetValue(null, null);
                    if (SelectedColor == c)
                        return fi.Name;
                    double d = ColorHelper.ColorDifference(SelectedColor, c);
                    if (d < nearestDist)
                    {
                        nearestColor = "~ " + fi.Name; // 'kind of'
                        nearestDist = d;
                    }
                }
                if (SelectedColor.A < 255)
                {
                    return String.Format("{0}, {1:0} %", nearestColor, SelectedColor.A / 2.55);
                }
                return nearestColor;
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

        public string HexValue
        {
            get
            {
                return ColorHelper.ColorToHex(SelectedColor);
            }
            set
            {
                updateHexValue = false;
                SelectedColor = ColorHelper.HexToColor(value);
                updateHexValue = true;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void ColorPicker_Loaded(object sender, RoutedEventArgs e)
        {
            InitializePaletteSettings();
            LoadLastPalette();
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static void IsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).IsDropDownOpenChanged(e);
        }

        private static object CoerceIsDropDownOpen(DependencyObject d, object basevalue)
        {
            return ((ColorPicker) d).OnCoerceIsDropDownOpen(basevalue);
        }

        private object OnCoerceIsDropDownOpen(object basevalue)
        {
            if ((bool)basevalue)
            {
                if (!IsLoaded)
                {
                    return false;
                }
            }
            return basevalue;
        }


        private void IsDropDownOpenChanged(DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("IsDropDownOpenChanged: " + e.NewValue);
            var newValue = (bool)e.NewValue;
            if (newValue)
            {
                Mouse.Capture(this, CaptureMode.SubTree);
                staticColorList.Focus();
            }
            else
            {
                if (Mouse.Captured == this)
                {
                    Mouse.Capture(null);
                }
            }


            // Reload last used palette each time the dropdown is opened.
            if (IsDropDownOpen)
            {
                LoadLastPalette();
            }

            // turn off picking when drop down is closed
            if (!IsDropDownOpen && IsPicking)
                IsPicking = false;
        }

        private static void IsPickingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).IsPickingChanged();
        }

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
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                try
                {
                    Point pt = CaptureScreenshot.GetMouseScreenPosition();
                    BitmapSource bmp = CaptureScreenshot.Capture(new Rect(pt, new Size(1, 1)));
                    var pixels = new byte[4];
                    bmp.CopyPixels(pixels, 4, 0);
                    SelectedColor = Color.FromArgb(0xFF, pixels[2], pixels[1], pixels[0]);
                }
                catch (Exception)
                {
                }
            }
        }

        private static void SelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).OnSelectedValueChanged();
        }

        private static void SelectedPersistentColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // No action here
        }

        private void OnSelectedValueChanged()
        {
            // don't update the HSV controls if the original change was H, S or V.
            if (updateHSV)
            {
                byte[] hsv = ColorHelper.ColorToHsvBytes(SelectedColor);
                hue = hsv[0];
                saturation = hsv[1];
                brightness = hsv[2];
                OnPropertyChanged("Hue");
                OnPropertyChanged("Saturation");
                OnPropertyChanged("Brightness");
            }
            if (updateHexValue)
            {
                OnPropertyChanged("HexValue");
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

        public static ObservableCollection<Color> CreateDefaultPalette()
        {
            var palette = new ObservableCollection<Color>();

            // transparent colors
            palette.Add(Colors.Transparent);
            palette.Add(Color.FromArgb(128, 0, 0, 0));
            palette.Add(Color.FromArgb(128, 255, 255, 255));

            // shades of gray
            palette.Add(Colors.White);
            palette.Add(Colors.Silver);
            palette.Add(Colors.Gray);
            palette.Add(Colors.DarkSlateGray);
            palette.Add(Colors.Black);

            // standard colors
            palette.Add(Colors.Firebrick);
            palette.Add(Colors.Red);
            palette.Add(Colors.Tomato);
            palette.Add(Colors.OrangeRed);
            palette.Add(Colors.Orange);
            palette.Add(Colors.Gold);
            palette.Add(Colors.Yellow);
            palette.Add(Colors.YellowGreen);
            palette.Add(Colors.SeaGreen);
            palette.Add(Colors.DeepSkyBlue);
            palette.Add(Colors.CornflowerBlue);
            palette.Add(Colors.LightBlue);
            palette.Add(Colors.DarkCyan);
            palette.Add(Colors.MidnightBlue);
            palette.Add(Colors.DarkOrchid);

            // Add colors by hue
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