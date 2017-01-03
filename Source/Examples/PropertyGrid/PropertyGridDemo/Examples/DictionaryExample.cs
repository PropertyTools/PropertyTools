// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.Collections.Generic;
    using System.ComponentModel;

    [PropertyGridExample]
    public class DictionaryExample : Example
    {
        [Description("This is not yet working.")]
        public Dictionary<int, Item> Dictionary { get; set; } = new Dictionary<int, Item>();
    }
}