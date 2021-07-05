// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SizeConfigurationDemo
{
    using System.Windows.Media;

    public class MainWindowViewModel
    {
        public Person SelectedPerson { get; set; }

        public MainWindowViewModel()
        {
            this.SelectedPerson = new Person
                {
                    FirstName = "Anders",
                    LastName = "Andersen",
                    Age = 29,
                    Gender = Genders.Male,
                    Height = 1.82,
                    HairColor = Colors.Brown
                };
        }
    }
}