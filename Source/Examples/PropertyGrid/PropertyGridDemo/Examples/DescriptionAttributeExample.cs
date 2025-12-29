// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DescriptionAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class DescriptionAttributeExample : Example
    {
        private string property1;
        private string property2;
        private string property3;

        [Category("No attribute")]
        public string Property1 { get => this.property1; set { this.property1 = value; this.RaisePropertyChanged(nameof(Property1)); } }

        [Category("System.ComponentModel")]
        [System.ComponentModel.Description("Customized description (Property2)")]
        public string Property2 { get => this.property2; set { this.property2 = value; this.RaisePropertyChanged(nameof(Property2)); } }

        [Category("PropertyTools.DataAnnotations")]
        [Description("Customized description (Property3)")]
        public string Property3 { get => this.property3; set { this.property3 = value; this.RaisePropertyChanged(nameof(Property3)); } }
    }
}