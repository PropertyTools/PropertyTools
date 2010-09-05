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

namespace ControlsDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Scale = 2.5;
            DataContext = this;
        }

        public double Scale { get; set; }
        public int Number { get; set; }
        public Color Color { get; set; }
        public SolidColorBrush Brush { get; set; }
        public string FilePath { get; set; }
        public TestEnum TestEnum { get; set; }
        public string Text { get; set; }
    }

    public enum TestEnum
    {
        Apples,
        Pears,
        Oranges
    } ;
}
