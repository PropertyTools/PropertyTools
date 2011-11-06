namespace TestLibrary
{
    public class TestAutomaticDisplayNames : TestBase
    {
        public string FirstName { get; set; }
        public string PositionX { get; set; }
        public string PositionXYZ { get; set; }
        public string XYZ { get; set; }

        public override string ToString()
        {
            return "Automatic display names";
        }
    }
}