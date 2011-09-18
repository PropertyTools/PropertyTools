using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows.Media;
using PropertyTools.Wpf;
using System.Windows.Controls.Primitives;

namespace FeaturesDemo
{

    public enum Genders
    {
        Male,
        Female
    }

    // Example Enum with description attributes
    public enum House
    {
        [Description("Small house")]
        SmallHouse,
        [Description("Big house")]
        BigHouse,
        [Description("Apartment")]
        Apartment,
        [Description("Yurt")]
        Yurt,
        [Description("Straw hut")]
        StrawHut
    }

    public enum YesOrNo
    {
        No,
        Yes
    }

    public class ExampleObject : Observable, IResettableProperties, IEditableObject
    {
        public ExampleObject()
        {
            String = "Lorem ipsum";
            Byte = 255;
            Integer = -1;
            Double = 12.3;
            Boolean = true;

            DateTime1 = DateTime2 = DateTime.Now;
            TimeSpan1 = TimeSpan2 = TimeSpan.FromHours(39.5);

            Font = new FontFamily("Arial");
            Color = Colors.Blue;
            SolidBrush = Brushes.Gold;

            ReadonlyString = "Ipsum lorem";
            ReadonlyInt = 123;
            ReadonlyBool = true;

            SliderDouble = 4.31;
            SliderInt = 37;
            FormattedDouble = Math.PI;
            FormattedDouble2 = Math.E;

            Password = "OpenSesame!";
        }

        // When the [Category(...)] attribute is used with a "|", the
        // first part is the header of the tab and the second part is
        // the header of the category

        // The Category and SortOrder is shared for following properties
        // but the order of the properties is not guaranteed to be fixed...

        [Category("Basic|Fundamental types"), SortOrder(100)]
        public string String { get; set; }

        public byte Byte { get; set; }
        public int Integer { get; set; }
        public double Double { get; set; }
        public float Float { get; set; }
        public bool Boolean { get; set; }
        public Genders Enum { get; set; }
        [RadioButtons]
        public YesOrNo Vote { get; set; }

        [Category("Enum with Description attributes")]
        public House House { get; set; }

        [Category("Type with ValueConverter")]
        public Mass Weight { get; set; }

        [Category("DateTime")]
        public DateTime DateTime1 { get; set; }
        [FormatString("yyyy-MM-dd")]       
        public DateTime DateTime2 { get; set; }

        [Category("TimeSpan")]
        public TimeSpan TimeSpan1 { get; set; }
        [FormatString("HH:mm")] // remember to escape ":" and "." characters!
        public TimeSpan TimeSpan2 { get; set; }

        [Category("FormatString")]
        [FormatString("0.0000")]
        public double FormattedDouble { get; set; }

        [FormatString("x = {0:0.00}")]
        public double FormattedDouble2 { get; set; }

        [FormatString("X")]
        public int HexNumber { get; set; }

		[Category( "No FormatString" )]
		public double NonFormattedDouble { get; set; }

        [Category("ReadOnly properties")]
        public string ReadonlyString { get; private set; }

        public int ReadonlyInt { get; private set; }
        public bool ReadonlyBool { get; private set; }

        [Category("Optional|Optional properties"), SortOrder(200)]

        // Optional properties have a checkbox instead of a label
        // The checkbox controls the enabled/disabled state of the property
        // The following properties are disabled when the value is null

        [Optional]
        public int? OptionalInteger { get; set; }

        [Optional]
        public double? OptionalDouble { get; set; }

        [Optional]
        public Color? OptionalColor { get; set; }

        [Optional]
        public string OptionalString { get; set; }

        [Optional]
        // todo: this does not work - EnumValuesConverter does not get the enum type?
        public Genders? OptionalEnum { get; set; }

        // Properties matching the "Use{0}" search pattern will contain the bool if the corresponding property is checked.
        // The pattern can be changed by creating a PropertyViewModelFactory, or setting the UsePropertyPattern attribute
        // in the DefaultPropertyViewModelFactory
        [Browsable(false)]
        public bool UseWindowsVersion { get; set; }

        [Optional]
        public string WindowsVersion { get; set; }


        // This property is used to control the optional property
        // The property should be public, but not browsable
        [Browsable(false)]
        public bool HasValue { get; set; }

        [Optional("HasValue")]
        public int Value { get; set; }

        [Optional("HasValue")]
        public int Value2 { get; set; }

        [Category("Visible/invisible properties")]
        public bool IsString3Visible { get; set; }

        // this property controls the visibility state of String3
        public string String3 { get; set; }

        [Category("Enabled/disabled properties")]
        public bool IsString4Enabled { get; set; }

        // this property controls the state of String4
        public string String4 { get; set; }

        private string autoUpdatingText;

        [Category("Auto updating (trig on PropertyChange)")]
        [AutoUpdateText] // this will trig an update on PropertyChange
        public string AutoUpdatingText
        {
            get { return autoUpdatingText; }
            set { autoUpdatingText = value; Trace.WriteLine(string.Format("AutoUpdatingText='{0}'", value)); }
        }

        // The default behaviour is to update on LostFocus
        private string notAutoUpdatingText;
        public string NotAutoUpdatingText
        {
            get { return notAutoUpdatingText; }
            set { notAutoUpdatingText = value; Trace.WriteLine(string.Format("NotAutoUpdatingText='{0}'", value)); }
        }

        [Category("Slidable|Slidable properties")]
        [Slidable(0, 10, 0.5, 2.5)]
        [FormatString("0.00")]
        public double SliderDouble { get; set; }

        [Slidable(0.1, 99.9, 0.1, 0.5)]
        public float SliderFloat { get; set; }

        [Slidable(0, 100, 1, 10)]
        public int SliderInt { get; set; }

        [Slidable(0, 10, 1, 5, true, 1.5, TickPlacement.BottomRight)]
        public double TickSlider { get; set; }

        [Category("Special|Colors and brushes")]
        public Color Color { get; set; }
        public Color? NullableColor { get; set; }
        public Brush SolidBrush { get; set; }

        [Category("Fonts")]
        public FontFamily Font { get; set; }

        [FilePath("Image files (*.jpg)|*.jpg|All files (*.*)|*.*", ".jpg")]
        public string FilePath { get; set; }

        [DirectoryPath]
        public string DirectoryPath { get; set; }
        
        [Category("Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Category("Wide|Wide properties")]
        // Use [WideProperty] to use the full width of the control
        // Use [Height(...)] to set the height of a multiline text control
        [WideProperty, Height(100)]
        public string WideMultilineString { get; set; }

        // [Category("Custom editors")]
        // todo: not included DateTimePicker
        // public DateTime DateTime { get; set; }

        [Category("Described property"),
         DisplayName("Property display name"),
         Description("This is the description."),
         SortOrder(401)]
        public bool DescribedProperty { get; set; }

        [Category("Reset|Resettable")]
        [Resettable]
        public string ResettableString { get; set; }
        [Resettable("0")]
        public double ResettableDouble { get; set; }
        [Resettable("Default")]
        public Color ResettableColor { get; set; }

        public object GetResetValue(string propertyName)
        {
            switch (propertyName)
            {
                case "ResettableString":
                    return "default";
                case "ResettableDouble":
                    return 0.0;
                case "ResettableColor":
                    return Colors.White;
                default:
                    return null;
            }
        }

        public void BeginEdit()
        {
            Debug.WriteLine("BeginEdit");
        }

        public void EndEdit()
        {
            Debug.WriteLine("EndEdit");
        }

        public void CancelEdit()
        {
            Debug.WriteLine("CancelEdit");
        }
    }
}