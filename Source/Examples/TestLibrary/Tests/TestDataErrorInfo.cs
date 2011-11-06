namespace TestLibrary
{
    using System.ComponentModel;

    using PropertyTools.DataAnnotations;

    public class TestDataErrorInfo : TestBase, IDataErrorInfo
    {
        [AutoUpdateText]
        [Description("Should not be empty.")]
        public string Name { get; set; }

        [AutoUpdateText]
        [Description("Should be larger or equal tozero.")]
        public int Age { get; set; }

        [Browsable(false)]
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name": return string.IsNullOrEmpty(Name) ? "Name is empty." : null;
                    case "Age": return Age < 0 ? "Age less than 0." : null;
                }
                return null;
            }
        }

        [Browsable(false)]
        public string Error
        {
            get
            {
                if (Name == "") return "Name is empty!";
                if (Age < 0) return "Age is less than 0!";
                return null;
            }
        }

        public TestDataErrorInfo()
        {
            Name = "Mike";
            Age = 3;
        }

        public override string ToString()
        {
            return "IDataErrorInfo";
        }
    }
}