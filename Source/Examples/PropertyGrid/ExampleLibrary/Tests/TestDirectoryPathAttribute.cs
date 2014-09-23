// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestDirectoryPathAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class TestDirectoryPathAttribute : TestBase
    {
        [DirectoryPath]
        [AutoUpdateText]
        public string DirectoryPath { get; set; }

        public override string ToString()
        {
            return "DirectoryPath attribute";
        }
    }
}