using System;
using System.ComponentModel;
using OpenControls;

namespace ViewModelDemo
{
    public class PersonViewModel : INotifyPropertyChanged, IDataErrorInfo, IPropertyState
    {
        private readonly Person _model;

        public PersonViewModel(Person model)
        {
            _model = model;
        }
        [Category("Personal data|General")]
        #region Anonymous (INotifyPropertyChanged Property)
        [DisplayName("Is anonymous"), Description("If this person is anonymous")]
        public bool Anonymous
        {
            get { return _model.Anonymous; }
            set
            {
                if (_model.Anonymous != value)
                {
                    _model.Anonymous = value;
                    RaisePropertyChanged("Anonymous");
                }
            }
        }
        #endregion


        #region Name (INotifyPropertyChanged Property)
        [DisplayName("Full name"), Description("The full name of the person")]
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

        #region Height (INotifyPropertyChanged Property)
        [DisplayName("Height (m)"), Description("The height of the person"), 
        Slidable(0, 2, 0.01, 0.1), FormatString("0.00")]
        public double Height
        {
            get { return _model.Height; }
            set
            {
                if (_model.Height != value)
                {
                    _model.Height = value;
                    RaisePropertyChanged("Height");
                }
            }
        }
        #endregion

        #region Age (INotifyPropertyChanged Property)
        [DisplayName("Age (years)"), Description("The age of the person"), Slidable(0,100)]
        public double Age
        {
            get { return _model.Age; }
            set
            {
                if (_model.Age != value)
                {
                    _model.Age = (int)value;
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

        #region IPropertyState members
        public void GetPropertyStates(PropertyState state)
        {
            state.Enable("Age", !String.IsNullOrEmpty(Name));
            state.Enable("Height", !Anonymous);
            state.Enable("Name", !Anonymous);
        }
        #endregion

        #region IDataErrorInfo Members

        [Browsable(false)]
        public string Error
        {
            get
            {
                if (Age < 0) return "ERROR: Negative age";
                if (String.IsNullOrEmpty(Name)) return "ERROR: Missing name";
                return null;
            }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Age":
                        if (Age < 0) return "Negative age";
                        break;
                    case "Name":
                        if (String.IsNullOrEmpty(Name)) return "Missing name";
                        break;
                }
                return null;
            }
        }

        #endregion
    }
}