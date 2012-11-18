namespace TestLibrary
{
    using System.ComponentModel;

    using PropertyTools.DataAnnotations;

    public class TestHeaderPlacementAttribute : TestBase
    {
        [Category("HeaderPlacement.Above")]
        [HeaderPlacement(HeaderPlacement.Above), Height(100)]
        public string HeaderAbove { get; set; }

        [Category("HeaderPlacement.Collapsed")]
        [HeaderPlacement(HeaderPlacement.Collapsed), Height(100)]
        public string HeaderCollapsed { get; set; }

        [Category("HeaderPlacement.Left")]
        [HeaderPlacement(HeaderPlacement.Left), Height(100)]
        public string HeaderLeft { get; set; }

        public override string ToString()
        {
            return "HeaderPlacement and Height";
        }
    }
}