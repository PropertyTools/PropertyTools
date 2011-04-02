﻿using System;
using System.ComponentModel;
using System.Windows.Media;
using PropertyTools.Wpf;

namespace SimpleDemo
{
    public enum Genders
    {
        Male,
        Female
    };

    public class Person
    {
        // When the [Category(...)] attribute is used with a "|", the
        // first part is the header of the tab and the second part is
        // the header of the category.
        // The order of the properties are not guaranteed to be fixed

        private const string Personal = "General|Personal";
        private const string Favourites = "General|Favourites";
        private const string Habits = "General|Habits";
        private const string Vehicles = "Optional|Vehicles";
        private const string Details = "More|Details";

        public Person()
        {
            Font = new FontFamily("Arial");
        }

        [Category(Personal), SortOrder(100)]
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public int Age { get; set; }
        public double Height { get; set; }
        public Mass Weight { get; set; }
        public Genders Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public Color HairColor { get; set; }

        [FilePath("Image files (*.jpg)|*.jpg|All files (*.*)|*.*", ".jpg")]
        public string Image { get; set; }

        [DirectoryPath]
        public string Folder { get; set; }

        [Category(Favourites), SortOrder(200)]
        public Brush FavouriteBrush { get; set; }

        [Category(Favourites)]
        public FontFamily Font { get; set; }

        // The DisplayName attribute is shown in the property label
        // The Description attribute is shown in the tooltips
        [Category(Habits), SortOrder(300), DisplayName(@"Is smoking"), Description("Check if the person is smoking.")]
        public bool IsSmoking { get; set; }

        [Category(Vehicles), DisplayName(@"Bicyle"), Description("Check if this person owns a bicycle."), SortOrder(401)]
        public bool OwnsBicycle { get; set; }

        // This property is used to control the optional Car property
        // The property should be public, but not browsable
        [Browsable(false)]
        public bool HasCar { get; set; }

        // Car is enabled when HasCar==true
        [Category(Vehicles), Optional("HasCar"), SortOrder(402)]
        public string Car { get; set; }

        // Optional reference types are disabled when they are null
        [Category(Vehicles), Optional, SortOrder(403)]
        public string Scooter { get; set; }

        // Nullable types can be optional
        [Category(Vehicles), Optional, SortOrder(404)]
        public int? Year { get; set; }

        // Use [WideProperty] to use the full width of the control
        // Use [Height(...)] to set the height of a multiline text control
        [Category(Details), WideProperty, Height(100), SortOrder(500)]
        public string History { get; set; }
    }

}