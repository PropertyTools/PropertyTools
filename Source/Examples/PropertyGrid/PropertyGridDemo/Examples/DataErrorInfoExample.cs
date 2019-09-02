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
        [AutoUpdateText]
        [Description("Should not be empty.")]
        public string Name { get; set; }

        [AutoUpdateText]
        [Description("Should be larger or equal to zero.")]
        public int Age { get; set; }

        [DependsOn(nameof(Honey))]
        [Description("You cannot select both.")]
        public bool CondensedMilk { get; set; }

        [DependsOn(nameof(CondensedMilk))]
        [Description("You cannot select both.")]
        public bool Honey { get; set; }

        [ItemsSourceProperty(nameof(Countries))]
        [Description("Required field.")]
        public string Country { get; set; }

        [Category("HeaderPlacement = Above")]
        [AutoUpdateText]
        [Description("Should not be empty.")]
        [HeaderPlacement(HeaderPlacement.Above)]
        public string Name2 { get; set; }

        [Description("This property contains a collection of `Item`s")]
        [HeaderPlacement(HeaderPlacement.Above)]
        public Collection<Item> Collection1 { get; set; } = new Collection<Item>();

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