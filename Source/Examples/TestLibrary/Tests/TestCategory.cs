namespace TestLibrary
{
    using System.ComponentModel;

    public class TestCategory : TestBase
    {
        [Category("Tab1|Category1")]
        public string Name1 { get; set; }

        [Category("Tab1|Category2")]
        public string Name2 { get; set; }

        [Category("Tab2|Category3")]
        public string Name3 { get; set; }

        public override string ToString()
        {
            return "Tabs and categories";
        }
    }
}