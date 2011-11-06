namespace TestLibrary
{
    using System;
    using System.ComponentModel;
    using System.Windows.Media;

    using PropertyTools.DataAnnotations;
    using PropertyTools.Wpf;

    public class TestOptionalProperties : TestBase
    {
        public string Name { get; set; }

        [Browsable(false)]
        public bool UseName { get; set; }

        public int Age { get; set; }

        [Browsable(false)]
        public bool UseAge { get; set; }

        [Optional("SpecifyWeight")]
        public double Weight { get; set; }

        [Browsable(false)]
        public bool SpecifyWeight { get; set; }

        [Category("Nullable types")]
        [Optional]
        public int? Number { get; set; }

        [Optional]
        public string String { get; set; }

        [Optional]
        public Color? Color { get; set; }

        [Optional]
        public DateTime? DateTime { get; set; }

        public override string ToString()
        {
            return "Optional properties";
        }
    }
}