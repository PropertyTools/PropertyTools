// Persistent color palette support courtesy of Per Malmberg <per.malmberg@gmail.com>
// 
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Shapes;
using PropertyTools.Wpf.Controls.ColorPicker;

namespace PropertyTools.Wpf
{
	/// <summary>
	/// 
	/// </summary>
	public partial class ColorPicker
	{
		private enum Mode { None, Add, Remove, Update };

		/// <summary>
		/// Gets or sets the settings file where the ColorPicker will store user settings.
		/// </summary>
		/// <value>The settings file.</value>
		public static string SettingsFile { get; set; }

		public static string DefaultPalettePath { get; set; }

		// Private as it should never be used outside the ColorPicker class
		private static readonly DependencyProperty SelectedPersistentColorProperty =
			DependencyProperty.Register( "SelectedPersistentColor", typeof( ColorWrapper ), typeof( ColorPicker ),
										new FrameworkPropertyMetadata( new ColorWrapper( Colors.Black ),
																	  FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
																	  SelectedPersistentColorChanged ) );

		public static readonly DependencyProperty PersistentPaletteProperty =
			DependencyProperty.Register( "PersistentPalette", typeof( ObservableCollection<ColorWrapper> ), typeof( ColorPicker ),
								new UIPropertyMetadata( CreateEmptyPalette() ) );

		public static readonly DependencyProperty CurrentStoreProperty =
			DependencyProperty.Register( "CurrentStore", typeof( string ), typeof( ColorPicker ),
												new FrameworkPropertyMetadata( "",
																			FrameworkPropertyMetadataOptions.BindsTwoWayByDefault ) );

		/// <summary>
		/// Reference to the listbox that holds the static palette
		/// </summary>
		private ListBox staticList;

		/// <summary>
		/// Reference to the listbox that holds the persistent palette
		/// </summary>
		private ListBox persistentList;

		/// <summary>
		/// Event handler for use by multiple controls
		/// </summary>
		private MouseButtonEventHandler mouseEvent;

		public ObservableCollection<ColorWrapper> PersistentPalette
		{
			get { return (ObservableCollection<ColorWrapper>)GetValue( PersistentPaletteProperty ); }
			set { SetValue( PersistentPaletteProperty, value ); }
		}

		public ColorWrapper SelectedPersistentColor
		{
			get { return (ColorWrapper)GetValue( SelectedPersistentColorProperty ); }
			set { SetValue( SelectedPersistentColorProperty, value ); }
		}

		public string CurrentStore
		{
			get { return (string)GetValue( CurrentStoreProperty ); }
			set { SetValue( CurrentStoreProperty, value ); }
		}

