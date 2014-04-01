namespace TestLibrary
{
    using System.ComponentModel;

    using PropertyTools.DataAnnotations;

    public class TestFillTabAttribute : TestBase
    {
        [Category("Header|Group category is not shown!")]
        [FillTab]
        public string Text { get; set; }

        [Category("No header|Group category is not shown!")]
        [FillTab]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public string Text2 { get; set; }

        public override string ToString()
        {
            return "FillTab";
        }
    }
}