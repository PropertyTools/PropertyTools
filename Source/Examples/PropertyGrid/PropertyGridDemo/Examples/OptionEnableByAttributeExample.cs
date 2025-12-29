using PropertyTools.DataAnnotations;

namespace ExampleLibrary
{
    using System.ComponentModel;

    [PropertyGridExample]
    public class OptionEnableByAttributeExample : Example
    {
        private TestEnumeration color;
        private bool enableGreenColorOption;

        public enum TestEnumeration
        {
            Red,
            [PropertyTools.DataAnnotations.OptionEnableBy("EnableGreenColorOption")]
            Green,
            Blue
        }

        [Category("Option enable by attribute")]
        [Description("Green color can be enabled or disabled by a 'Enable green color option'")]
        public TestEnumeration Color { get => this.color; set { this.color = value; this.RaisePropertyChanged(nameof(Color)); } }

        public bool EnableGreenColorOption { get => this.enableGreenColorOption; set { this.enableGreenColorOption = value; this.RaisePropertyChanged(nameof(EnableGreenColorOption)); } }
    }
}