namespace TestLibrary
{
    using System.ComponentModel;

    using PropertyTools.DataAnnotations;

    public class TestFillHeightAttribute : TestBase
    {
        [Category("FillHeight")]
        [FillHeight]
        public string Text { get; set; }

        public override string ToString()
        {
            return "FillHeight";
        }
    }
}