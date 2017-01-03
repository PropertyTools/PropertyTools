// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnabledByAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.ComponentModel;

    [PropertyGridExample]
    public class EnabledByAttributeExample : Example
    {
        [Category("Enable by convention")]
        [Description("The IsAgeEnabled property controls the enable state.")]
        public int Age { get; set; }

        public bool IsAgeEnabled { get; set; }

        [Description("The IsNameEnabled property controls the enable state. This property is also optional.")]
        [PropertyTools.DataAnnotations.Optional]
        public string Name { get; set; }

        public bool IsNameEnabled { get; set; }

        [Category("Enable by attribute")]
        [Description("Select green or blue to enable the string")]
        public TestEnumeration Color { get; set; }

        [PropertyTools.DataAnnotations.EnableBy("IsColorOk")]
        [Description("The IsColorOk property controls the enable state. The property should be enabled when the color is green or blue.")]
        public string EnableByIsColorOkProperty { get; set; }

        [PropertyTools.DataAnnotations.EnableBy("Color", TestEnumeration.Blue)]
        [Description("The control is enabled when Color = Blue.")]
        public string EnableByBlueColor { get; set; }

        [Browsable(false)]
        public bool IsColorOk => this.Color == TestEnumeration.Green || this.Color == TestEnumeration.Blue;
    }
}