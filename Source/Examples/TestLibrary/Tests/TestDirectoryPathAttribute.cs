namespace TestLibrary
{
    using PropertyTools.DataAnnotations;

    public class TestDirectoryPathAttribute : TestBase
    {
        [DirectoryPath]
        public string DirectoryPath { get; set; }

        public override string ToString()
        {
            return "DirectoryPath attribute";
        }
    }
}