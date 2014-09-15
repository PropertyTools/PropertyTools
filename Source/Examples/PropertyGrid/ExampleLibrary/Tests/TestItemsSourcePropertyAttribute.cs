// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestItemsSourcePropertyAttribute.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
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
// <summary>
//   Provides an example using <see cref="ItemsSourcePropertyAttribute" />.
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