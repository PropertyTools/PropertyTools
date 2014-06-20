// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestCheckableItems.cs" company="PropertyTools">
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
// --------------------------------------------------------------------------------------------------------------------

namespace TestLibrary
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using PropertyTools;
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
                return Languages[0].IsChecked;
            }
            set
            {
                Languages[0].IsChecked = value;
            }
        }

        public bool English
        {
            get
            {
                return Languages[1].IsChecked;
            }
            set
            {
                Languages[1].IsChecked = value;
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
    }

    public class CheckableItem : Observable
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