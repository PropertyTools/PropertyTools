using PropertyTools.DataAnnotations;

namespace ExampleLibrary
{
    using System.ComponentModel;

    [PropertyGridExample]
    public class OptionEnableByAttributeExample : Example
    {
        public enum TestEnumeration
        {
            Red,
            [PropertyTools.DataAnnotations.OptionEnableBy("EnableGreenColorOption")]
            Green,
            Blue
        }

        [Category("Option enable by attribute")]
        [Description("Green color can be enabled or disabled by a 'Enable green color option'")]
        public TestEnumeration Color { get; set; }

        public bool EnableGreenColorOption{ get; set; }
    }
}