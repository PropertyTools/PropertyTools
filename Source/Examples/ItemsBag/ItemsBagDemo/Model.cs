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

    /// <summary>
    /// Generic value type struct example
    /// </summary>
    public struct R<T>
    {
        public T Value { get; set; }

        public R(T value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "null";
        }
    }

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

        private double? nullableDouble;

        [Category("Nullable Types")]
        [Description("Nullable double value")]
        public double? NullableDouble
        {
            get
            {
                return this.nullableDouble;
            }
            set
            {
                this.nullableDouble = value;
                RaisePropertyChanged(nameof(NullableDouble));
            }
        }

        private Colors? nullableColor;

        [Category("Nullable Types")]
        [Description("Nullable enum value")]
        public Colors? NullableColor
        {
            get
            {
                return this.nullableColor;
            }
            set
            {
                this.nullableColor = value;
                RaisePropertyChanged(nameof(NullableColor));
            }
        }

        private R<int> genericInt;

        [Category("Generic Value Types")]
        [Description("Generic struct with int")]
        public R<int> GenericInt
        {
            get
            {
                return this.genericInt;
            }
            set
            {
                this.genericInt = value;
                RaisePropertyChanged(nameof(GenericInt));
            }
        }

        private R<string> genericString;

        [Category("Generic Value Types")]
        [Description("Generic struct with string")]
        public R<string> GenericString
        {
            get
            {
                return this.genericString;
            }
            set
            {
                this.genericString = value;
                RaisePropertyChanged(nameof(GenericString));
            }
        }

        private R<double>? nullableGenericDouble;

        [Category("Generic Value Types")]
        [Description("Nullable generic struct with double")]
        public R<double>? NullableGenericDouble
        {
            get
            {
                return this.nullableGenericDouble;
            }
            set
            {
                this.nullableGenericDouble = value;
                RaisePropertyChanged(nameof(NullableGenericDouble));
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