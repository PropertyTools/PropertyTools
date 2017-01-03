// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    [PropertyGridExample]
    public class CategoryAttributeExample : Example
    {
        [System.ComponentModel.Category("Tab1|Category1")]
        public string Name1 { get; set; }

        [System.ComponentModel.Category("Tab1|Category2")]
        public string Name2 { get; set; }

        [System.ComponentModel.Category("Tab2|Category3")]
        public string Name3 { get; set; }

        [PropertyTools.DataAnnotations.Category("Tab1|Category1 (PropertyTools.DataAnnotations)")]
        public string Name11 { get; set; }

        [PropertyTools.DataAnnotations.Category("Tab1|Category2 (PropertyTools.DataAnnotations)")]
        public string Name12 { get; set; }

        [PropertyTools.DataAnnotations.Category("Tab2|Category3 (PropertyTools.DataAnnotations)")]
        public string Name13 { get; set; }
    }
}