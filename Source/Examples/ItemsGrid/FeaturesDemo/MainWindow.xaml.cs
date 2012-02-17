using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FeaturesDemo
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ExampleObject> Items { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Items = new ObservableCollection<ExampleObject>();
            Items.Add(new ExampleObject() { Boolean = true, DateTime = DateTime.Now, Color = Colors.Blue, Double = Math.PI, Enum = Fruit.Apple, Integer = 7, Selector = null, String = "Hello" });
            Items.Add(new ExampleObject());
        }
    }
}
