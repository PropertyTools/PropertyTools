// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestValuesPropertyAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace TestLibrary
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using PropertyTools.DataAnnotations;

    public class TestValuesPropertyAttribute : TestBase
    {
        [Browsable(false)]
        public List<string> Items { get; set; }

        [ValuesProperty("Items")]
        public string SelectedItem { get; set; }

        public TestValuesPropertyAttribute()
        {
            Items = new List<string>() { "Oslo", "Stockholm", "K�benhavn" };
        }

        public override string ToString()
        {
            return "ValuesProperty";
        }
    }
}