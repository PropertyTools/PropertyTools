using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ExampleBrowser.Pages
{
    /// <summary>
    /// Interaction logic for UncategorizedExample.xaml
    /// </summary>
    public partial class UncategorizedExample : Page
    {
        public UncategorizedExample()
        {
            InitializeComponent();
        DataContext = this;
            TestObject=new TestClass();
        }

        public TestClass TestObject { get; set; }
    }

    public class TestClass
    {
        public int Property1 { get; set; }
        [Category("Category2")]
        public int Property2 { get; set; }
        public int Property3 { get; set; }
        [Category("Category4")]
        public int Property4 { get; set; }
        public int Property5 { get; set; }
    }
    
}
