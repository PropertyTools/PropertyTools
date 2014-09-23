// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace SimpleDemo
{
    using System.Windows.Media;

    public class MainWindowViewModel
    {
        public Person SelectedPerson { get; set; }

        public MainWindowViewModel()
        {
            this.SelectedPerson = new Person()
                {
                    FirstName = "Anders",
                    LastName = "Andersen",
                    Age = 29,
                    Gender = Genders.Male,
                    Weight = new Mass { Value = 70 },
                    Height = 1.82,
                    HairColor = Colors.Brown
                };
        }
    }
}