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
        [ColumnsProperty(nameof(StringColumns))]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public List<string> StringList { get; set; }

        [Category("Lists|Collection")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public Collection<Item> Collection { get; set; }

        [Browsable(false)]
        public IEnumerable<Column> Collection2Columns { get; } = new[]
        {
            new Column("Name", "Name", null, "2*", 'L'),
            new Column("Fraction", "%", "P2", "1*", 'R')
        };

        [Category("Custom|Specified columns")]
        [ColumnsProperty(nameof(Collection2Columns))]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public Collection<Item> Collection2 { get; set; }

        [Category("Observable|ObservableCollection")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public ObservableCollection<Item> ObservableCollection { get; set; }

        [Browsable(false)]
        public IEnumerable<Column> CityColumns { get; } = new[]
        {
            new Column("", "City")
        };

        [Category("ItemsSource at each item")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        [ListItemItemsSourceProperty("Cities")]
        [ColumnsProperty(nameof(CityColumns))]
        public ObservableCollection<string> ListOfCities { get; set; }

        [Browsable(false)]
        public IEnumerable<string> Cities => new[] { "Oslo", "Reykjavik", "New York" };

        public CollectionsExample()
        {
            this.StringArray1 = new string[10];
            this.StringArray2 = new string[9];
            this.IntArray1 = new int[5];
            this.IntArray2 = new int[4];
            
            this.ItemArray1 = new Item[10]; 
            for (int i = 0; i < this.ItemArray1.Length; i++)
            {
                this.ItemArray1[i] = new Item();
            }

            this.List = new List<Item>();
            this.StringList = new List<string>();
            this.Collection = new Collection<Item>();
            this.Collection2 = new Collection<Item>();
            this.ObservableCollection = new ObservableCollection<Item>();
            this.ListOfCities = new ObservableCollection<string>();
        }
    }

    public class Item : Observable
    {
        private string name;

        private int number;

        private double fraction;

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