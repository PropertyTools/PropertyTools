using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
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
            for (int n = 0; n < 10; n++)
                ExampleItems.Add(new ExampleObject { DateTime = DateTime.Now.AddHours(n), Uri=new Uri("http://www.google.com") });

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
        }

        public string[,] Data { get; set; }
        public double[,] Matrix { get; set; }
        public string[] Vector { get; set; }

        public ObservableCollection<City> Items { get; set; }
        public Collection<ExampleObject> ExampleItems { get; set; }
        public Collection<string> StringItems { get; set; }
        public Collection<Mass> MassItems { get; set; }
        public IEnumerable IEnumerable { get { return GetValues(); } }
        public IEnumerable<ExampleObject> IEnumerable2 { get { return GetValues2(); } }

        private IEnumerable<ExampleObject> GetValues2()
        {
            for (int i = 0; i < 100; i++)
                yield return new ExampleObject() { Integer = i };
        }

        private IEnumerable GetValues()
        {
            for (int i = 0; i < 100; i++)
                yield return new ExampleObject() {Integer = i};
        }

        public IEnumerable<string> IEnumerableString { get; set; }

        private void CloseExecute(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }
    }

    public class City : Observable
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
    }

    public enum Fruit { Apple, Pear, Banana, Orange }
}