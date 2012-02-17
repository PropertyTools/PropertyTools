namespace TestLibrary
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public class TestDictionary : TestBase
    {
        [Description("This is not yet working.")]
        public Dictionary<int, Item> Dictionary { get; set; }

        public TestDictionary()
        {
            Dictionary = new Dictionary<int, Item>();
        }

        public override string ToString()
        {
            return "Dictionary";
        }
    }
}