// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestAutoUpdateTextAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class AutoUpdateTextAttributeExample : Example
    {
        [AutoUpdateText]
        public string Text { get; set; }

        [AutoUpdateText]
        public double Number { get; set; }
    }
}