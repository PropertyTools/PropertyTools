using System;
using System.ComponentModel;
using System.Windows.Media;

namespace CustomEditorDemo
{
    public class Person : Observable
    {
        #region Name
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        #endregion

        #region BirthDate
        private DateTime birthDate;
        public DateTime BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }
        #endregion

        #region CanSwim
        private bool canSwim;
        public bool CanSwim
        {
            get { return canSwim; }
            set { canSwim = value; OnPropertyChanged("CanSwim"); }
        }
        #endregion

        private Color hairColor;
        public Color HairColor
        {
            get { return hairColor; }
            set { hairColor = value; OnPropertyChanged("HairColor"); }
        }

        public Person()
        {
            Name = "Joe";
            BirthDate = DateTime.Today.AddYears(-25);
            HairColor = Colors.Crimson;
        }

    }

    public class Observable : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
