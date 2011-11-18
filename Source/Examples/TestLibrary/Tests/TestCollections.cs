namespace TestLibrary
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using PropertyTools.DataAnnotations;

    public class TestCollections : TestBase
    {
        [Category("Arrays|Array")]
        [Column(0, "", "String", null, "*", 'L')]
        public string[] StringArray { get; set; }
        public string[,] StringArray2 { get; set; }
        public int[] IntArray { get; set; }
        public int[,] IntArray2 { get; set; }
        public Item[] Array { get; set; }

        [Category("Lists|List")]
        public List<Item> List { get; set; }

        [Column(0, "", "String", null, "*", 'L')]
        public List<string> StringList { get; set; }

        [Category("Lists|Collection")]
        public Collection<Item> Collection { get; set; }

        [Category("Custom|Specified columns")]
        [Column(0, "Name", "Name", null, "2*", 'L')]
        [Column(0, "Fraction", "%", "P2", "1*", 'R')]
        public Collection<Item> Collection2 { get; set; }

        [Category("Observable|ObservableCollection")]
        public ObservableCollection<Item> ObservableCollection { get; set; }

        public TestCollections()
        {
            StringArray = new string[10];
            StringArray2 = new string[3, 3];
            IntArray = new int[5];
            IntArray2 = new int[2, 2];
            Array = new Item[10];

            List = new List<Item>();
            StringList = new List<string>();
            Collection = new Collection<Item>();
            Collection2 = new Collection<Item>();
            ObservableCollection = new ObservableCollection<Item>();
        }

        public override string ToString()
        {
            return "Collections";
        }
    }

    public class Item : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public double Fraction { get; set; }

#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67
    }
}