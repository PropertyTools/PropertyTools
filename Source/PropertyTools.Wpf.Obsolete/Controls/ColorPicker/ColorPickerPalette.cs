// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPickerPalette.cs" company="PropertyTools">
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
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using PropertyTools.Wpf.Controls.ColorPicker;

    using Button = System.Windows.Controls.Button;
    using ListBox = System.Windows.Controls.ListBox;
    using MessageBox = System.Windows.MessageBox;
    using Path = System.IO.Path;

    /// <summary>
    /// Represents a control that lets the user pick a color.
    /// </summary>
    public partial class ColorPicker
    {
        /// <summary>
        /// The current store property.
        /// </summary>
        public static readonly DependencyProperty CurrentStoreProperty = DependencyProperty.Register(
            "CurrentStore",
            typeof(string),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// The persistent palette property.
        /// </summary>
        public static readonly DependencyProperty PersistentPaletteProperty =
            DependencyProperty.Register(
                "PersistentPalette",
                typeof(ObservableCollection<ColorWrapper>),
                typeof(ColorPicker),
                new UIPropertyMetadata(CreateEmptyPalette()));

        /// <summary>
        /// The selected persistent color property.
        /// </summary>
        private static readonly DependencyProperty SelectedPersistentColorProperty =
            DependencyProperty.Register(
                "SelectedPersistentColor",
                typeof(ColorWrapper),
                typeof(ColorPicker),
                new FrameworkPropertyMetadata(
                    new ColorWrapper(Colors.Black),
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    SelectedPersistentColorChanged));

#pragma warning disable 649
        /// <summary>
        /// Event handler for use by multiple controls
        /// </summary>
        private readonly MouseButtonEventHandler mouseEvent;
#pragma warning restore 649

        /// <summary>
        /// Reference to the listbox that holds the persistent palette
        /// </summary>
        private ListBox persistentList;

        /// <summary>
        /// Reference to the listbox that holds the static palette
        /// </summary>
        private ListBox staticList;

        /// <summary>
        /// The mode.
        /// </summary>
        private enum Mode
        {
            /// <summary>
            /// The none.
            /// </summary>
            None,

            /// <summary>
            /// The add.
            /// </summary>
            Add,

            /// <summary>
            /// The remove.
            /// </summary>
            Remove,

            /// <summary>
            /// The update.
            /// </summary>
            Update
        };

        /// <summary>
        /// Gets or sets DefaultPalettePath.
        /// </summary>
        public static string DefaultPalettePath { get; set; }

        /// <summary>
        /// Gets or sets the settings file where the ColorPicker will store user settings.
        /// </summary>
        /// <value>The settings file.</value>
        public static string SettingsFile { get; set; }

        /// <summary>
        /// Gets or sets CurrentStore.
        /// </summary>
        public string CurrentStore
        {
            get
            {
                return (string)this.GetValue(CurrentStoreProperty);
            }

            set
            {
                this.SetValue(CurrentStoreProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets PersistentPalette.
        /// </summary>
        public ObservableCollection<ColorWrapper> PersistentPalette
        {
            get
            {
                return (ObservableCollection<ColorWrapper>)this.GetValue(PersistentPaletteProperty);
            }

            set
            {
                this.SetValue(PersistentPaletteProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets SelectedPersistentColor.
        /// </summary>
        public ColorWrapper SelectedPersistentColor
        {
            get
            {
                return (ColorWrapper)this.GetValue(SelectedPersistentColorProperty);
            }

            set
            {
                this.SetValue(SelectedPersistentColorProperty, value);
            }
        }

        /// <summary>
        /// Creates the empty palette.
        /// </summary>
        /// <returns>
        /// </returns>
        public static ObservableCollection<ColorWrapper> CreateEmptyPalette()
        {
            // Create an 'empty' palette with a few transparent items.
            var palette = new ObservableCollection<ColorWrapper>();
            for (int i = 0; i < 8; ++i)
            {
                palette.Add(new ColorWrapper(Colors.Transparent));
            }

            return palette;
        }

        /// <summary>
        /// Loads the palette.
        /// </summary>
        /// <param name="picker">
        /// The picker.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        public void LoadPalette(ColorPicker picker, string path)
        {
            string s = File.ReadAllText(path, Encoding.UTF8);
            string[] colors = s.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            this.StoreLastUsedPalette(path);

            var palette = new ObservableCollection<ColorWrapper>();

            foreach (string c in colors)
            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(c);
                    palette.Add(new ColorWrapper(color));
                }
                catch
                {
                    // Silently ignore
                }
            }

            picker.PersistentPalette = palette.Count > 0 ? palette : CreateEmptyPalette();
        }

        /// <summary>
        /// Stores the palette.
        /// </summary>
        /// <param name="picker">
        /// The picker.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        public void StorePalette(ColorPicker picker, string path)
        {
            // Write the colors as text
            var sb = new StringBuilder();
            foreach (var cw in picker.PersistentPalette)
            {
                sb.AppendFormat("{0}\n", cw.Color.ToString(CultureInfo.InvariantCulture));
            }

            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);

            this.StoreLastUsedPalette(path);
        }

        /// <summary>
        /// Gets the mode of operation
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.
        /// </param>
        /// <returns>
        /// The mode of operation for the click event
        /// </returns>
        private Mode GetMode(object sender, MouseButtonEventArgs e)
        {
            Mode result = Mode.None;

            if (e.ChangedButton == MouseButton.Right)
            {
                // User used the right button to click, get mode based on where he/she clicked.
                if (sender == this.persistentList)
                {
                    // Only update or remove modes possible when clicking the persistent list
                    if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                    {
                        result = Mode.Remove;
                    }
                    else if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
                    {
                        result = Mode.Update;
                    }
                }
                else
                {
                    // All other clicks results in an added color.
                    result = Mode.Add;
                }
            }

            return result;
        }

        /// <summary>
        /// Links the palette event handlers.
        /// </summary>
        private void InitializePaletteSettings()
        {
            SettingsFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "colorpicker.settings");
            DefaultPalettePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "colorpicker.default.palette");

            // Find and setup the event handler for the palettes in the ControlTemplate
            this.persistentList = this.Template.FindName("PART_PersistentColorList", this) as ListBox;
            if (this.persistentList != null)
            {
                this.persistentList.MouseUp += this.mouseEvent;
            }

            this.staticList = this.Template.FindName("PART_StaticColorList", this) as ListBox;
            if (this.staticList != null)
            {
                this.staticList.MouseUp += this.mouseEvent;
            }

            // Allow user to click on the color box to add the current color to the palette
            var rect = this.Template.FindName("PART_CurrentColorBox", this) as Rectangle;
            if (rect != null)
            {
                rect.MouseUp += this.mouseEvent;
            }

            var b = this.Template.FindName("PART_LoadPalette", this) as Button;
            if (b != null)
            {
                b.Click += this.LoadPalette_Click;
            }

            b = this.Template.FindName("PART_SavePalette", this) as Button;
            if (b != null)
            {
                b.Click += this.SavePalette_Click;
            }
        }

        /// <summary>
        /// The load last palette.
        /// </summary>
        private void LoadLastPalette()
        {
            try
            {
                if (!File.Exists(SettingsFile))
                {
                    // Use default palette
                    this.StorePalette(this, DefaultPalettePath);
                }

                this.LoadPalette(this, File.ReadAllText(SettingsFile, Encoding.UTF8));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Handles the Click event of the LoadPalette control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void LoadPalette_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var open = new OpenFileDialog())
                {
                    open.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    open.Filter = "Palette files (*.palette)|*.palette";
                    open.FilterIndex = 0;
                    open.RestoreDirectory = true;
                    open.CheckFileExists = true;
                    open.CheckPathExists = true;
                    open.DefaultExt = "palette";
                    open.Multiselect = false;
                    if (open.ShowDialog() == DialogResult.OK)
                    {
                        this.LoadPalette(this, open.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        /// <summary>
        /// Handles the MouseUp event of the PersistentList control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.
        /// </param>
        private void PaletteList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var cw = this.persistentList.SelectedItem as ColorWrapper;
            var items = this.persistentList.ItemsSource as ObservableCollection<ColorWrapper>;

            if (items != null)
            {
                // Get the operation mode based on either keys or mouse click-position
                Mode m = this.GetMode(sender, e);

                if (m == Mode.Add)
                {
                    // Add another color item
                    items.Insert(0, new ColorWrapper(this.SelectedColor));
                    this.UpdateCurrentPaletteStore();
                }
                else if (m == Mode.Remove)
                {
                    // Remove current color, but only if there are two or more items left and there is a selected item
                    // and the user clicked the persistent list
                    if (cw != null && items.Count > 1)
                    {
                        items.Remove(cw);
                        this.UpdateCurrentPaletteStore();
                    }
                }
                else if (cw != null)
                {
                    if (m == Mode.Update)
                    {
                        // Update the persistent palette with the current color
                        cw.Color = this.SelectedColor;
                        this.UpdateCurrentPaletteStore();
                    }
                    else if (sender == this.persistentList)
                    {
                        // No key pressed, just update the current color
                        this.SelectedColor = cw.Color;
                    }
                }

                // Event handled if mode was anything other than None
                e.Handled = m != Mode.None;
            }
        }

        /// <summary>
        /// Handles the Click event of the SavePalette control.
        /// </summary>
        /// <param name="sender">
        /// The source of the event.
        /// </param>
        /// <param name="e">
        /// The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void SavePalette_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var save = new SaveFileDialog())
                {
                    save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    save.Filter = "Palette files (*.palette)|*.palette";
                    save.FilterIndex = 0;
                    save.RestoreDirectory = true;
                    save.OverwritePrompt = true;
                    save.CheckPathExists = true;
                    save.DefaultExt = "palette";
                    save.AddExtension = true;

                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        this.StorePalette(this, save.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        /// <summary>
        /// Stores the last used palette.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        private void StoreLastUsedPalette(string path)
        {
            // Store last used palette
            try
            {
                File.WriteAllText(SettingsFile, path, Encoding.UTF8);
                this.CurrentStore = Path.GetFileNameWithoutExtension(path);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Updates the current palette store.
        /// </summary>
        private void UpdateCurrentPaletteStore()
        {
            try
            {
                if (File.Exists(SettingsFile))
                {
                    string s = File.ReadAllText(SettingsFile, Encoding.UTF8);
                    this.StorePalette(this, s);
                }
            }
            catch (Exception)
            {
                // Silently ignore
            }
        }

    }
}