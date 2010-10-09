using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace ControlsDemo.Pages
{
    /// <summary>
    /// Interaction logic for FormattingTextBoxPage.xaml
    /// </summary>
    public partial class FormattingTextBoxPage : Page
    {
        public FormattingTextBoxPage()
        {
            InitializeComponent();
            DataContext = new TestObject();
        }
    }

    public class TestObject : INotifyPropertyChanged
    {
        private string _s;
        private DateTime _time;
        private double _value;

        public TestObject()
        {
            Value = Math.PI;
            Time = DateTime.Now;
            String = "John";
        }

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
            }
        }

        public DateTime Time
        {
            get { return _time; }
            set
            {
                _time = value;
                RaisePropertyChanged("Time");
            }
        }

        public string String
        {
            get { return _s; }
            set
            {
                _s = value;
                RaisePropertyChanged("String");
            }
        }

        #region PropertyChanged Block

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion
    }
}