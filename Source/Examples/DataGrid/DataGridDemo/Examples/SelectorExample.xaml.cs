// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectorExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for SelectorExample.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

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
                item.Fruit1 = item.Fruits.First();
                item.Fruit2 = "Kiwi";
                item.City1 = item.Cities.First();
                item.City2 = item.Cities.Last().Name;
                item.City3 = item.Cities.First();
                item.City4 = item.Cities.First().Name;
                item.SelectedAnimal = item.Animals.First();
                this.Items.Add(item);
            }

            public ObservableCollection<Item> Items { get; } = new ObservableCollection<Item>();

            public class Item : Observable
            {
                private string fruit1;
                private string fruit2;

                private City city1;
                private string city2;
                private City city3;
                private string city4;

                private Animal selectedAnimal;

                [ItemsSourceProperty(nameof(Fruits))]
                public string Fruit1
                {
                    get
                    {
                        return this.fruit1;
                    }

                    set
                    {
                        this.SetValue(ref this.fruit1, value);
                    }
                }

                [ItemsSourceProperty(nameof(Fruits))]
                [Editable(true)]
                public string Fruit2
                {
                    get
                    {
                        return this.fruit2;
                    }

                    set
                    {
                        this.SetValue(ref this.fruit2, value);
                    }
                }

                public IEnumerable<string> Fruits { get; } = new[] { "Apple", "Banana", "Orange" };

                [ItemsSourceProperty(nameof(Cities))]
                public City City1
                {
                    get
                    {
                        return this.city1;
                    }

                    set
                    {
                        this.SetValue(ref this.city1, value);
                    }
                }

                [ItemsSourceProperty(nameof(Cities))]
                [SelectedValuePath(nameof(City.Name))]
                public string City2
                {
                    get
                    {
                        return this.city2;
                    }

                    set
                    {
                        this.SetValue(ref this.city2, value);
                    }
                }

                [ItemsSourceProperty(nameof(Cities))]
                [DisplayMemberPath(nameof(City.Name))]
                public City City3
                {
                    get
                    {
                        return this.city3;
                    }

                    set
                    {
                        this.SetValue(ref this.city3, value);
                    }
                }

                [ItemsSourceProperty(nameof(Cities))]
                [SelectedValuePath(nameof(City.Name))]
                [DisplayMemberPath(nameof(City.Name))]
                public string City4
                {
                    get
                    {
                        return this.city4;
                    }

                    set
                    {
                        this.SetValue(ref this.city4, value);
                    }
                }

                public IEnumerable<City> Cities { get; } = CreateCities().ToArray();

                [ItemsSourceProperty(nameof(Animals))]
                [DisplayMemberPath(nameof(Animal.Name))]
                public Animal SelectedAnimal
                {
                    get
                    {
                        return this.selectedAnimal;
                    }

                    set
                    {
                        this.SetValue(ref this.selectedAnimal, value);
                    }
                }

                public IEnumerable<Animal> Animals => CreateAnimals(); // create a new sequence every time... The element class must implement Equals.

                private static IEnumerable<City> CreateCities()
                {
                    yield return new City { Name = "Oslo", Country = "Norway" };
                    yield return new City { Name = "Stockholm", Country = "Sweden" };
                    yield return new City { Name = "Copenhagen", Country = "Denmark" };
                }

                private static IEnumerable<Animal> CreateAnimals()
                {
                    yield return new Animal { Name = "Bear" };
                    yield return new Animal { Name = "Wolf" };
                    yield return new Animal { Name = "Moose" };
                }

                public class City
                {
                    public string Name { get; set; }
                    public string Country { get; set; }
                    public override string ToString()
                    {
                        return $"{this.Name}, {this.Country}";
                    }
                }

                public class Animal
                {
                    public string Name { get; set; }

                    public override bool Equals(object obj)
                    {
                        return string.Equals(this.Name, (obj as Animal)?.Name);
                    }

                    public override int GetHashCode()
                    {
                        return this.Name.GetHashCode();
                    }
                }
            }
        }
    }
}