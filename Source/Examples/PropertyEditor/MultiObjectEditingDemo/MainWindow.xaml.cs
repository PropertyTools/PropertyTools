using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using PropertyTools.Wpf;
using System.Windows.Data;

namespace MultiObjectEditingDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            People = new ObservableCollection<Person>
                         {
                             new Person {Name = "John", Age = 32, Coolness = CoolnessLevel.Cool},
                             new Person {Name = "Mary", Age = 33, Coolness = CoolnessLevel.Cool},
                             new Person {Name = "Roger", Age = 31, Coolness = CoolnessLevel.Uncool},
                         };
            foreach (var p in People)
                p.PropertyChanged += Person_PropertyChanged;
            DataContext = this;
        }

        private void Person_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                RaisePropertyChanged("SelectedPeople");
            }
        }

        public ObservableCollection<Person> People { get; set; }

        public IList<Person> SelectedPeople
        {
            get
            {
                return People.Where(p => p.IsSelected).ToList();
            }
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

    public enum CoolnessLevel
    {
        Cool,
        Uncool
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
            get
            {
                if (BirthDate.HasValue)
                    return (int)((DateTime.Now - BirthDate.Value).TotalDays / 365.25);
                return age;
            }
            set { 
                if (!BirthDate.HasValue)
                    age = value; 
                RaisePropertyChanged("Age"); }
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

        private DateTime? birthDate;
        [FormatString("dd.MM.yyyy")]
        public DateTime? BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; RaisePropertyChanged("BirthDate"); RaisePropertyChanged("Age"); }
        }

        private CoolnessLevel coolness;
        public CoolnessLevel Coolness
        {
            get { return coolness; }
            set { coolness = value; RaisePropertyChanged("Coolness"); }
        }

        private bool isSelected;
        [Browsable(false)]
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; RaisePropertyChanged("IsSelected"); }
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

    public class MyEnumToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (CoolnessLevel)Enum.Parse(typeof(CoolnessLevel), value.ToString(), true);
        }
    }
}
