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

namespace ViewModelDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            var p1 = new Person() { Name = "John Johnson", Age = 59, Height = 1.78 };
            var p2 = new Person() { Name = "Bill Williams", Age = 59, Height = 1.72 };
            
            var people = new List<Person>();
            people.Add(p1);
            people.Add(p2);
//            editor1.SelectedObjects = people;

            var pvm = new PersonViewModel(p1);
            editor1.SelectedObject = pvm;
            group1.DataContext = pvm;
        }
    }
}
