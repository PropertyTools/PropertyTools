// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpinControlPage.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for FilePickerPageSpinControlPage.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControlDemos
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for SpinControlPage.xaml
    /// </summary>
    public partial class SpinControlPage
    {
        public SpinControlPage()
        {
            this.InitializeComponent();
            this.DataContext = new SpinControlViewModel();
        }
    }

    public class SpinControlViewModel : INotifyPropertyChanged
    {
        private double doubleValue;
        private int intValue;
        private DateTime dateValue;

        public SpinControlViewModel()
        {
            this.DoubleValue = 10;
            this.DateValue = DateTime.Now;
            this.MinimumDate = DateTime.Now.AddDays(-1);
            this.MaximumDate = DateTime.Now.AddDays(7);
        }

        public event PropertyChangedEventHandler PropertyChanged;

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
            get
            {
                return this.doubleValue;
            }

            set
            {
                this.doubleValue = value;
                this.RaisePropertyChanged("DoubleValue");
            }
        }

        public int IntValue
        {
            get
            {
                return this.intValue;
            }

            set
            {
                this.intValue = value;
                this.RaisePropertyChanged("IntValue");
            }
        }

        public DateTime DateValue
        {
            get { return this.dateValue; }
            set
            {
                this.dateValue = value;
                this.RaisePropertyChanged("DateValue");
            }
        }

        protected void RaisePropertyChanged(string property)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}