using System;
using System.Collections.Generic;
using System.Windows;
using PropertyEditorLibrary;

namespace ViewModelDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public PersonViewModel Person { get; private set; }
        public IPropertyStateProvider PropertyStateProvider { get; private set; }

        public Window1()
        {
            InitializeComponent();
            PropertyStateProvider = new PropertyStateProvider();
            var p1 = new Person() { Name = "John Johnson", Age = 59, Height = 1.78 };
            var p2 = new Person() { Name = "Bill Williams", Age = 59, Height = 1.72 };

            var people = new List<Person>();
            people.Add(p1);
            people.Add(p2);

            Person = new PersonViewModel(p1);
            DataContext = this;
        }
    }
}
