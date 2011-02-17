using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SimpleGridDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, CloseExecute));

            int rows = 32;
            int columns = 16;

            Data = new string[rows, columns];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    Data[i, j] = "Cell " + i + "," + j;

            DataContext = this;

            Items = new ObservableCollection<City>();
            Items.Add(new City() { Name = "Oslo", Country = "Norway", Population = 590041, IsCapital = true });
            Items.Add(new City() { Name = "Bergen", Country = "Norway", Population = 257864 });
            Items.Add(new City() { Name = "Stockholm", Country = "Sweden", Population = 818603, IsCapital = true });
            Items.Add(new City() { Name = "Copenhagen", Country = "Denmark", Population = 1181239, IsCapital = true });
            Items.Add(new City() { Name = "London", Country = "United Kingdom", Population = 7556900, IsCapital = true });

            StringItems = new Collection<string>();
            StringItems.Add("Swix");
            StringItems.Add("SkiGo");
            StringItems.Add("Rode");
            StringItems.Add("Holmenkol");

            MassItems = new Collection<Mass>();

            ExampleItems = new Collection<ExampleObject>();
            ExampleItems.Add(new ExampleObject() { DateTime = DateTime.Now, String = "Now" });
            for (int n=0;n<10;n++)
                ExampleItems.Add(new ExampleObject() { DateTime = DateTime.UtcNow, String = "UtcNow" });

            var rot = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 45);
            var rt = new RotateTransform3D(rot);
            var mt = new Transform3DGroup();
            mt.Children.Add(rt);
            var a = new double[4, 4];
            a[0, 0] = mt.Value.M11; a[0, 1] = mt.Value.M12; a[0, 2] = mt.Value.M13; a[0, 3] = mt.Value.M14;
            a[1, 0] = mt.Value.M21; a[1, 1] = mt.Value.M22; a[1, 2] = mt.Value.M23; a[1, 3] = mt.Value.M24;
            a[2, 0] = mt.Value.M31; a[2, 1] = mt.Value.M32; a[2, 2] = mt.Value.M33; a[2, 3] = mt.Value.M34;
            a[3, 0] = mt.Value.OffsetX; a[3, 1] = mt.Value.OffsetY; a[3, 2] = mt.Value.OffsetZ; a[3, 3] = mt.Value.M44;
            Matrix = a;

            Vector = new[] { "Apples", "Pears", "Bananas" };
            EmptyCollection = new Collection<ExampleObject>();
        }

        public string[,] Data { get; set; }
        public ObservableCollection<City> Items { get; set; }
        public Collection<ExampleObject> ExampleItems { get; set; }
        public Collection<ExampleObject> EmptyCollection { get; set; }
        public Collection<string> StringItems { get; set; }
        public Collection<Mass> MassItems { get; set; }
        public double[,] Matrix { get; set; }
        public string[] Vector { get; set; }

        private void CloseExecute(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }

    public class City : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged("Name"); }
        }

        private string country;
        public string Country
        {
            get { return country; }
            set { country = value; RaisePropertyChanged("Country"); }
        }

        private int population;
        public int Population
        {
            get { return population; }
            set { population = value; RaisePropertyChanged("Population"); }
        }

        private bool isCapital;
        public bool IsCapital
        {
            get { return isCapital; }
            set { isCapital = value; RaisePropertyChanged("IsCapital"); }
        }

        #region PropertyChanged Block
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            Debug.WriteLine(property + " was changed.");
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion


    }

    public class ExampleObject
    {
        public bool Boolean { get; set; }
        public Island Enum { get; set; }
        public double Double { get; set; }
        public int Integer { get; set; }
        public DateTime DateTime { get; set; }
        public string String { get; set; }
        public Color Color { get; set; }
    }

    [TypeConverter(typeof(MassConverter))]
    public class Mass
    {
        public double Value { get; set; }

        public static Mass Parse(string s)
        {
            s = s.Replace(',', '.').Trim();
            var r = new Regex(@"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?");
            Match m = r.Match(s);
            if (!m.Success)
                return null;
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

    public enum Island { Iceland, Java, Sumatra, Taiwan, Madagascar, Borneo, Cuba, Hainan, PuertoRico, Mauritius, Sardinia,Tasmania, Trinidad, Madeira, Hawaii, Oahu, Kauai, Maui, Tahiti, Fiji, BoraBora, Maldives, Reunion, Seychelles }
}