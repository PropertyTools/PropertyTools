namespace TestLibrary
{
    public class TestReadOnlyProperties : TestBase
    {
        public string Name { get; private set; }

        public TestReadOnlyProperties()
        {
            Name = "John";
        }

        public override string ToString()
        {
            return "Read-only properties";
        }
    }
}