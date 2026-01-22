// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayNameAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class DisplayNameAttributeExample : Example
    {
        private string property1;
        private string property2;
        private string property3;
        private string property4;

        [Category("No attribute")]
        public string Property1 { get => this.property1; set { this.property1 = value; this.RaisePropertyChanged(nameof(Property1)); } }

        [Category("System.ComponentModel")]
        [System.ComponentModel.DisplayName("Customized name (Property2)")]
        public string Property2 { get => this.property2; set { this.property2 = value; this.RaisePropertyChanged(nameof(Property2)); } }

        [Category("PropertyTools.DataAnnotations")]
        [DisplayName("Customized name (Property3)")]
        public string Property3 { get => this.property3; set { this.property3 = value; this.RaisePropertyChanged(nameof(Property3)); } }

        [Category("Derived DisplayNameAttribute")]
        [CustomDisplayName("Property4")]
        public string Property4 { get => this.property4; set { this.property4 = value; this.RaisePropertyChanged(nameof(Property4)); } }

        public class CustomDisplayNameAttribute : DisplayNameAttribute
        {
            public CustomDisplayNameAttribute(string displayName) : base(displayName)
            {
            }

            public override string DisplayName => base.DisplayName.ToUpper();
        }
    }
}