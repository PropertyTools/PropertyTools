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
        [HeaderPlacement(HeaderPlacement.Above)]
        public string[] StringArray1 { get; set; }

        [HeaderPlacement(HeaderPlacement.Above)]
        public string[] StringArray2 { get; set; }

        [HeaderPlacement(HeaderPlacement.Above)]
        public int[] IntArray1 { get; set; }

        [HeaderPlacement(HeaderPlacement.Above)]
        public int[] IntArray2 { get; set; }

        [HeaderPlacement(HeaderPlacement.Above)]
        public Item[] ItemArray1 { get; set; }

        [Category("Lists|List of items")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public List<Item> List { get; set; }

        [Category("Lists|List of strings")]
        [Column(0, "", "String", null, "*", 'L')]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public List<string> StringList { get; set; }

        [Category("Lists|Collection")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public Collection<Item> Collection { get; set; }

        [Category("Custom|Specified columns")]
        [Column(0, "Name", "Name", null, "2*", 'L')]
        [Column(1, "Fraction", "%", "P2", "1*", 'R')]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public Collection<Item> Collection2 { get; set; }

        [Category("Observable|ObservableCollection")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public ObservableCollection<Item> ObservableCollection { get; set; }

        public TestCollections()
        {
            StringArray1 = new string[10];
            StringArray2 = new string[9];
            IntArray1 = new int[5];
            IntArray2 = new int[4];
            ItemArray1 = new Item[10];

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