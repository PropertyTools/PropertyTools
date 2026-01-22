// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace DialogDemos
{

    public class Person : Observable
    {
        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged(nameof(LastName)); }
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
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