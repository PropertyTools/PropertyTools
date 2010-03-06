using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Media;
using OpenControls;

namespace DialogDemos
{
   
    public class Person : Observable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
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