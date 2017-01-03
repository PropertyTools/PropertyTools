// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryPathAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class DirectoryPathAttributeExample : Example
    {
        [DirectoryPath]
        [AutoUpdateText]
        public string DirectoryPath { get; set; }
    }
}