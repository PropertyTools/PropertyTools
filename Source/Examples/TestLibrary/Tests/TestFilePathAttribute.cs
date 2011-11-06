namespace TestLibrary
{
    using System.IO;

    using PropertyTools.DataAnnotations;

    public class TestFilePathAttribute : TestBase
    {
        [FilePath(".txt", true)]
        [FilterProperty("Filter")]
        public string OpenFilePath { get; set; }

        [FilePath(".txt", false)]
        public string SaveFilePath { get; set; }

        [FilePath(".txt", true)]
        [BasePathProperty("BasePath")]
        public string RelativePath { get; set; }

        public string BasePath { get; private set; }

        public string Filter { get; private set; }

        public TestFilePathAttribute()
        {
            BasePath = Directory.GetCurrentDirectory();
            Filter= "All files (*.*)|*.*";
        }

        public override string ToString()
        {
            return "FilePath attribute";
        }
    }
}