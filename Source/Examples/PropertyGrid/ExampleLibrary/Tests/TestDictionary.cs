// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDictionary.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Collections.Generic;
    using System.ComponentModel;

    [PropertyGridExample]
    public class TestDictionary : TestBase
    {
        [Description("This is not yet working.")]
        public Dictionary<int, Item> Dictionary { get; set; }

        public TestDictionary()
        {
            Dictionary = new Dictionary<int, Item>();
        }

        public override string ToString()
        {
            return "Dictionary";
        }
    }
}