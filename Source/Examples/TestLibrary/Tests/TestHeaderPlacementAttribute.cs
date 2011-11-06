namespace TestLibrary
{
    using PropertyTools.DataAnnotations;

    public class TestHeaderPlacementAttribute : TestBase
    {
        [HeaderPlacement(HeaderPlacement.Above), Height(100)]
        public string Comment { get; set; }

        public override string ToString()
        {
            return "HeaderPlacement and Height";
        }
    }
}