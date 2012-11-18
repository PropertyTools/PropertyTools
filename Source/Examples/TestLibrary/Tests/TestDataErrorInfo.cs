namespace TestLibrary
{
    using System.ComponentModel;
    using System.Windows.Markup;

    using PropertyTools.DataAnnotations;

    public class TestDataErrorInfo : TestBase, IDataErrorInfo
    {
        [AutoUpdateText]
        [Description("Should not be empty.")]
        public string Name { get; set; }

        [AutoUpdateText]
        [Description("Should be larger or equal to zero.")]
        public int Age { get; set; }

        [DependsOn("Honey")]
        public bool CondensedMilk { get; set; }

        [DependsOn("CondensedMilk")]
        public bool Honey { get; set; }

        [Browsable(false)]
        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Name": return string.IsNullOrEmpty(Name) ? "Name is empty." : null;
                    case "Age": return Age < 0 ? "Age less than 0." : null;
                    case "CondensedMilk":
                    case "Honey":
                        return CondensedMilk && Honey ? "You cannot have both condensed milk and honey!" : null;
                }
                return null;
            }
        }

        [Browsable(false)]
        string IDataErrorInfo.Error
        {
            get
            {
                if (Name == "") return "Name is empty!";
                if (Age < 0) return "Age is less than 0!";
                if (CondensedMilk && Honey) return "You cannot have both condensed milk and honey!";
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