// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Model.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ItemsBagDemo
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;

    using PropertyTools.DataAnnotations;

    public enum Colors { Red, Green, Blue }

    public class Model : INotifyPropertyChanged
    {
        public bool ShowColor => IsChecked;

        private bool isChecked;

        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    RaisePropertyChanged(nameof(IsChecked));
                    RaisePropertyChanged(nameof(ShowColor));
                }
            }
        }

        private string name;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value; RaisePropertyChanged("Name");
            }
        }

        private int? value;

        [Required]
        public int? Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value; RaisePropertyChanged("Value");
            }
        }

        private Colors color;

        [VisibleBy(nameof(ShowColor))]
        public Colors Color
        {
            get
            {
                return this.color;
            }
            set
            {
                this.color = value;
                RaisePropertyChanged("Color");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            Debug.WriteLine("Model.RaisePropertyChanged on " + property);

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}