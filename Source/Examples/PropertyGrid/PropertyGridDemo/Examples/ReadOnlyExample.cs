// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    [PropertyGridExample]
    public class ReadOnlyExample : Example
    {
        public bool ReadOnlyBoolean => this.Boolean;
        public bool Boolean { get; set; }
    }
}