// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestItemsSourcePropertyAttribute.cs" company="PropertyTools">
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
    public class TestItemsSourcePropertyAttribute : TestBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestItemsSourcePropertyAttribute" /> class.
        /// </summary>
        public TestItemsSourcePropertyAttribute()
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
            this.SelectedEmployee = 12345;
        }

        [Browsable(false)]
        public List<string> Items { get; set; }

        [Category("Default SelectedValuePath")]
        [ItemsSourceProperty("Items")]
        public string SelectedItem { get; set; }

        [ItemsSourceProperty("Items")]
        [System.ComponentModel.DataAnnotations.Editable(true)]
        public string EditableItem { get; set; }

        [Browsable(false)]
        public List<Employee> Employees { get; set; }

        [Category("SelectedValuePath = \"EmployeeNumber\"")]
        [ItemsSourceProperty("Employees")]
        [SelectedValuePath("EmployeeNumber")]
        public int SelectedEmployee { get; set; }

        public override string ToString()
        {
            return "ItemsSourceProperty";
        }

        public class Employee
        {
            public string Name { get; set; }

            public string Type { get; set; }

            public int EmployeeNumber { get; set; }

            public override string ToString()
            {
                return string.Format("{0} ({1})", this.Name, this.Type);
            }
        }
    }
}