namespace TestLibrary
{
    using System.ComponentModel;

    public class TestDisplayName : TestBase
    {
        [DisplayName("Display name"), Description("Property description.")]
        public string Name { get; set; }

        public override string ToString()
        {
            return "Display name and description";
        }
    }
}