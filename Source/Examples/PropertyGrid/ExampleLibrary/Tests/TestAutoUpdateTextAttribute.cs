// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestAutoUpdateTextAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class TestAutoUpdateTextAttribute : TestBase
    {
        [AutoUpdateText]
        public string Comment { get; set; }

        public override string ToString()
        {
            return "AutoUpdateText";
        }
    }
}