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
        public bool IsColorOk
        {
            get
            {
                return Color == TestEnumeration.Green || Color == TestEnumeration.Blue;
            }
        }

        public override string ToString()
        {
            return "Enabled properties";
        }
    }
}