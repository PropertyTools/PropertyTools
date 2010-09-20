using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using PropertyEditorLibrary;

namespace FeaturesDemo
{
    #region Enums
    public enum Genders
    {
        Male,
        Female
    } ;

    public enum YesOrNo
    {
        No,
        Yes
    }
    #endregion

    #region Mass
    [TypeConverter(typeof(MassConverter))]
    public class Mass
    {
        public double Value { get; set; }

        public static Mass Parse(string s)
        {
            s = s.Replace(',', '.').Trim();
            var r = new Regex(@"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?");
            Match m = r.Match(s);
            if (!m.Success) return null;
            double value = double.Parse(m.Groups[0].Value, CultureInfo.InvariantCulture);
            // string unit = m.Groups[1].Value;
            return new Mass { Value = value };
        }

        public override string ToString()
        {
            return String.Format("{0:N0} kg", Value, CultureInfo.InvariantCulture);
        }
    }

    public class MassConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return Mass.Parse((string)value);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
    #endregion

    public class SuperExampleObject : ExampleObject
    {
        [Category("Subclass|Subclass properties"),SortOrder(999)]
        public string String2 { get; set; }
        public float Float2 { get; set; }
    }

    public class ExampleObject : Observable
    {
        public ExampleObject()
        {
            String = "Lorem ipsum";
            Byte = 255;
            Integer = -1;
            Double = 12.3;
            Boolean = true;
            Font = new FontFamily("Arial");
            Color = Colors.Blue;
            SolidBrush = Brushes.Gold;

            ReadonlyString = "Ipsum lorem";
            ReadonlyInt = 123;
            ReadonlyBool = true;

            SliderDouble = 4.31;
            SliderInt = 37;
            FormattedDouble = Math.PI;
        }
        // When the [Category(...)] attribute is used with a "|", the
        // first part is the header of the tab and the second part is
        // the header of the category
        // The order of the properties are not guaranteed to be fixed

        [Category("Basic|Fundamental types"),SortOrder(100)]
        public string String { get; set; }
        public byte Byte { get; set; }
        public int Integer { get; set; }
        public double Double { get; set; }
        public bool Boolean { get; set; }
        public Genders Enum { get; set; }
       
        [Category("Type with ValueConverter")]
        public Mass Weight { get; set; }

        [Category("FormatString")]
        [FormatString("0.0000")]
        public double FormattedDouble { get; set; }

        [Category("ReadOnly properties")]
        public string ReadonlyString { get; private set; }
        public int ReadonlyInt { get; private set; }
        public bool ReadonlyBool { get; private set; }

        [Category("Advanced|Optional properties"),SortOrder(200)]
        public int? OptionalInteger { get; set; }
        public double? OptionalDouble { get; set; }
        [Optional]
        public string OptionalString { get; set; }
        
        // This property is used to control the optional property
        // The property should be public, but not browsable
        [Browsable(false)]
        public bool HasValue { get; set; }

        [Optional("HasValue")]
        public int Value { get; set; }

        [Category("Enabled/disabled properties")]
        public string String3 { get; set; }
        public bool IsString3Enabled { get; set; }
        

        [Category("Slidable properties")]
        [Slidable(0,10,0.25,2.5)]
        [FormatString("0.00")]
        public double SliderDouble { get; set; }
        [Slidable(0, 100, 1, 100)]
        public int SliderInt { get; set; }        

        [Category("Special editors")]
        public Color Color { get; set; }
        public Brush SolidBrush { get; set; }
        public FontFamily Font { get; set; }

        [FilePath("Image files (*.jpg)|*.jpg|All files (*.*)|*.*", ".jpg")]
        public string FilePath { get; set; }
        [DirectoryPath]
        public string DirectoryPath { get; set; }

        // Use [WideProperty] to use the full width of the control
        // Use [Height(...)] to set the height of a multiline text control
        [WideProperty, Height(100)]
        public string WideMultilineString { get; set; }

        [Category("Custom editors")]
        public DateTime DateTime { get; set; }

        [Category("Described property"), DisplayName("Property display name"),
        Description("This is the description."), SortOrder(401)]
        public bool Descripted { get; set; }
    }

    public class Observable : INotifyPropertyChanged
    {
        internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}