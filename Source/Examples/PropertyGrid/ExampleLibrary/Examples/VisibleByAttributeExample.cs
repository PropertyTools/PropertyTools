// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisibleByAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class VisibleByAttributeExample : Example
    {
        [Category("Visibility by convention")]
        public double Weight { get; set; }

        public bool IsWeightVisible { get; set; }

        [Category("Visibility by attribute")]
        [Description("Select green or blue to make the string visible")]
        public TestEnumeration Color2 { get; set; }

        [Browsable(false)]
        public bool IsColor2Ok => this.Color2 == TestEnumeration.Green || this.Color2 == TestEnumeration.Blue;

        [VisibleBy("IsColor2Ok")]
        public string GreenOrBlue { get; set; }

        [VisibleBy("Color2", TestEnumeration.Green)]
        public string Green { get; set; }

        [Category("Collapsing group")]
        [Description("This group is collapsing when String3 is not visible.")]
        [VisibleBy("IsColor2Ok")]
        public string NotRed { get; set; }

        public VisibleByAttributeExample()
        {
            this.IsWeightVisible = true;
        }

   }
}