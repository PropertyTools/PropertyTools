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
        [Description("Turn off 'Show declared only'.")]
        public string Name1 { get; set; }
    }
}