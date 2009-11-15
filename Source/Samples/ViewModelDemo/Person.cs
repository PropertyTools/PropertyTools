using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ViewModelDemo
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class PersonViewModel : INotifyPropertyChanged
    {
        private readonly Person _model;

        public PersonViewModel(Person model)
        {
            _model = model;
        }

        #region Name (INotifyPropertyChanged Property)
        [Category("Personal data|General"),DisplayName("Full name"),Description("The full name of the person")]
        public string Name
        {
            get { return _model.Name; }
            set
            {
                if (_model.Name != value)
                {
                    _model.Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        #endregion

        #region Age (INotifyPropertyChanged Property)
        [DisplayName("Age (years)"), Description("The age of the person")]
        public int Age
        {
            get { return _model.Age; }
            set
            {
                if (_model.Age != value)
                {
                    _model.Age = value;
                    RaisePropertyChanged("Age");
                }
            }
        }
        #endregion
        
        #region PropertyChanged Block
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion
    }
}
