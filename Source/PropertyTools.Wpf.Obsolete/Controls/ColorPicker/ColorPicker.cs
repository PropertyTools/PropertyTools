// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPicker.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Represents a control that lets the user pick a color.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
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

    /// <summary>
    /// Represents a control that lets the user pick a color.
    /// </summary>
    public partial class ColorPicker : Control, INotifyPropertyChanged
    {
        /// <summary>
        /// The is drop down open property.
        /// </summary>
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
            "IsDropDownOpen",
            typeof(bool),
            typeof(ColorPicker),
            new UIPropertyMetadata(false, IsDropDownOpenChanged, CoerceIsDropDownOpen));

        /// <summary>
        /// The is picking property.
        /// </summary>
        public static readonly DependencyProperty IsPickingProperty = DependencyProperty.Register(
            "IsPicking", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(false, IsPickingChanged));

        /// <summary>
        /// The palette property.
        /// </summary>
        public static readonly DependencyProperty PaletteProperty = DependencyProperty.Register(
            "Palette",
            typeof(ObservableCollection<Color>),
            typeof(ColorPicker),
            new UIPropertyMetadata(CreateDefaultPalette()));

        /// <summary>
        /// The selected color property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
            "SelectedColor",
            typeof(Color),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(
                Color.FromArgb(0, 0, 0, 0), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedColorChanged));

        /// <summary>
        /// The show as hex property.
        /// </summary>
        public static readonly DependencyProperty ShowAsHexProperty = DependencyProperty.Register(
            "ShowAsHex", typeof(bool), typeof(ColorPicker), new UIPropertyMetadata(false));

        /// <summary>
        /// The tab strip placement property.
        /// </summary>
        public static readonly DependencyProperty TabStripPlacementProperty =
            DependencyProperty.Register(
                "TabStripPlacement", typeof(Dock), typeof(ColorPicker), new UIPropertyMetadata(Dock.Bottom));

        /// <summary>
        /// The brightness.
        /// </summary>
        private byte brightness;

        /// <summary>
        /// The hue.
        /// </summary>
        private byte hue;

        /// <summary>
        /// The picking timer.
        /// </summary>
        private DispatcherTimer pickingTimer;

        /// <summary>
        /// The saturation.
        /// </summary>
        private byte saturation;

        /// <summary>
        /// The static color list.
        /// </summary>
        private ListBox staticColorList;

        /// <summary>
        /// The update hsv.
        /// </summary>
        private bool updateHSV = true;

        /// <summary>
        /// The update hex value.
        /// </summary>
        private bool updateHexValue = true;

        /// <summary>
        /// Initializes static members of the <see cref = "ColorPicker" /> class.
        /// </summary>
        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
            EventManager.RegisterClassHandler(
                typeof(ColorPicker), Mouse.MouseDownEvent, new MouseButtonEventHandler(OnMouseButtonDown), true);
            EventManager.RegisterClassHandler(typeof(ColorPicker), GotFocusEvent, new RoutedEventHandler(OnGotFocus));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref = "ColorPicker" /> class.
        /// </summary>
        public ColorPicker()
        {
            this.Loaded += this.ColorPickerLoaded;

            // this.mouseEvent = this.PaletteList_MouseUp;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the alpha.
        /// </summary>
        /// <value>The alpha.</value>
        public byte Alpha
        {
            get
            {
                return this.SelectedColor.A;
            }

            set
            {
                this.SelectedColor = Color.FromArgb(value, this.Red, this.Green, this.Blue);
            }
        }

        /// <summary>
        /// Gets the alpha gradient.
        /// </summary>
        /// <value>The alpha gradient.</value>
        public Brush AlphaGradient
        {
            get
            {
                return new LinearGradientBrush(Colors.Transparent, Color.FromRgb(this.Red, this.Green, this.Blue), 0);
            }
        }

        /// <summary>
        /// Gets or sets the blue.
        /// </summary>
        /// <value>The blue.</value>
        public byte Blue
        {
            get
            {
                return this.SelectedColor.B;
            }

            set
            {
                this.SelectedColor = Color.FromArgb(this.Alpha, this.Red, this.Green, value);
            }
        }

        /// <summary>
        /// Gets or sets the brightness.
        /// </summary>
        /// <value>The brightness.</value>
        public byte Brightness
        {
            get
            {
                return this.brightness;
            }

            set
            {
                this.updateHSV = false;
                this.SelectedColor = ColorHelper.HsvToColor(this.Hue, this.Saturation, value);
                this.updateHSV = true;
                this.brightness = value;
                this.OnPropertyChanged("Brightness");
            }
        }

        /// <summary>
        /// Gets the brightness gradient.
        /// </summary>
        /// <value>The brightness gradient.</value>
        public Brush BrightnessGradient
        {
            get
            {
                return new LinearGradientBrush(
                    ColorHelper.HsvToColor(this.Hue, this.Saturation, 0),
                    ColorHelper.HsvToColor(this.Hue, this.Saturation, 255),
                    0);
            }
        }

        /// <summary>
        /// Gets the name of the color.
        /// </summary>
        /// <value>The name of the color.</value>
        public string ColorName
        {
            get
            {
                if (this.ShowAsHex)
                {
                    return this.SelectedColor.ColorToHex();
                }

                Type t = typeof(Colors);
                PropertyInfo[] fields = t.GetProperties(BindingFlags.Public | BindingFlags.Static);
                string nearestColor = "Custom";
                double nearestDist = 30;

                // find the color that is closest
                foreach (var fi in fields)
                {
                    var c = (Color)fi.GetValue(null, null);
                    if (this.SelectedColor == c)
                    {
                        return fi.Name;
                    }

                    double d = ColorHelper.ColorDifference(this.SelectedColor, c);
                    if (d < nearestDist)
                    {
                        nearestColor = "~ " + fi.Name; // 'kind of'
                        nearestDist = d;
                    }
                }

                if (this.SelectedColor.A < 255)
                {
                    return string.Format("{0}, {1:0} %", nearestColor, this.SelectedColor.A / 2.55);
                }

                return nearestColor;
            }
        }

        /// <summary>
        /// Gets or sets the green.
        /// </summary>
        /// <value>The green.</value>
        public byte Green
        {
            get
            {
                return this.SelectedColor.G;
            }

            set
            {
                this.SelectedColor = Color.FromArgb(this.Alpha, this.Red, value, this.Blue);
            }
        }

        /// <summary>
        /// Gets or sets the hex value.
        /// </summary>
        /// <value>The hex value.</value>
        public string HexValue
        {
            get
            {
                return this.SelectedColor.ColorToHex();
            }

            set
            {
                this.updateHexValue = false;
                this.SelectedColor = ColorHelper.HexToColor(value);
                this.updateHexValue = true;
            }
        }

        /// <summary>
        /// Gets or sets the hue.
        /// </summary>
        /// <value>The hue.</value>
        public byte Hue
        {
            get
            {
                return this.hue;
            }

            set
            {
                this.updateHSV = false;
                this.SelectedColor = ColorHelper.HsvToColor(value, this.Saturation, this.Brightness);
                this.hue = value;
                this.OnPropertyChanged("Hue");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the color picker popup is open.
        /// </summary>
        public bool IsDropDownOpen
        {
            get
            {
                return (bool)this.GetValue(IsDropDownOpenProperty);
            }

            set
            {
                this.SetValue(IsDropDownOpenProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets if picking colors from the screen is active.
        /// </summary>
        /// <remarks>
        /// Use the 'SHIFT' button to select colors when this mode is active.
        /// </remarks>
        public bool IsPicking
        {
            get
            {
                return (bool)this.GetValue(IsPickingProperty);
            }

            set
            {
                this.SetValue(IsPickingProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the palette.
        /// </summary>
        /// <value>The palette.</value>
        public ObservableCollection<Color> Palette
        {
            get
            {
                return (ObservableCollection<Color>)this.GetValue(PaletteProperty);
            }

            set
            {
                this.SetValue(PaletteProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the red.
        /// </summary>
        /// <value>The red.</value>
        public byte Red
        {
            get
            {
                return this.SelectedColor.R;
            }

            set
            {
                this.SelectedColor = Color.FromArgb(this.Alpha, value, this.Green, this.Blue);
            }
        }

        /// <summary>
        /// Gets or sets the saturation.
        /// </summary>
        /// <value>The saturation.</value>
        public byte Saturation
        {
            get
            {
                return this.saturation;
            }

            set
            {
                this.updateHSV = false;
                this.SelectedColor = ColorHelper.HsvToColor(this.Hue, value, this.Brightness);
                this.updateHSV = true;
                this.saturation = value;
                this.OnPropertyChanged("Saturation");
            }
        }

        /// <summary>
        /// Gets the saturation gradient.
        /// </summary>
        /// <value>The saturation gradient.</value>
        public Brush SaturationGradient
        {
            get
            {
                return new LinearGradientBrush(
                    ColorHelper.HsvToColor(this.Hue, 0, this.Brightness),
                    ColorHelper.HsvToColor(this.Hue, 255, this.Brightness),
                    0);
            }
        }

        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        /// <value>The color of the selected.</value>
        public Color SelectedColor
        {
            get
            {
                return (Color)this.GetValue(SelectedColorProperty);
            }

            set
            {
                this.SetValue(SelectedColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show as color names as hex strings.
        /// </summary>
        /// <value><c>true</c> if show as hex; otherwise, <c>false</c>.</value>
        public bool ShowAsHex
        {
            get
            {
                return (bool)this.GetValue(ShowAsHexProperty);
            }

            set
            {
                this.SetValue(ShowAsHexProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the tab strip placement.
        /// </summary>
        /// <value>The tab strip placement.</value>
        public Dock TabStripPlacement
        {
            get
            {
                return (Dock)this.GetValue(TabStripPlacementProperty);
            }

            set
            {
                this.SetValue(TabStripPlacementProperty, value);
            }
        }

        /// <summary>
        /// Creates the default palette.
        /// </summary>
        /// <returns>
        /// </returns>
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

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see cref="M:System.Windows.FrameworkElement.ApplyTemplate"/>.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.staticColorList = this.Template.FindName("PART_StaticColorList", this) as ListBox;
        }

        /// <summary>
        /// Invoked when an unhandled <see cref="E:System.Windows.Input.Keyboard.KeyDown"/> attached event reaches an element in its route that is derived from this class. Implement this method to add class handling for this event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="T:System.Windows.Input.KeyEventArgs"/> that contains the event data.
        /// </param>
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
                    if (this.IsDropDownOpen)
                    {
                        this.CloseDropDown();
                        handled = true;
                    }

                    break;
                case Key.Escape:
                    if (this.IsDropDownOpen)
                    {
                        this.CloseDropDown();
                        handled = true;
                    }

                    break;
            }

            e.Handled = handled;
        }

        /// <summary>
        /// Called when a property changed.
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// The coerce is drop down open.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="basevalue">
        /// The basevalue.
        /// </param>
        /// <returns>
        /// The coerce is drop down open.
        /// </returns>
        private static object CoerceIsDropDownOpen(DependencyObject d, object basevalue)
        {
            return ((ColorPicker)d).OnCoerceIsDropDownOpen(basevalue);
        }

        /// <summary>
        /// The is drop down open changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void IsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).IsDropDownOpenChanged(e);
        }

        /// <summary>
        /// The is picking changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void IsPickingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).IsPickingChanged();
        }

        /// <summary>
        /// Called when the control got focus.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
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

        /// <summary>
        /// Called when [mouse button down].
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.
        /// </param>
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

        /// <summary>
        /// The selected color changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void SelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPicker)d).OnSelectedValueChanged();
        }

        /// <summary>
        /// The selected persistent color changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private static void SelectedPersistentColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // No action here
        }

        /// <summary>
        /// Closes the drop down.
        /// </summary>
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

        /// <summary>
        /// Called when the control is loaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void ColorPickerLoaded(object sender, RoutedEventArgs e)
        {
            // this.InitializePaletteSettings();
            // this.LoadLastPalette();
        }

        /// <summary>
        /// The is drop down open changed.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        private void IsDropDownOpenChanged(DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("IsDropDownOpenChanged: " + e.NewValue);
            var newValue = (bool)e.NewValue;
            if (newValue)
            {
                Mouse.Capture(this, CaptureMode.SubTree);
                this.staticColorList.Focus();
            }
            else
            {
                if (Mouse.Captured == this)
                {
                    Mouse.Capture(null);
                }
            }

            // Reload last used palette each time the dropdown is opened.
            if (this.IsDropDownOpen)
            {
                // this.LoadLastPalette();
            }

            // turn off picking when drop down is closed
            if (!this.IsDropDownOpen && this.IsPicking)
            {
                this.IsPicking = false;
            }
        }

        /// <summary>
        /// Called when IsPicking is changed.
        /// </summary>
        private void IsPickingChanged()
        {
            if (this.IsPicking && this.pickingTimer == null)
            {
                this.pickingTimer = new DispatcherTimer();
                this.pickingTimer.Interval = TimeSpan.FromMilliseconds(100);
                this.pickingTimer.Tick += this.Pick;
                this.pickingTimer.Start();
            }

            if (!this.IsPicking && this.pickingTimer != null)
            {
                this.pickingTimer.Tick -= this.Pick;
                this.pickingTimer.Stop();
                this.pickingTimer = null;
            }
        }

        /// <summary>
        /// The on coerce is drop down open.
        /// </summary>
        /// <param name="basevalue">
        /// The basevalue.
        /// </param>
        /// <returns>
        /// The on coerce is drop down open.
        /// </returns>
        private object OnCoerceIsDropDownOpen(object basevalue)
        {
            if ((bool)basevalue)
            {
                if (!this.IsLoaded)
                {
                    return false;
                }
            }

            return basevalue;
        }

        /// <summary>
        /// Called when the selected value changed.
        /// </summary>
        private void OnSelectedValueChanged()
        {
            // don't update the HSV controls if the original change was H, S or V.
            if (this.updateHSV)
            {
                byte[] hsv = this.SelectedColor.ColorToHsvBytes();
                this.hue = hsv[0];
                this.saturation = hsv[1];
                this.brightness = hsv[2];
                this.OnPropertyChanged("Hue");
                this.OnPropertyChanged("Saturation");
                this.OnPropertyChanged("Brightness");
            }

            if (this.updateHexValue)
            {
                this.OnPropertyChanged("HexValue");
            }

            this.OnPropertyChanged("Red");
            this.OnPropertyChanged("Green");
            this.OnPropertyChanged("Blue");
            this.OnPropertyChanged("Alpha");
            this.OnPropertyChanged("ColorName");
            this.OnPropertyChanged("AlphaGradient");
            this.OnPropertyChanged("SaturationGradient");
            this.OnPropertyChanged("BrightnessGradient");
        }

        /// <summary>
        /// Picks a color from the screen.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
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
                    this.SelectedColor = Color.FromArgb(0xFF, pixels[2], pixels[1], pixels[0]);
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Toggles the drop down.
        /// </summary>
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

    }
}