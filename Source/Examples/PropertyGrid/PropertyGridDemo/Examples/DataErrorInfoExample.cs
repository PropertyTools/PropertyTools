// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataErrorInfoExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Markup;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class DataErrorInfoExample : Example, System.ComponentModel.IDataErrorInfo
    {
        private string name;
        private int age;
        private bool condensedMilk;
        private bool honey;
        private string country;
        private string name2;
        private Collection<Item> collection1 = new Collection<Item>();

        [AutoUpdateText]
        [Description("Should not be empty.")]
        public string Name { get => this.name; set { this.name = value; this.RaisePropertyChanged(nameof(Name)); } }

        [AutoUpdateText]
        [Description("Should be larger or equal to zero.")]
        public int Age { get => this.age; set { this.age = value; this.RaisePropertyChanged(nameof(Age)); } }

        [DependsOn(nameof(Honey))]
        [Description("You cannot select both.")]
        public bool CondensedMilk { get => this.condensedMilk; set { this.condensedMilk = value; this.RaisePropertyChanged(nameof(CondensedMilk)); } }

        [DependsOn(nameof(CondensedMilk))]
        [Description("You cannot select both.")]
        public bool Honey { get => this.honey; set { this.honey = value; this.RaisePropertyChanged(nameof(Honey)); } }

        [ItemsSourceProperty(nameof(Countries))]
        [Description("Required field.")]
        public string Country { get => this.country; set { this.country = value; this.RaisePropertyChanged(nameof(Country)); } }

        [Category("HeaderPlacement = Above")]
        [AutoUpdateText]
        [Description("Should not be empty.")]
        [HeaderPlacement(HeaderPlacement.Above)]
        public string Name2 { get => this.name2; set { this.name2 = value; this.RaisePropertyChanged(nameof(Name2)); } }

        [Description("This property contains a collection of `Item`s")]
        [HeaderPlacement(HeaderPlacement.Above)]
        public Collection<Item> Collection1 { get => this.collection1; set { this.collection1 = value; this.RaisePropertyChanged(nameof(Collection1)); } }

        [Browsable(false)]
        public IEnumerable<string> Countries => new[] { "Norway", "Sweden", "Denmark", "Finland", string.Empty };

        [Browsable(false)]
        string System.ComponentModel.IDataErrorInfo.this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Name): return string.IsNullOrEmpty(this.Name) ? "The name should be specified." : null;
                    case nameof(Name2): return string.IsNullOrEmpty(this.Name2) ? "The name should be specified." : null;
                    case nameof(Age):
                        {
                            if (this.Age > 130) return "The age is probably too large";
                            if (this.Age < 0) return "The age should not be less than 0.";
                            return null;
                        }
                    case nameof(CondensedMilk):
                    case nameof(Honey):
                        return this.CondensedMilk && this.Honey ? "You cannot have both condensed milk and honey!" : null;
                    case nameof(Country): return string.IsNullOrEmpty(this.Country) ? "The country should be specified." : null;
                    case nameof(Collection1): return this.Collection1.Count == 0 ? "The collection cannot be empty" : null;
                }

                return null;
            }
        }

        [Browsable(false)]
        string System.ComponentModel.IDataErrorInfo.Error
        {
            get
            {
                var o = (System.ComponentModel.IDataErrorInfo)this;
                return o[nameof(this.Name)] ?? o[nameof(this.Age)] ?? o[nameof(this.Honey)] ?? o[nameof(this.Country)];
            }
        }
    }
}