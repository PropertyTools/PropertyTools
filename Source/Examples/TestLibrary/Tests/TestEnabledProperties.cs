namespace TestLibrary
{
    using System.ComponentModel;

    using PropertyTools.DataAnnotations;

    public class TestEnabledProperties : TestBase
    {
        [Category("Enable by convention")]
        public int Age { get; set; }

        public bool IsAgeEnabled { get; set; }

        [Category("Enable by attribute")]
        [Description("Select green or blue to enable the string")]
        public TestEnumeration Color { get; set; }

        [EnableBy("IsColorOk")]
        public string String { get; set; }

        [Browsable(false)]
        public bool IsColorOk { get { return Color == TestEnumeration.Green || Color == TestEnumeration.Blue; } }

        [Category("Visibility by convention")]
        public double Weight { get; set; }

        public bool IsWeightVisible { get; set; }

        [Category("Visibility by attribute")]
        [Description("Select green or blue to make the string visible")]
        public TestEnumeration Color2 { get; set; }

        [Browsable(false)]
        public bool IsColor2Ok { get { return Color2 == TestEnumeration.Green || Color2 == TestEnumeration.Blue; } }
        
        [VisibleBy("IsColor2Ok")]
        public string String2 { get; set; }

        public TestEnabledProperties()
        {
            IsWeightVisible = true;
        }

        public override string ToString()
        {
            return "Enabled/Visible properties";
        }
    }
}