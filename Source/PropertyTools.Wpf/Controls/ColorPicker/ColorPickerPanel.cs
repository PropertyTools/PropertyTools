// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPickerPanel.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
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
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;

    /// <summary>
    /// Represents a control that lets the user pick a color.
    /// </summary>
    [TemplatePart(Name = PartHsv, Type = typeof(HsvControl))]
    [TemplatePart(Name = PartPredefinedColorPanel, Type = typeof(StackPanel))]
    public class ColorPickerPanel : Control, INotifyPropertyChanged
    {
        /// <summary>
        /// The alpha property.
        /// </summary>
        public static readonly DependencyProperty AlphaProperty = DependencyProperty.Register(
            "Alpha",
            typeof(int),
            typeof(ColorPickerPanel),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ComponentChanged));

        /// <summary>
        /// The blue property.
        /// </summary>
        public static readonly DependencyProperty BlueProperty = DependencyProperty.Register(
            "Blue",
            typeof(int),
            typeof(ColorPickerPanel),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ComponentChanged));

        /// <summary>
        /// The brightness property.
        /// </summary>
        public static readonly DependencyProperty BrightnessProperty = DependencyProperty.Register(
            "Brightness",
            typeof(int),
            typeof(ColorPickerPanel),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ComponentChanged));

        /// <summary>
        /// The green property.
        /// </summary>
        public static readonly DependencyProperty GreenProperty = DependencyProperty.Register(
            "Green",
            typeof(int),
            typeof(ColorPickerPanel),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ComponentChanged));

        /// <summary>
        /// The hue property.
        /// </summary>
        public static readonly DependencyProperty HueProperty = DependencyProperty.Register(
            "Hue",
            typeof(int),
            typeof(ColorPickerPanel),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ComponentChanged));

        /// <summary>
        /// The is picking property.
        /// </summary>
        public static readonly DependencyProperty IsPickingProperty = DependencyProperty.Register(
            "IsPicking", typeof(bool), typeof(ColorPickerPanel), new UIPropertyMetadata(false, IsPickingChanged));

        /// <summary>
        /// The red property.
        /// </summary>
        public static readonly DependencyProperty RedProperty = DependencyProperty.Register(
            "Red",
            typeof(int),
            typeof(ColorPickerPanel),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ComponentChanged));

        /// <summary>
        /// The saturation property.
        /// </summary>
        public static readonly DependencyProperty SaturationProperty = DependencyProperty.Register(
            "Saturation",
            typeof(int),
            typeof(ColorPickerPanel),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ComponentChanged));

        /// <summary>
        /// The selected color property.
        /// </summary>
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
            "SelectedColor",
            typeof(Color?),
            typeof(ColorPickerPanel),
            new FrameworkPropertyMetadata(
                Color.FromArgb(0, 0, 0, 0),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                SelectedColorChanged,
                CoerceSelectedColorValue));

        /// <summary>
        /// The HSV control part name.
        /// </summary>
        private const string PartHsv = "PART_HSV";

        /// <summary>
        /// The predefined color panel part name
        /// </summary>
        private const string PartPredefinedColorPanel = "PART_PredefinedColorPanel";

        /// <summary>
        /// The max number of recent colors.
        /// </summary>
        private static int maxNumberOfRecentColors = 20;

        /// <summary>
        /// The show hsv panel.
        /// </summary>
        private static bool showHsvPanel;

        /// <summary>
        /// The HSV control.
        /// </summary>
        private HsvControl hsvControl;

        /// <summary>
        /// The picking timer.
        /// </summary>
        private DispatcherTimer pickingTimer;

        /// <summary>
        /// The within color change.
        /// </summary>
        private bool withinColorChange;

        /// <summary>
        /// The within component change.
        /// </summary>
        private bool withinComponentChange;

        /// <summary>
        /// Initializes static members of the <see cref="ColorPickerPanel" /> class.
        /// </summary>
        static ColorPickerPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ColorPickerPanel), new FrameworkPropertyMetadata(typeof(ColorPickerPanel)));
            InitPalette();
            Strings = new ColorPickerPanelStrings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPickerPanel" /> class.
        /// </summary>
        public ColorPickerPanel()
        {
            this.Unloaded += this.PanelUnloaded;
            this.OpacityVariations = new ObservableCollection<Color>();
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The predefined colors selection changed event
        /// </summary>
        public event SelectionChangedEventHandler PredefinedColorPanelSelectionChangedEvent;

        /// <summary>
        /// Gets the recent colors.
        /// </summary>
        /// <value> The recent colors. </value>
        public static ObservableCollection<Color> RecentColors { get; private set; }

        /// <summary>
        /// Gets the standard colors.
        /// </summary>
        /// <value> The standard colors. </value>
        public static ObservableCollection<Color> StandardColors { get; private set; }

        /// <summary>
        /// Gets or sets the localized strings.
        /// </summary>
        public static ColorPickerPanelStrings Strings { get; set; }

        /// <summary>
        /// Gets the theme colors.
        /// </summary>
        /// <value> The theme colors. </value>
        public static ObservableCollection<Color> ThemeColors { get; private set; }

        /// <summary>
        /// Gets the opacity colors.
        /// </summary>
        /// <value> The opacity colors. </value>
        public ObservableCollection<Color> OpacityVariations { get; private set; }

        /// <summary>
        /// Gets or sets the alpha value.
        /// </summary>
        /// <value> The alpha. </value>
        public int Alpha
        {
            get
            {
                return (int)this.GetValue(AlphaProperty);
            }

            set
            {
                this.SetValue(AlphaProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the blue.
        /// </summary>
        /// <value> The blue. </value>
        public int Blue
        {
            get
            {
                return (int)this.GetValue(BlueProperty);
            }

            set
            {
                this.SetValue(BlueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the brightness.
        /// </summary>
        /// <value> The brightness. </value>
        public int Brightness
        {
            get
            {
                return (int)this.GetValue(BrightnessProperty);
            }

            set
            {
                this.SetValue(BrightnessProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the green.
        /// </summary>
        /// <value> The green. </value>
        public int Green
        {
            get
            {
                return (int)this.GetValue(GreenProperty);
            }

            set
            {
                this.SetValue(GreenProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the hue.
        /// </summary>
        /// <value> The hue. </value>
        public int Hue
        {
            get
            {
                return (int)this.GetValue(HueProperty);
            }

            set
            {
                this.SetValue(HueProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this user is picking colors on the screen.
        /// </summary>
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
        /// Gets or sets the max number of recent colors.
        /// </summary>
        /// <value> The max number of recent colors. </value>
        public int MaxNumberOfRecentColors
        {
            get
            {
                return maxNumberOfRecentColors;
            }

            set
            {
                maxNumberOfRecentColors = value;
            }
        }

        /// <summary>
        /// Gets or sets the red value.
        /// </summary>
        /// <value> The red. </value>
        public int Red
        {
            get
            {
                return (int)this.GetValue(RedProperty);
            }

            set
            {
                this.SetValue(RedProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the saturation.
        /// </summary>
        /// <value> The saturation. </value>
        public int Saturation
        {
            get
            {
                return (int)this.GetValue(SaturationProperty);
            }

            set
            {
                this.SetValue(SaturationProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected color.
        /// </summary>
        /// <value> The color of the selected. </value>
        public Color? SelectedColor
        {
            get
            {
                return (Color?)this.GetValue(SelectedColorProperty);
            }

            set
            {
                this.SetValue(SelectedColorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the HSV panel.
        /// </summary>
        /// <remarks>
        /// The backing field is static.
        /// </remarks>
        public bool ShowHsvPanel
        {
            get
            {
                return showHsvPanel;
            }

            set
            {
                showHsvPanel = value;
                this.RaisePropertyChanged("ShowHsvPanel");
            }
        }

        /// <summary>
        /// Determines whether the user is color picking.
        /// </summary>
        /// <returns><c>true</c> if the user is picking a color; otherwise, <c>false</c>.</returns>
        public bool IsPickingColor()
        {
            return this.IsPicking && this.IsPickKeyDown();
        }

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes call <see
        /// cref="M:System.Windows.FrameworkElement.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.hsvControl = this.GetTemplateChild(PartHsv) as HsvControl;
            var predefinedColorPanel = this.GetTemplateChild(PartPredefinedColorPanel) as StackPanel;
            if (predefinedColorPanel != null)
            {
                predefinedColorPanel.AddHandler(Selector.SelectionChangedEvent, (SelectionChangedEventHandler)this.OnPredefinedColorPanelSelectionChanged, true);
            }
        }

        /// <summary>
        /// Coerces the selected color value.
        /// </summary>
        /// <param name="baseValue">
        /// The base value.
        /// </param>
        /// <returns>
        /// The coerced selected color value.
        /// </returns>
        protected virtual object CoerceSelectedColorValue(object baseValue)
        {
            if (baseValue == null)
            {
                return this.SelectedColor;
            }

            return baseValue;
        }

        /// <summary>
        /// Called when a color component is changed.
        /// </summary>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        protected virtual void OnComponentChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.withinColorChange)
            {
                return;
            }

            if (this.SelectedColor == null)
            {
                return;
            }

            var color = this.SelectedColor.Value;
            this.withinComponentChange = true;
            this.withinColorChange = true;
            var i = Convert.ToInt32(e.NewValue);
            byte x = i <= 255 ? (byte)i : (byte)255;
            if (e.Property == AlphaProperty)
            {
                this.SelectedColor = Color.FromArgb(x, color.R, color.G, color.B);
            }

            if (e.Property == RedProperty)
            {
                this.SelectedColor = Color.FromArgb(color.A, x, color.G, color.B);
                this.UpdateHSV(color);
            }

            if (e.Property == GreenProperty)
            {
                this.SelectedColor = Color.FromArgb(color.A, color.R, x, color.B);
                this.UpdateHSV(color);
            }

            if (e.Property == BlueProperty)
            {
                this.SelectedColor = Color.FromArgb(color.A, color.R, color.G, x);
                this.UpdateHSV(color);
            }

            var hsv = color.ColorToHsv();
            double y = Convert.ToDouble(e.NewValue);
            if (e.Property == HueProperty)
            {
                this.SelectedColor = ColorHelper.HsvToColor(y / 360, hsv[1], hsv[2], color.A / 255.0);
                this.UpdateRGB(this.SelectedColor.Value);
            }

            if (e.Property == SaturationProperty)
            {
                this.SelectedColor = ColorHelper.HsvToColor(hsv[0], y / 100, hsv[2], color.A / 255.0);
                this.UpdateRGB(this.SelectedColor.Value);
            }

            if (e.Property == BrightnessProperty)
            {
                this.SelectedColor = ColorHelper.HsvToColor(hsv[0], hsv[1], y / 100, color.A / 255.0);
                this.UpdateRGB(this.SelectedColor.Value);
            }

            this.withinColorChange = false;
            this.withinComponentChange = false;
        }

        /// <summary>
        /// Called when the IsPicking property is changed.
        /// </summary>
        protected virtual void OnIsPickingChanged()
        {
            if (this.IsPicking && this.pickingTimer == null)
            {
                this.pickingTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
                this.pickingTimer.Tick += this.Pick;
                this.pickingTimer.Start();

                // Mouse.SetCursor(PickerCursor);
            }

            if (!this.IsPicking && this.pickingTimer != null)
            {
                // Mouse.SetCursor(Cursors.Arrow);
                this.pickingTimer.Tick -= this.Pick;
                this.pickingTimer.Stop();
                this.pickingTimer = null;
            }
        }

        /// <summary>
        /// Called when the selected color changed.
        /// </summary>
        /// <param name="newColor">
        /// The new color.
        /// </param>
        /// <param name="oldColor">
        /// The old color.
        /// </param>
        protected virtual void OnSelectedColorChanged(Color? newColor, Color? oldColor)
        {
            if (!this.withinColorChange && !this.withinComponentChange && newColor != null)
            {
                this.UpdateRGB(newColor.Value);
                this.UpdateHSV(newColor.Value);
                this.UpdateOpacityVariations(newColor.Value);
            }
        }

        /// <summary>
        /// The raise property changed.
        /// </summary>
        /// <param name="property">
        /// The property.
        /// </param>
        protected void RaisePropertyChanged(string property)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        /// <summary>
        /// Coerces the selected color value.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="basevalue">
        /// The base value.
        /// </param>
        /// <returns>
        /// The coerce selected color value.
        /// </returns>
        private static object CoerceSelectedColorValue(DependencyObject d, object basevalue)
        {
            return ((ColorPickerPanel)d).CoerceSelectedColorValue(basevalue);
        }

        /// <summary>
        /// Called when a color component is changed.
        /// </summary>
        /// <param name="d">
        /// The d.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.
        /// </param>
        private static void ComponentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ColorPickerPanel)d).OnComponentChanged(e);
        }

        /// <summary>
        /// Initializes the palettes.
        /// </summary>
        private static void InitPalette()
        {
            ThemeColors = new ObservableCollection<Color>
                {
                    Colors.White,
                    Colors.Black,
                    ColorHelper.UIntToColor(0xFFeeece1),
                    ColorHelper.UIntToColor(0xFF1f497d),
                    ColorHelper.UIntToColor(0xFF4f81bd),
                    ColorHelper.UIntToColor(0xFFc0504d),
                    ColorHelper.UIntToColor(0xFF9bbb59),
                    ColorHelper.UIntToColor(0xFF8064a2),
                    ColorHelper.UIntToColor(0xFF4bacc6),
                    ColorHelper.UIntToColor(0xFFf79646),
                    ColorHelper.UIntToColor(0xFFf2f2f2),
                    ColorHelper.UIntToColor(0xFF7f7f7f),
                    ColorHelper.UIntToColor(0xFFddd9c3),
                    ColorHelper.UIntToColor(0xFFc6d9f0),
                    ColorHelper.UIntToColor(0xFFdbe5f1),
                    ColorHelper.UIntToColor(0xFFf2dcdb),
                    ColorHelper.UIntToColor(0xFFebf1dd),
                    ColorHelper.UIntToColor(0xFFe5e0ec),
                    ColorHelper.UIntToColor(0xFFdbeef3),
                    ColorHelper.UIntToColor(0xFFfdeada),
                    ColorHelper.UIntToColor(0xFFd8d8d8),
                    ColorHelper.UIntToColor(0xFF595959),
                    ColorHelper.UIntToColor(0xFFc4bd97),
                    ColorHelper.UIntToColor(0xFF8db3e2),
                    ColorHelper.UIntToColor(0xFFb8cce4),
                    ColorHelper.UIntToColor(0xFFe5b9b7),
                    ColorHelper.UIntToColor(0xFFd7e3bc),
                    ColorHelper.UIntToColor(0xFFccc1d9),
                    ColorHelper.UIntToColor(0xFFb7dde8),
                    ColorHelper.UIntToColor(0xFFfbd5b5),
                    ColorHelper.UIntToColor(0xFFbfbfbf),
                    ColorHelper.UIntToColor(0xFF3f3f3f),
                    ColorHelper.UIntToColor(0xFF938953),
                    ColorHelper.UIntToColor(0xFF548dd4),
                    ColorHelper.UIntToColor(0xFF95b3d7),
                    ColorHelper.UIntToColor(0xFFd99694),
                    ColorHelper.UIntToColor(0xFFc3d69b),
                    ColorHelper.UIntToColor(0xFFb2a2c7),
                    ColorHelper.UIntToColor(0xFF92cddc),
                    ColorHelper.UIntToColor(0xFFfac08f),
                    ColorHelper.UIntToColor(0xFFa5a5a5),
                    ColorHelper.UIntToColor(0xFF262626),
                    ColorHelper.UIntToColor(0xFF494429),
                    ColorHelper.UIntToColor(0xFF17365d),
                    ColorHelper.UIntToColor(0xFF366092),
                    ColorHelper.UIntToColor(0xFF953734),
                    ColorHelper.UIntToColor(0xFF76923c),
                    ColorHelper.UIntToColor(0xFF5f497a),
                    ColorHelper.UIntToColor(0xFF31859b),
                    ColorHelper.UIntToColor(0xFFe36c09),
                    ColorHelper.UIntToColor(0xFF6f7f7f),
                    ColorHelper.UIntToColor(0xFF0c0c0c),
                    ColorHelper.UIntToColor(0xFF1d1b10),
                    ColorHelper.UIntToColor(0xFF0f243e),
                    ColorHelper.UIntToColor(0xFF244061),
                    ColorHelper.UIntToColor(0xFF632423),
                    ColorHelper.UIntToColor(0xFF4f6128),
                    ColorHelper.UIntToColor(0xFF3f3151),
                    ColorHelper.UIntToColor(0xFF205867),
                    ColorHelper.UIntToColor(0xFF974806)
                };

            StandardColors = new ObservableCollection<Color>
                {
                    Colors.Firebrick,
                    Colors.Red,
                    Colors.Tomato,
                    Colors.OrangeRed,
                    Colors.Orange,
                    Colors.Gold,
                    Colors.Yellow,
                    Colors.YellowGreen,
                    Colors.SeaGreen,
                    Colors.DeepSkyBlue,
                    Colors.CornflowerBlue,
                    Colors.LightBlue,
                    Colors.DarkCyan,
                    Colors.MidnightBlue,
                    Colors.DarkOrchid,
                    Colors.Transparent,
                    Color.FromArgb(128, 0, 0, 0),
                    Color.FromArgb(128, 255, 255, 255),
                    ColorHelper.UndefinedColor,
                    ColorHelper.Automatic
                };

            RecentColors = new ObservableCollection<Color>();
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
            ((ColorPickerPanel)d).OnIsPickingChanged();
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
            ((ColorPickerPanel)d).OnSelectedColorChanged((Color?)e.NewValue, (Color?)e.OldValue);
        }

        /// <summary>
        /// The add color to recent colors if missing.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        private void AddColorToRecentColorsIfMissing(Color color)
        {
            // Check if the color exists
            if (RecentColors.Contains(color))
            {
                var index = RecentColors.IndexOf(color);
                RecentColors.Move(index, 0);
                return;
            }

            if (RecentColors.Count >= this.MaxNumberOfRecentColors)
            {
                RecentColors.RemoveAt(RecentColors.Count - 1);
            }

            RecentColors.Insert(0, color);
        }

        /// <summary>
        /// Handles the <see cref="E:PredefinedColorPanelSelectionChangedEvent" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnPredefinedColorPanelSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            var listBox = args.OriginalSource as ListBox;

            if (listBox != null && args.AddedItems.Count != 0)
            {
                this.SelectedColor = (Color)args.AddedItems[0];
                listBox.UnselectAll();
            }

            if (this.PredefinedColorPanelSelectionChangedEvent != null)
            {
                this.PredefinedColorPanelSelectionChangedEvent(sender, args);
            }
        }

        /// <summary>
        /// Called when the panel is unloaded.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void PanelUnloaded(object sender, RoutedEventArgs e)
        {
            if (this.IsPicking)
            {
                this.IsPicking = false;
            }

            if (this.SelectedColor != null)
            {
                this.AddColorToRecentColorsIfMissing(this.SelectedColor.Value);
            }
        }

        /// <summary>
        /// Picks a color from the current mouse position.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.EventArgs"/> instance containing the event data.
        /// </param>
        private void Pick(object sender, EventArgs e)
        {
            if (this.IsPickKeyDown())
            {
                try
                {
                    Point pt = CaptureScreenshot.GetMouseScreenPosition();
                    BitmapSource bmp = CaptureScreenshot.Capture(new Rect(pt, new Size(1, 1)));
                    var pixels = new byte[4];
                    bmp.CopyPixels(pixels, 4, 0);
                    this.SelectedColor = Color.FromArgb(0xFF, pixels[2], pixels[1], pixels[0]);
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch
                {
                }
            }
        }

        /// <summary>
        /// Determines whether the color picking key is pressed.
        /// </summary>
        /// <returns><c>true</c> if the key is down; otherwise, <c>false</c>.</returns>
        private bool IsPickKeyDown()
        {
            return Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
        }

        /// <summary>
        /// Updates the opacity variation collection.
        /// </summary>
        /// <param name="color">The currently selected color.</param>
        private void UpdateOpacityVariations(Color color)
        {
            this.OpacityVariations.Clear();
            for (int i = 1; i <= 9; i++)
            {
                this.OpacityVariations.Add(Color.FromArgb((byte)(255 * (i * 0.1)), color.R, color.G, color.B));
            }
        }

        /// <summary>
        /// Updates the hue, saturation and brightness properties.
        /// </summary>
        /// <param name="color">
        /// The currently selected color.
        /// </param>
        // ReSharper disable once InconsistentNaming
        private void UpdateHSV(Color color)
        {
            this.withinColorChange = true;
            if (this.hsvControl != null)
            {
                this.hsvControl.withinUpdate = true;
            }

            var hsv = color.ColorToHsv();
            this.Hue = (int)(hsv[0] * 360);
            this.Saturation = (int)(hsv[1] * 100);
            this.Brightness = (int)(hsv[2] * 100);
            this.withinColorChange = false;
            if (this.hsvControl != null)
            {
                this.hsvControl.withinUpdate = false;
            }
        }

        /// <summary>
        /// Updates the red, green, blue and alpha properties.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        // ReSharper disable once InconsistentNaming
        private void UpdateRGB(Color color)
        {
            this.withinColorChange = true;
            if (this.hsvControl != null)
            {
                this.hsvControl.withinUpdate = true;
            }

            this.Alpha = color.A;
            this.Red = color.R;
            this.Green = color.G;
            this.Blue = color.B;
            this.withinColorChange = false;
            if (this.hsvControl != null)
            {
                this.hsvControl.withinUpdate = false;
            }
        }
    }
}