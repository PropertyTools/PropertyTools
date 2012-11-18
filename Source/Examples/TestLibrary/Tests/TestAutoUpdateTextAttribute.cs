namespace TestLibrary
{
    using PropertyTools.DataAnnotations;

    public class TestAutoUpdateTextAttribute : TestBase
    {
        [AutoUpdateText]
        public string Comment { get; set; }

        public override string ToString()
        {
            return "AutoUpdateText";
        }
    }
}