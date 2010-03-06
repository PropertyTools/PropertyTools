using System;
using System.ComponentModel;

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

        public Person()
        {
            Name = "Joe";
            BirthDate = DateTime.Today.AddYears(-25);
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
