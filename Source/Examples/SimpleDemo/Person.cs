using System;
using System.ComponentModel;
using System.Windows.Media;
using PropertyEditorLibrary;

namespace SimpleDemo
{
    public enum Genders
    {
        Male,
        Female
    }

    public class Person
    {
        // When the [Category(...)] attribute is used with a "|", the
        // first part is the header of the tab and the second part is
        // the header of the category
        // The order of the properties are not guaranteed to be the same 
        // as in the code, but so far I have not seen any difference...

        private const string catPersonal = "General|Personal";
        private const string catFavourites = "General|Favourites";
        private const string catHabits = "General|Habits";
        private const string catVehicles = "Optional|Vehicles";
        private const string catDetails = "More|Details";

        public Person()
        {
            Font = new FontFamily("Arial");
        }

        [Category(catPersonal), SortOrder(100)]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }
        
        public double Height { get; set; }
        
        public Mass Weight { get; set; }
        
        public Genders Gender { get; set; }
        
        // DateTime is showing a TextBox - see the CustomEditorDemo for usage with the Date picker from WPF toolkit.
        public DateTime BirthDate { get; set; }
        
        public Color HairColor { get; set; }

        [FilePath("Image files (*.jpg)|*.jpg|All files (*.*)|*.*", ".jpg")]
        public string Image { get; set; }

        [DirectoryPath]
        public string Folder { get; set; }

        [Category(catFavourites), SortOrder(200)]
        public Brush FavouriteBrush { get; set; }

        [Category(catFavourites)]
        public FontFamily Font { get; set; }

        // The DisplayName attribute is shown in the property label
        // The Description attribute is shown in the tooltips
        [Category(catHabits), SortOrder(300), DisplayName("Is smoking"), Description("Check if the person is smoking.")]
        public bool IsSmoking { get; set; }

        [Category(catVehicles), DisplayName("Bicyle"), Description("Check if this person owns a bicycle."), SortOrder(401)]
        public bool OwnsBicycle { get; set; }

        // This property is used to control the optional Car property
        // The property should be public, but not browsable
        [Browsable(false)]
        public bool HasCar { get; set; }

        // Car is enabled when HasCar==true
        [Category(catVehicles), Optional("HasCar"), SortOrder(402)]
        public string Car { get; set; }

        // Optional reference types are disabled when they are null
        [Category(catVehicles), Optional, SortOrder(403)]
        public string Scooter { get; set; }

        // Nullable types can be optional, the property is disabled when the value is null
        [Category(catVehicles), Optional, SortOrder(404)]
        public int? Year { get; set; }

        // Use [WideProperty] to use the full width of the control
        // Use [Height(...)] to set the height of a multiline text control
        [Category(catDetails), WideProperty, Height(100), SortOrder(500)]
        public string History { get; set; }
    }

}