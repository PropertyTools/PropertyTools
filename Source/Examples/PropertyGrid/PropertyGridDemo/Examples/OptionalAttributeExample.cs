// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionalAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Windows.Media;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class OptionalAttributeExample : Example
    {
        [Category("By convention")]
        [Description("The UseName property specifies if Name is active or not.")]
        public string Name { get; set; }

        [Browsable(false)]
        public bool UseName { get; set; }

        [Description("The UseAge property specifies if Age is active or not.")]
        public int Age { get; set; }

        [Browsable(false)]
        public bool UseAge { get; set; }

        [Category("By OptionalAttribute")]
        [Optional("SpecifyWeight")]
        [Description("The SpecifyWeight property specifies if the Weight is active or not.")]
        public double Weight { get; set; }

        [Browsable(false)]
        public bool SpecifyWeight { get; set; }

        [Optional]
        [Description("The Nullable property is inactive if set to null.")]
        public int? Number { get; set; }

        [Optional]
        [Description("The property is inactive if set to null.")]
        public string String { get; set; }

        [Optional]
        [Description("The property is inactive if set to NaN.")]
        public double Double { get; set; }

        [Optional]
        [Description("The property is inactive if set to null.")]
        public Color? Color { get; set; }

        [Optional]
        [Description("The property is inactive if set to null.")]
        public DateTime? DateTime { get; set; }
    }
}