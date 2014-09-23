// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDataErrorInfo.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Collections.Generic;
    using System.Windows.Markup;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class TestDataErrorInfo : TestBase, System.ComponentModel.IDataErrorInfo
    {
        [AutoUpdateText]
        [Description("Should not be empty.")]
        public string Name { get; set; }

        [AutoUpdateText]
        [Description("Should be larger or equal to zero.")]
        public int Age { get; set; }

        [DependsOn("Honey")]
        [Description("You cannot select both.")]
        public bool CondensedMilk { get; set; }

        [DependsOn("CondensedMilk")]
        [Description("You cannot select both.")]
        public bool Honey { get; set; }

        [ItemsSourceProperty("Countries")]
        [Description("Required field.")]
        public string Country { get; set; }

        [Browsable(false)]
        public IEnumerable<string> Countries
        {
            get
            {
                return new[] { "Norway", "Sweden", "Denmark", "Finland", string.Empty };
            }
        }

        [Browsable(false)]
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name": return string.IsNullOrEmpty(Name) ? "The name should be specified." : null;
                    case "Age":
                        {
                            if (Age > 130) return "The age is probably too large";
                            if (Age < 0) return "The age should not be less than 0.";
                            return null;
                        }
                    case "CondensedMilk":
                    case "Honey":
                        return this.CondensedMilk && this.Honey ? "You cannot have both condensed milk and honey!" : null;
                    case "Country": return string.IsNullOrEmpty(Country) ? "The country should be specified." : null;
                }

                return null;
            }
        }

        [Browsable(false)]
        string System.ComponentModel.IDataErrorInfo.Error
        {
            get
            {
                return this["Name"] ?? this["Age"] ?? this["Honey"] ?? this["Country"];
            }
        }

        public TestDataErrorInfo()
        {
            this.Name = "Mike";
            this.Age = 3;
            this.Country = "Norway";
        }

        public override string ToString()
        {
            return "Validation with IDataErrorInfo";
        }
    }
}