// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StyleApplicationExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Example demonstrating that implicit styles from Style.Resources work correctly with IDataErrorInfo.
    /// This demonstrates the fix for issue #455 where implicit styles were not applied to controls
    /// when PropertyGrid was bound to objects implementing IDataErrorInfo.
    /// </summary>
    [PropertyGridExample]
    public class StyleApplicationExample : Example
    {
        private string name1 = "John";
        private int age1 = 30;
        private string name2 = "Jane";
        private int age2 = 25;

        [Category("Person (without IDataErrorInfo)")]
        [Description("Simple property without validation")]
        public string Name1
        {
            get => this.name1;
            set
            {
                this.name1 = value;
                this.RaisePropertyChanged(nameof(Name1));
            }
        }

        [Category("Person (without IDataErrorInfo)")]
        [Description("Simple property without validation")]
        public int Age1
        {
            get => this.age1;
            set
            {
                this.age1 = value;
                this.RaisePropertyChanged(nameof(Age1));
            }
        }

        [Category("Person2 (with IDataErrorInfo)")]
        [Description("Property with IDataErrorInfo validation")]
        public string Name2
        {
            get => this.name2;
            set
            {
                this.name2 = value;
                this.RaisePropertyChanged(nameof(Name2));
            }
        }

        [Category("Person2 (with IDataErrorInfo)")]
        [Description("Property with IDataErrorInfo validation")]
        public int Age2
        {
            get => this.age2;
            set
            {
                this.age2 = value;
                this.RaisePropertyChanged(nameof(Age2));
            }
        }

        [Browsable(false)]
        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                // Only validate the "2" properties (simulating Person2 with IDataErrorInfo)
                if (columnName == nameof(Name2))
                {
                    return string.IsNullOrEmpty(this.Name2) ? "Name2 should not be empty" : null;
                }

                if (columnName == nameof(Age2))
                {
                    if (this.Age2 < 0) return "Age2 should not be negative";
                    if (this.Age2 > 130) return "Age2 is probably too large";
                }

                return null;
            }
        }

        [Browsable(false)]
        string IDataErrorInfo.Error
        {
            get
            {
                var dei = (IDataErrorInfo)this;
                return dei[nameof(Name2)] ?? dei[nameof(Age2)];
            }
        }
    }
}
