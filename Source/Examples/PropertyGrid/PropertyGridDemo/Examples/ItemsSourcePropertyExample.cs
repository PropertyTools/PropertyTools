// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsSourcePropertyExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides an example using ItemsSourcePropertyAttribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Collections.Generic;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Provides an example using <see cref="ItemsSourcePropertyAttribute" />.
    /// </summary>
    [PropertyGridExample]
    public class ItemsSourcePropertyExample : Example
    {
        private List<string> items;
        private string selectedItem;
        private string editableItem;
        private List<Employee> employees;
        private int selectedEmployeeNumber;
        private int selectedEmployeeNumber2;
        private Employee selectedEmployee;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestItemsSourcePropertyAttribute" /> class.
        /// </summary>
        public ItemsSourcePropertyExample()
        {
            this.Items = new List<string> { "Oslo", "Stockholm", "København" };
            this.SelectedItem = "Stockholm";
            this.EditableItem = "Reykjavik";
            this.Employees = new List<Employee>
                            {
                                new Employee { Name = "Terry Adams", Type = "FTE", EmployeeNumber = 1 },
                                new Employee { Name = "Claire O'Donnell", Type = "FTE", EmployeeNumber = 12345 },
                                new Employee { Name = "Palle Peterson", Type = "FTE", EmployeeNumber = 5678 },
                                new Employee { Name = "Amy E. Alberts", Type = "CSG", EmployeeNumber = 99222 },
                                new Employee { Name = "Stefan Hesse", Type = "FTE", EmployeeNumber = -1 },
                            };
            this.SelectedEmployeeNumber = 12345;
            this.SelectedEmployee = this.Employees[2];
        }

        [Browsable(false)]
        public List<string> Items { get => this.items; set { this.items = value; this.RaisePropertyChanged(nameof(Items)); } }

        [Category("Default SelectedValuePath")]
        [ItemsSourceProperty("Items")]
        public string SelectedItem { get => this.selectedItem; set { this.selectedItem = value; this.RaisePropertyChanged(nameof(SelectedItem)); } }

        [ItemsSourceProperty("Items")]
        [System.ComponentModel.DataAnnotations.Editable(true)]
        public string EditableItem { get => this.editableItem; set { this.editableItem = value; this.RaisePropertyChanged(nameof(EditableItem)); } }

        [Browsable(false)]
        public List<Employee> Employees { get => this.employees; set { this.employees = value; this.RaisePropertyChanged(nameof(Employees)); } }

        [Category("SelectedValuePath")]
        [ItemsSourceProperty("Employees")]
        [SelectedValuePath("EmployeeNumber")]
        public int SelectedEmployeeNumber { get => this.selectedEmployeeNumber; set { this.selectedEmployeeNumber = value; this.RaisePropertyChanged(nameof(SelectedEmployeeNumber)); } }

        [Category("SelectedValuePath / DisplayMemberPath")]
        [ItemsSourceProperty("Employees")]
        [DisplayMemberPath("Name")]
        [SelectedValuePath("EmployeeNumber")]
        public int SelectedEmployeeNumber2 { get => this.selectedEmployeeNumber2; set { this.selectedEmployeeNumber2 = value; this.RaisePropertyChanged(nameof(SelectedEmployeeNumber2)); } }

        [Category("DisplayMemberPath")]
        [ItemsSourceProperty("Employees")]
        [DisplayMemberPath("EmployeeNumber")]
        public Employee SelectedEmployee { get => this.selectedEmployee; set { this.selectedEmployee = value; this.RaisePropertyChanged(nameof(SelectedEmployee)); } }

        public class Employee
        {
            public string Name { get; set; }

            public string Type { get; set; }

            public int EmployeeNumber { get; set; }

            public override string ToString()
            {
                return $"{this.Name} ({this.Type}) #{this.EmployeeNumber}";
            }
        }
    }
}