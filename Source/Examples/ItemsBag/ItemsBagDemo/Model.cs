// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Model.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ItemsBagDemo
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics;

    using PropertyTools.DataAnnotations;
    using Category = System.ComponentModel.CategoryAttribute;
    using Description = System.ComponentModel.DescriptionAttribute;

    public enum Colors { Red, Green, Blue }

    /// <summary>
    /// Generic value type struct example with string conversion support.
    /// </summary>
    /// <typeparam name="T">The type of value stored in the struct.</typeparam>
    [TypeConverter(typeof(RTypeConverter))]
    public struct R<T>
    {
        public T Value { get; set; }

        public R(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Implicit conversion from T to R&lt;T&gt;.
        /// Allows assignment like: R&lt;int&gt; x = 3;
        /// </summary>
        public static implicit operator R<T>(T value)
        {
            return new R<T>(value);
        }

        /// <summary>
        /// Implicit conversion from R&lt;T&gt; to T.
        /// Allows retrieval like: int x = myR;
        /// </summary>
        public static implicit operator T(R<T> r)
        {
            return r.Value;
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "null";
        }

        public override bool Equals(object obj)
        {
            if (obj is R<T> other)
            {
                return object.Equals(Value, other.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value?.GetHashCode() ?? 0;
        }
    }

    /// <summary>
    /// TypeConverter for R&lt;T&gt; that supports conversion to/from string.
    /// </summary>
    public class RTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string str)
            {
                try
                {
                    // Get the generic type argument T from the target type
                    var targetType = context?.PropertyDescriptor?.PropertyType;
                    if (targetType != null && targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(R<>))
                    {
                        var innerType = targetType.GetGenericArguments()[0];
                        
                        // Use TypeDescriptor to convert the string to the inner type
                        var converter = TypeDescriptor.GetConverter(innerType);
                        if (converter != null && converter.CanConvertFrom(typeof(string)))
                        {
                            var innerValue = converter.ConvertFromString(str);
                            
                            // Create R<T> instance using reflection since we don't know T at compile time
                            // This constructs the appropriate R<T> type (e.g., R<int>, R<double>) dynamically
                            var rType = typeof(R<>).MakeGenericType(innerType);
                            return Activator.CreateInstance(rType, innerValue);
                        }
                    }
                }
                catch (Exception ex) when (ex is FormatException || ex is InvalidCastException || ex is NotSupportedException)
                {
                    // Let the base converter handle invalid format errors
                    throw new FormatException($"Cannot convert '{str}' to the target type.", ex);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value != null)
            {
                // Use the ToString method of R<T>
                return value.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
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