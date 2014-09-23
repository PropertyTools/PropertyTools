// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestCheckableItems.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Collections.Generic;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class TestCheckableItems : TestBase
    {
        [Category("Checkable items")]
        [CheckableItems("IsChecked", "Name")]
        public List<CheckableItem> Languages { get; private set; }

        [Category("Boolean values")]
        public bool Norwegian
        {
            get
            {
                return this.Languages[0].IsChecked;
            }

            set
            {
                this.Languages[0].IsChecked = value;
            }
        }

        public bool English
        {
            get
            {
                return this.Languages[1].IsChecked;
            }

            set
            {
                this.Languages[1].IsChecked = value;
            }
        }

        public TestCheckableItems()
        {
            this.Languages = new List<CheckableItem>
                                 {
                                     new CheckableItem { Name = "Norwegian" },
                                     new CheckableItem { Name = "English" },
                                     new CheckableItem { Name = "French" },
                                     new CheckableItem { Name = "Greek" }
                                 };
        }

        public override string ToString()
        {
            return "Checkable items";
        }

        public class CheckableItem : PropertyTools.Observable
        {
            private bool isChecked;

            public bool IsChecked
            {
                get
                {
                    return this.isChecked;
                }

                set
                {
                    this.SetValue(ref this.isChecked, value, () => this.IsChecked);
                }
            }

            public string Name { get; set; }
        }
    }
}