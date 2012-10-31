// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Person.cs" company="PropertyTools">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Windows.Media;
using PropertyTools.Wpf;

namespace SimpleDemo
{
    using PropertyTools;
    using PropertyTools.DataAnnotations;

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

        [Category(Personal), SortIndex(100)]
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

        [Category(Favourites), SortIndex(200)]
        public Brush FavouriteBrush { get; set; }

        [Category(Favourites)]
        public FontFamily Font { get; set; }

        // The DisplayName attribute is shown in the property label
        // The Description attribute is shown in the tooltips
        [Category(Habits), SortIndex(300), DisplayName(@"Is smoking"), Description("Check if the person is smoking.")]
        public bool IsSmoking { get; set; }

        [Category(Vehicles), DisplayName(@"Bicyle"), Description("Check if this person owns a bicycle."), SortIndex(401)]
        public bool OwnsBicycle { get; set; }

        // This property is used to control the optional Car property
        // The property should be public, but not browsable
        [Browsable(false)]
        public bool HasCar { get; set; }

        // Car is enabled when HasCar==true
        [Category(Vehicles), Optional("HasCar"), SortIndex(402)]
        public string Car { get; set; }

        // Optional reference types are disabled when they are null
        [Category(Vehicles), Optional, SortIndex(403)]
        public string Scooter { get; set; }

        // Nullable types can be optional
        [Category(Vehicles), Optional, SortIndex(404)]
        public int? Year { get; set; }

        // Use [WideProperty] to use the full width of the control
        // Use [Height(...)] to set the height of a multiline text control
        [Category(Details), WideProperty, Height(100), SortIndex(500)]
        public string History { get; set; }
    }

}