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
        [Category("No attribute")]
        public string Property1 { get; set; }

        [Category("System.ComponentModel")]
        [System.ComponentModel.Description("Customized description (Property2)")]
        public string Property2 { get; set; }

        [Category("PropertyTools.DataAnnotations")]
        [Description("Customized description (Property3)")]
        public string Property3 { get; set; }
    }
}