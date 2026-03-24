// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimalPersonExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for AnimalPersonExample - demonstrates property shadowing with "new" keyword.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using PropertyTools;

    /// <summary>
    /// Interaction logic for AnimalPersonExample.
    /// </summary>
    public partial class AnimalPersonExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnimalPersonExample" /> class.
        /// </summary>
        public AnimalPersonExample()
        {
            this.InitializeComponent();
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as AnimalPersonExampleViewModel;
            viewModel?.AddAnimal();
        }

        private void AddPerson_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as AnimalPersonExampleViewModel;
            viewModel?.AddPerson();
        }

        private void RemoveSelected_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as AnimalPersonExampleViewModel;
            viewModel?.RemoveSelected();
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as AnimalPersonExampleViewModel;
            viewModel?.ClearAll();
        }

        private void AnimalListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var viewModel = this.DataContext as AnimalPersonExampleViewModel;
            if (viewModel != null)
            {
                viewModel.SelectedAnimals.Clear();
                foreach (var item in this.AnimalListBox.SelectedItems)
                {
                    if (item is AnimalEntity animal)
                    {
                        viewModel.SelectedAnimals.Add(animal);
                    }
                }
            }
        }
    }

    /// <summary>
    /// View model for the Animal and Person example.
    /// </summary>
    public class AnimalPersonExampleViewModel : Observable
    {
        private ObservableCollection<AnimalEntity> animals;
        private ObservableCollection<object> selectedAnimals;
        private int animalCounter = 0;
        private int personCounter = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnimalPersonExampleViewModel" /> class.
        /// </summary>
        public AnimalPersonExampleViewModel()
        {
            this.animals = new ObservableCollection<AnimalEntity>
            {
                new AnimalEntity { Name = "Dog", Species = "Canine", Gender = "Male", Age = 5 },
                new AnimalEntity { Name = "Cat", Species = "Feline", Gender = "Female", Age = 3 },
                new PersonEntity { Name = "Alice", Species = "Human", Gender = Genders.Female, Age = 30, Occupation = "Engineer" },
                new PersonEntity { Name = "Bob", Species = "Human", Gender = Genders.Male, Age = 35, Occupation = "Doctor" }
            };

            this.selectedAnimals = new ObservableCollection<object>();
            this.animalCounter = 2;
            this.personCounter = 2;
        }

        /// <summary>
        /// Gets the collection of animals.
        /// </summary>
        public ObservableCollection<AnimalEntity> Animals
        {
            get => this.animals;
            set => this.SetValue(ref this.animals, value);
        }

        /// <summary>
        /// Gets the collection of selected animals (for PropertyGrid).
        /// </summary>
        public ObservableCollection<object> SelectedAnimals
        {
            get => this.selectedAnimals;
            set => this.SetValue(ref this.selectedAnimals, value);
        }

        /// <summary>
        /// Adds a new Animal to the collection.
        /// </summary>
        public void AddAnimal()
        {
            this.animalCounter++;
            this.Animals.Add(new AnimalEntity
            {
                Name = $"Animal {this.animalCounter}",
                Species = "Generic",
                Gender = "Unknown",
                Age = 1
            });
        }

        /// <summary>
        /// Adds a new Person to the collection.
        /// </summary>
        public void AddPerson()
        {
            this.personCounter++;
            this.Animals.Add(new PersonEntity
            {
                Name = $"Person {this.personCounter}",
                Species = "Human",
                Gender = Genders.Male,
                Age = 25,
                Occupation = "Worker"
            });
        }

        /// <summary>
        /// Removes the selected items from the collection.
        /// </summary>
        public void RemoveSelected()
        {
            var itemsToRemove = this.SelectedAnimals.OfType<AnimalEntity>().ToList();
            foreach (var item in itemsToRemove)
            {
                this.Animals.Remove(item);
            }
            this.SelectedAnimals.Clear();
        }

        /// <summary>
        /// Clears all items from the collection.
        /// </summary>
        public void ClearAll()
        {
            this.Animals.Clear();
            this.SelectedAnimals.Clear();
        }
    }

    /// <summary>
    /// Represents an animal with basic properties.
    /// </summary>
    public class AnimalEntity : Observable
    {
        private string name;
        private string species;
        private string gender;
        private int age;

        /// <summary>
        /// Gets or sets the name of the animal.
        /// </summary>
        [Category("Basic Information")]
        [DisplayName("Name")]
        [Description("The name of the animal")]
        public string Name
        {
            get => this.name;
            set => this.SetValue(ref this.name, value);
        }

        /// <summary>
        /// Gets or sets the species of the animal.
        /// </summary>
        [Category("Basic Information")]
        [DisplayName("Species")]
        [Description("The species of the animal")]
        public string Species
        {
            get => this.species;
            set => this.SetValue(ref this.species, value);
        }

        /// <summary>
        /// Gets or sets the gender of the animal (as a string).
        /// </summary>
        [Category("Details")]
        [DisplayName("Gender (String)")]
        [Description("The gender of the animal as a string value")]
        public string Gender
        {
            get => this.gender;
            set => this.SetValue(ref this.gender, value);
        }

        /// <summary>
        /// Gets or sets the age of the animal.
        /// </summary>
        [Category("Details")]
        [DisplayName("Age")]
        [Description("The age of the animal in years")]
        public int Age
        {
            get => this.age;
            set => this.SetValue(ref this.age, value);
        }

        /// <summary>
        /// Gets the type name for display purposes.
        /// </summary>
        [Browsable(false)]
        public virtual string TypeName => "Animal";
    }

    /// <summary>
    /// Represents a person, which is a specialized animal with a "new" Gender property.
    /// </summary>
    public class PersonEntity : AnimalEntity
    {
        private Genders gender;
        private string occupation;

        /// <summary>
        /// Gets or sets the gender of the person (as an enum, shadowing the base class string property).
        /// This demonstrates the "new" keyword property shadowing scenario.
        /// </summary>
        [Category("Details")]
        [DisplayName("Gender (Enum)")]
        [Description("The gender of the person as an enum value - this property shadows the base Animal.Gender string property")]
        public new Genders Gender
        {
            get => this.gender;
            set => this.SetValue(ref this.gender, value);
        }

        /// <summary>
        /// Gets or sets the occupation of the person.
        /// </summary>
        [Category("Person-Specific")]
        [DisplayName("Occupation")]
        [Description("The occupation or profession of the person")]
        public string Occupation
        {
            get => this.occupation;
            set => this.SetValue(ref this.occupation, value);
        }

        /// <summary>
        /// Gets the type name for display purposes.
        /// </summary>
        [Browsable(false)]
        public override string TypeName => "Person";
    }

    /// <summary>
    /// Gender enumeration for Person class.
    /// </summary>
    public enum Genders
    {
        Male,
        Female,
        Other
    }
}
