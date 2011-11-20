using System.ComponentModel;
using System.Windows.Controls;

namespace ControlsDemo
{
    using System;

    /// <summary>
    /// Interaction logic for FilePickerPage.xaml
    /// </summary>
    public partial class SpinControlPage : Page
    {
        public SpinControlPage()
        {
            InitializeComponent();
            DataContext = new SpinControlViewModel();
        }
    }

    public class SpinControlViewModel : INotifyPropertyChanged
    {
        private double doubleValue;

        public SpinControlViewModel()
        {
            this.DoubleValue = 10;
            DateValue = DateTime.Now;
            MinimumDate = DateTime.Now.AddDays(-1);
            MaximumDate = DateTime.Now.AddDays(7);
        }

        public TimeSpan HalfHourInterval { get { return new TimeSpan(0, 30, 0); } }

        public TimeSpan HourInterval
        {
            get
            {
                return new TimeSpan(1, 0, 0);
            }
        }
        public DateTime MinimumDate { get; set; }
        public DateTime MaximumDate { get; set; }
        public double DoubleValue
        {
            get { return this.doubleValue; }
            set
            {
                this.doubleValue = value;
                RaisePropertyChanged("DoubleValue");
            }
        }

        private int intValue;
        public int IntValue
        {
            get { return this.intValue; }
            set
            {
                this.intValue = value;
                RaisePropertyChanged("IntValue");
            }
        }

        private DateTime dateValue;
        public DateTime DateValue
        {
            get { return this.dateValue; }
            set
            {
                this.dateValue = value;
                RaisePropertyChanged("DateValue");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        protected void RaisePropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}