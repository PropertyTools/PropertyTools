// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectorExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for SelectorExample.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using PropertyTools;
    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Interaction logic for SelectorExample.xaml
    /// </summary>
    public partial class SelectorExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorExample" /> class.
        /// </summary>
        public SelectorExample()
        {
            this.InitializeComponent();
            this.DataContext = new ViewModel();
        }

        public class ViewModel : Observable
        {
            public ViewModel()
            {
                var item = new Item();
                item.City1 = item.Cities1.First();
                item.City2 = item.Cities2.Last().Name;
                item.City3 = item.Cities2.First();
                this.Items.Add(item);
            }
            public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();

            public class Item : Observable
            {
                private string city1;

                private string city2;

                private City city3;

                [ItemsSourceProperty(nameof(Cities1))]
                public string City1
                {
                    get
                    {
                        return this.city1;
                    }

                    set
                    {
                        this.SetValue(ref this.city1, value, () => this.City1);
                    }
                }

                public IEnumerable<string> Cities1 => new[] { "Oslo", "Stockholm", "Copenhagen" };

                [ItemsSourceProperty(nameof(Cities2))]
                [SelectedValuePath(nameof(City.Name))]
                public string City2
                {
                    get
                    {
                        return this.city2;
                    }

                    set
                    {
                        this.SetValue(ref this.city2, value, () => this.City2);
                    }
                }

                [ItemsSourceProperty(nameof(Cities2))]
                public City City3
                {
                    get
                    {
                        return this.city3;
                    }

                    set
                    {
                        this.SetValue(ref this.city3, value, () => this.City3);
                    }
                }

                public IEnumerable<City> Cities2
                    =>
                        new[]
                            {
                                new City { Name = "Oslo" }, new City { Name = "Stockholm" },
                                new City { Name = "Copenhagen" }
                            };

                public class City
                {
                    public string Name { get; set; }

                    public override string ToString()
                    {
                        return this.Name;
                    }
                }
            }
        }
    }
}