		/// <summary>
		/// Links the palette event handlers.
		/// </summary>
		private void InitializePaletteSettings()
		{
			SettingsFile = System.IO.Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ), "colorpicker.settings" );
			DefaultPalettePath = System.IO.Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ), "colorpicker.default.palette" );

			// Find and setup the event handler for the palettes in the ControlTemplate
			persistentList = Template.FindName( "PART_PersistentColorList", this ) as ListBox;
			if( persistentList != null ) {
				persistentList.MouseUp += mouseEvent;
			}

			staticList = Template.FindName( "PART_StaticColorList", this ) as ListBox;
			if( staticList != null ) {
				staticList.MouseUp += mouseEvent;
			}

			// Allow user to click on the color box to add the current color to the palette
			Rectangle rect = Template.FindName( "PART_CurrentColorBox", this ) as Rectangle;
			if( rect != null ) {
				rect.MouseUp += mouseEvent;
			}

			Button b = Template.FindName( "PART_LoadPalette", this ) as Button;
			if( b != null ) {
				b.Click += new RoutedEventHandler( LoadPalette_Click );
			}

			b = Template.FindName( "PART_SavePalette", this ) as Button;
			if( b != null ) {
				b.Click += new RoutedEventHandler( SavePalette_Click );
			}
		}

		/// <summary>
		/// Handles the Click event of the LoadPalette control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void LoadPalette_Click( object sender, RoutedEventArgs e )
		{
			try {
				using( System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog() ) {
					open.InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData );
					open.Filter = "Palette files (*.palette)|*.palette";
					open.FilterIndex = 0;
					open.RestoreDirectory = true;
					open.CheckFileExists = true;
					open.CheckPathExists = true;
					open.DefaultExt = "palette";
					open.Multiselect = false;
					if( open.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
						LoadPalette( this, open.FileName );
					}
				}
			}
			catch( Exception ex ) {
				MessageBox.Show( ex.Message, "Error" );
			}

		}

		/// <summary>
		/// Handles the Click event of the SavePalette control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
		void SavePalette_Click( object sender, RoutedEventArgs e )
		{
			try {
				using( System.Windows.Forms.SaveFileDialog save = new System.Windows.Forms.SaveFileDialog() ) {
					save.InitialDirectory = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData );
					save.Filter = "Palette files (*.palette)|*.palette";
					save.FilterIndex = 0;
					save.RestoreDirectory = true;
					save.OverwritePrompt = true;
					save.CheckPathExists = true;
					save.DefaultExt = "palette";
					save.AddExtension = true;

					if( save.ShowDialog() == System.Windows.Forms.DialogResult.OK ) {
						StorePalette( this, save.FileName );
					}
				}
			}
			catch( Exception ex ) {
				MessageBox.Show( ex.Message, "Error" );
			}
		}

		/// <summary>
		/// Handles the MouseUp event of the PersistentList control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
		void PaletteList_MouseUp( object sender, MouseButtonEventArgs e )
		{
			ColorWrapper cw = persistentList.SelectedItem as ColorWrapper;
			ObservableCollection<ColorWrapper> items = persistentList.ItemsSource as ObservableCollection<ColorWrapper>;

			if( items != null ) {
				// Get the operation mode based on either keys or mouse click-position
				Mode m = GetMode( sender, e );

				if( m == Mode.Add ) {
					// Add another color item
					items.Insert( 0, new ColorWrapper( SelectedColor ) );
					UpdateCurrentPaletteStore();
				}
				else if( m == Mode.Remove ) {
					// Remove current color, but only if there are two or more items left and there is a selected item
					// and the user clicked the persistent list
					if( cw != null && items.Count > 1 ) {
						items.Remove( cw );
						UpdateCurrentPaletteStore();
					}
				}
				else if( cw != null ) {
					if( m == Mode.Update ) {
						// Update the persistent palette with the current color	
						cw.Color = SelectedColor;
						UpdateCurrentPaletteStore();
					}
					else if( sender == persistentList ) {
						// No key pressed, just update the current color
						SelectedColor = cw.Color;
					}
				}

				// Event handled if mode was anything other than None
				e.Handled = m != Mode.None;
			}
		}

		/// <summary>
		/// Gets the mode of operation
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="System.Windows.Input.MouseButtonEventArgs"/> instance containing the event data.</param>
		/// <returns>The mode of operation for the click event</returns>
		private Mode GetMode( object sender, MouseButtonEventArgs e )
		{
			Mode result = Mode.None;

			if( e.ChangedButton == MouseButton.Right ) {
				// User used the right button to click, get mode based on where he/she clicked.
				if( sender == persistentList ) {
					// Only update or remove modes possible when clicking the persistent list
					if( Keyboard.IsKeyDown( Key.LeftShift ) || Keyboard.IsKeyDown( Key.RightShift ) ) {
						result = Mode.Remove;
					}
					else if( Keyboard.IsKeyDown( Key.LeftCtrl ) || Keyboard.IsKeyDown( Key.RightCtrl ) ) {
						result = Mode.Update;
					}
				}
				else {
					// All other clicks results in an added color.
					result = Mode.Add;
				}
			}

			return result;
		}

		/// <summary>
		/// Updates the current palette store.
		/// </summary>
		private void UpdateCurrentPaletteStore()
		{
			try {
				if( File.Exists( SettingsFile ) ) {
					string s = File.ReadAllText( SettingsFile, Encoding.UTF8 );
					StorePalette( this, s );
				}
			}
			catch( Exception ) {
				// Silently ignore
			}
		}

		/// <summary>
		/// Creates the empty palette.
		/// </summary>
		/// <returns></returns>
		public static ObservableCollection<ColorWrapper> CreateEmptyPalette()
		{
			// Create an 'empty' palette with a few transparent items.
			ObservableCollection<ColorWrapper> palette = new ObservableCollection<ColorWrapper>();
			for( int i = 0; i < 8; ++i ) {
				palette.Add( new ColorWrapper( Colors.Transparent ) );
			}
			return palette;
		}

		/// <summary>
		/// Stores the palette.
		/// </summary>
		/// <param name="picker">The picker.</param>
		/// <param name="path">The path.</param>
		public void StorePalette( ColorPicker picker, string path )
		{
			// Write the colors as text
			StringBuilder sb = new StringBuilder();
			foreach( ColorWrapper cw in picker.PersistentPalette ) {
				sb.AppendFormat( "{0}\n", cw.Color.ToString( CultureInfo.InvariantCulture ) );
			}

			File.WriteAllText( path, sb.ToString(), Encoding.UTF8 );

			StoreLastUsedPalette( path );
		}

		/// <summary>
		/// Stores the last used palette.
		/// </summary>
		/// <param name="path">The path.</param>
		private void StoreLastUsedPalette( string path )
		{
			// Store last used palette
			try { 
				File.WriteAllText( SettingsFile, path, Encoding.UTF8 );
				CurrentStore = System.IO.Path.GetFileNameWithoutExtension( path );
			}
			catch( Exception ) { }
		}

		/// <summary>
		/// Loads the palette.
		/// </summary>
		/// <param name="picker">The picker.</param>
		/// <param name="path">The path.</param>
		public void LoadPalette( ColorPicker picker, string path )
		{
			string s = File.ReadAllText( path, Encoding.UTF8 );
			string[] colors = s.Split( new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries );

			StoreLastUsedPalette( path );

			ObservableCollection<ColorWrapper> palette = new ObservableCollection<ColorWrapper>();

			foreach( string c in colors ) {
				try {
					Color color = (Color)ColorConverter.ConvertFromString( c );
					palette.Add( new ColorWrapper( color ) );
				}
				catch {
					// Silently ignore
				}
			}

			picker.PersistentPalette = palette.Count > 0 ? palette : CreateEmptyPalette();
		}

		private void LoadLastPalette()
		{
			try {
				if( !File.Exists( SettingsFile ) ) {
					// Use default palette
					StorePalette( this, DefaultPalettePath );
				}

				LoadPalette( this, File.ReadAllText( SettingsFile, Encoding.UTF8 ) );
			}
			catch( Exception ) { }
		}
	}
}
