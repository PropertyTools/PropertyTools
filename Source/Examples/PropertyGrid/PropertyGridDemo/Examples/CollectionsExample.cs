// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionsExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using PropertyTools;
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class CollectionsExample : Example
    {
        [Browsable(false)]
        public IEnumerable<Column> StringColumns { get; } = new[]
        {
            new Column("", "String", null, "*", 'L')
        };

        [Category("Arrays|Array")]
        [ColumnsProperty(nameof(StringColumns))]
        [HeaderPlacement(HeaderPlacement.Above)]
        public string[] StringArray1 { get; } = new string[10];

        [HeaderPlacement(HeaderPlacement.Above)]
        public string[] StringArray2 { get; } = new string[9];

        [HeaderPlacement(HeaderPlacement.Above)]
        public int[] IntArray1 { get; } = new int[5];

        [HeaderPlacement(HeaderPlacement.Above)]
        public int[] IntArray2 { get; } = new int[4];

        [HeaderPlacement(HeaderPlacement.Above)]
        public Item[] ItemArray1 { get; set; }

        [Category("Lists|List of items")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public List<Item> List { get; } = new List<Item>();

        [Browsable(false)]
        public IEnumerable<Column> CollectionItemsSourcePropertyColumns { get; } = new[]
        {
            new Column("Name", "Name", null, "2*", 'L', itemsSourcePropertyName: nameof(AvailableNames)),
            new Column("Fraction", "%", "P2", "1*", 'R'),
            new Column("Number", "Number As Hex", null, "1*", 'R')
        };

        [Category("Lists|List of items with ItemsSource at property")]
        [ColumnsProperty(nameof(CollectionItemsSourcePropertyColumns))]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public List<Item> ItemsSourceAtPropertyList { get; set; } = new List<Item>();

        [Browsable(false)]
        public List<string> AvailableNames => new List<string>{"Carl", "Hugo", "Fred"};

        [Category("Lists|List of strings")]
        [ColumnsProperty(nameof(StringColumns))]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public List<string> StringList { get; } = new List<string>();

        [Category("Collection|Collection of items")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public Collection<Item> Collection { get; } = new Collection<Item>();

        [Category("Collection|Collection of strings")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public Collection<string> StringCollection { get; } = new Collection<string>();

        [Browsable(false)]
        public IEnumerable<Column> Collection2Columns { get; } = new[]
        {
            new Column("Name", "Name", null, "2*", 'L'),
            new Column("Fraction", "%", "P2", "1*", 'R'),
            new Column("Number", "Number As Hex", null, "1*", 'R')
        };

        [Category("Custom|Specified columns")]
        [ColumnsProperty(nameof(Collection2Columns))]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public Collection<Item> Collection2 { get; } = new Collection<Item>();

        [Category("Observable|ObservableCollection")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public ObservableCollection<Item> ObservableCollection { get; } = new ObservableCollection<Item>();

        [Browsable(false)]
        public IEnumerable<Column> CityColumns { get; } = new[]
        {
            new Column("", "City")
        };

        [Category("ItemsSource at each item")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        [ListItemItemsSourceProperty("Cities")]
        [ColumnsProperty(nameof(CityColumns))]
        public ObservableCollection<string> ListOfCities { get; } = new ObservableCollection<string>();

        [Browsable(false)]
        public IEnumerable<string> Cities => new[] { "Oslo", "Reykjavik", "New York" };

        public CollectionsExample()
        {
            this.ItemArray1 = new Item[10];
            for (int i = 0; i < this.ItemArray1.Length; i++)
            {
                this.ItemArray1[i] = new Item();
            }
        }
    }

    public class Item : Observable
    {
        private string name;

        private int number;

        private double fraction;

        [Description("Column header ToolTip for 'Name' column")]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.SetValue(ref name, value);
            }
        }

        [Description("Column header ToolTip for 'Number' column")]
        [Converter(typeof(HexConverter))]
        public int Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.SetValue(ref this.number, value);
            }
        }

        [Description("Column header ToolTip for 'Fraction' column")]
        public double Fraction
        {
            get
            {
                return this.fraction;
            }
            set
            {
                this.SetValue(ref this.fraction, value);
            }
        }
    }
}