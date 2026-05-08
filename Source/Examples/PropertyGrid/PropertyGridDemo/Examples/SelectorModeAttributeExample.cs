// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectorModeAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2026 PropertyTools contributors
// </copyright>
// <summary>
//   Provides an example using SelectorMode attribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Collections.Generic;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class SelectorModeAttributeExample : Example
    {
        private List<Employee> employees;
        private int selectedEmployeeNumber3;
        private List<int> selectedEmployeeNumbers;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorModeAttributeExample" /> class.
        /// </summary>
        public SelectorModeAttributeExample()
        {
            this.Employees = new List<Employee>
                            {
                                new Employee { Name = "Terry Adams", Type = "FTE", EmployeeNumber = 1 },
                                new Employee { Name = "Claire O'Donnell", Type = "FTE", EmployeeNumber = 12345 },
                                new Employee { Name = "Palle Peterson", Type = "FTE", EmployeeNumber = 5678 },
                                new Employee { Name = "Amy E. Alberts", Type = "CSG", EmployeeNumber = 99222 },
                                new Employee { Name = "Stefan Hesse", Type = "FTE", EmployeeNumber = -1 },
                            };            
            this.SelectedEmployeeNumbersL = new List<int> { 12345 };
        }

        [Browsable(false)]
        public List<Employee> Employees { get => this.employees; set { this.employees = value; this.RaisePropertyChanged(nameof(Employees)); } }


        // tab Single

        [Category("SelectorMode=Single|SelectedValuePath / DisplayMemberPath")]
        [ItemsSourceProperty("Employees")]
        [DisplayMemberPath("Name")]
        [SelectedValuePath("EmployeeNumber")]
        // no SelectorMode attribute
        public int SelectedEmployeeNumber3
        {
            get => this.selectedEmployeeNumber3;
            set
            {
                this.selectedEmployeeNumber3 = value;
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3));
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3C));
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3R));
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3L));
            }
        }

        [Category("SelectorMode=Single|SelectedValuePath / DisplayMemberPath")]
        [ItemsSourceProperty("Employees")]
        [DisplayMemberPath("Name")]
        [SelectedValuePath("EmployeeNumber")]
        [SelectorMode(SelectorMode.Single)]
        public int SelectedEmployeeNumber3C
        {
            get => this.selectedEmployeeNumber3;
            set
            {
                this.selectedEmployeeNumber3 = value;
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3C));
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3));
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3R));
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3L));
            }
        }

        [Category("SelectorMode=Single|SelectedValuePath / DisplayMemberPath")]
        [ItemsSourceProperty("Employees")]
        [DisplayMemberPath("Name")]
        [SelectedValuePath("EmployeeNumber")]
        [SelectorStyle(SelectorStyle.RadioButtons)]
        [SelectorMode(SelectorMode.Single)]
        public int SelectedEmployeeNumber3R
        {
            get => this.selectedEmployeeNumber3;
            set
            {
                this.selectedEmployeeNumber3 = value;
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3R));
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3));
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3C));
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3L));
            }
        }

        [Category("SelectorMode=Single|SelectedValuePath / DisplayMemberPath")]
        [ItemsSourceProperty("Employees")]
        [DisplayMemberPath("Name")]
        [SelectedValuePath("EmployeeNumber")]
        [SelectorStyle(SelectorStyle.ListBox)]
        [SelectorMode(SelectorMode.Single)]
        public int SelectedEmployeeNumber3L
        {
            get => this.selectedEmployeeNumber3;
            set
            {
                this.selectedEmployeeNumber3 = value;
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3L));
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3R));
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3));
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumber3C));
            }
        }

        // tab Multiple

        [Category("SelectorMode=Multiple| SelectedValuePath / DisplayMemberPath")]
        [ItemsSourceProperty("Employees")]
        [DisplayMemberPath("Name")]
        [SelectedValuePath("EmployeeNumber")]
        [SelectorStyle(SelectorStyle.ListBox)]
        [SelectorMode(SelectorMode.Multiple)]
        public List<int> SelectedEmployeeNumbersL
        {
            get => this.selectedEmployeeNumbers;
            set
            {
                this.selectedEmployeeNumbers = value;
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumbersL));
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumbersChbx));
            }
        }

        [Category("SelectorMode=Multiple| SelectedValuePath / DisplayMemberPath")]
        [ItemsSourceProperty("Employees")]
        [DisplayMemberPath("Name")]
        [SelectedValuePath("EmployeeNumber")]
        [SelectorStyle(SelectorStyle.RadioButtons)]
        [SelectorMode(SelectorMode.Multiple)]
        public List<int> SelectedEmployeeNumbersChbx
        {
            get => this.selectedEmployeeNumbers;
            set
            {
                this.selectedEmployeeNumbers = value;
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumbersChbx));
                this.RaisePropertyChanged(nameof(SelectedEmployeeNumbersL));
            }
        }

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