// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestVisibleProperties.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class TestVisibleProperties : TestBase
    {
        [Category("Visibility by convention")]
        public double Weight { get; set; }

        public bool IsWeightVisible { get; set; }

        [Category("Visibility by attribute")]
        [Description("Select green or blue to make the string visible")]
        public TestEnumeration Color2 { get; set; }

        [Browsable(false)]
        public bool IsColor2Ok { get { return this.Color2 == TestEnumeration.Green || this.Color2 == TestEnumeration.Blue; } }

        [VisibleBy("IsColor2Ok")]
        public string String2 { get; set; }

        [Category("Collapsing group")]
        [Description("This group is collapsing when String3 is not visible.")]
        [VisibleBy("IsColor2Ok")]
        public string String3 { get; set; }

        public TestVisibleProperties()
        {
            this.IsWeightVisible = true;
        }

        public override string ToString()
        {
            return "Visibility";
        }
    }
}