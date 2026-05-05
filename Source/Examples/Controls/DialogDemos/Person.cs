// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DialogDemos
{
    using System.ComponentModel;

    public class Person : Observable
    {
        private string firstName;
        public string FirstName
        {
            get { return this.firstName; }
            set { this.firstName = value; OnPropertyChanged(nameof(FirstName)); }
        }

        private string lastName;
        public string LastName
        {
            get { return this.lastName; }
            set { this.lastName = value; OnPropertyChanged(nameof(LastName)); }
        }

        public override string ToString()
        {
            return this.FirstName + " " + this.LastName;
        }
    }

    public class Observable : INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}