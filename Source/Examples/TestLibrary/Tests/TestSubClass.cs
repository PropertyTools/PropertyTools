namespace TestLibrary
{
    using System.ComponentModel;

    public class TestSubClass : SuperClass
    {
        public string Name2 { get; set; }

        public override string ToString()
        {
            return "Sub-class";
        }
    }

    public class SuperClass : TestBase
    {
        [Description("Check 'Show declared only' on the 'PropertyControl' menu.")]
        [Category("SuperClass")]
        public string Name1 { get; set; }
    }
}