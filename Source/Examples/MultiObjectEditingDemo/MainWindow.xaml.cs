using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MultiObjectEditingDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            People = new ObservableCollection<Person>
                         {
                             new Person {Name = "John", Age = 32},
                             new Person {Name = "Mary", Age = 33},
                             new Person {Name = "Roger", Age = 31},
                         };
            DataContext = this;
        }

        public ObservableCollection<Person> People { get; set; }
    }

    public class Person : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged("Name"); }
        }

        private int age;
        public int Age
        {
            get { return age; }
            set { age = value; RaisePropertyChanged("Age"); }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Name, Age);
        }

        private bool isMarried;
        public bool IsMarried
        {
            get { return isMarried; }
            set { isMarried = value; RaisePropertyChanged("IsMarried"); }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; RaisePropertyChanged("IsSelected"); }
        }

        private DateTime? birthDate;
        public DateTime? BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; RaisePropertyChanged("BirthDate"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}
