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
    /// Interaction logic for RadioButtonListPage.xaml
    /// </summary>
    public partial class RadioButtonListPage : Page
    {
        public TestEnum TestEnum { get; set; }

        public RadioButtonListPage()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
    public enum TestEnum
    {
        Apples,
        Pears,
        Oranges
    } ;
}
