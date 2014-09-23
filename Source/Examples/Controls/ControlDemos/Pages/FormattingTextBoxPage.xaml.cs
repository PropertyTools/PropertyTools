// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormattingTextBoxPage.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for FormattingTextBoxPage.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControlDemos.Pages
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for FormattingTextBoxPage.xaml
    /// </summary>
    public partial class FormattingTextBoxPage
    {
        public FormattingTextBoxPage()
        {
            this.InitializeComponent();
            this.DataContext = new TestObject();
        }
    }

    public class TestObject : INotifyPropertyChanged
    {
        private string name;
        private DateTime time;
        private double number;

        public TestObject()
        {
            this.Number = Math.PI;
            this.Time = DateTime.Now;
            this.Name = "John";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double Number
        {
            get
            {
                return this.number;
            }

            set
            {
                this.number = value;
                this.RaisePropertyChanged("Number");
            }
        }

        public DateTime Time
        {
            get
            {
                return this.time;
            }

            set
            {
                this.time = value;
                this.RaisePropertyChanged("Time");
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                this.RaisePropertyChanged("Name");
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