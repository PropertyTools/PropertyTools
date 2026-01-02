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
        private string name1;
        private string name2;
        private string name3;
        private string name11;
        private string name12;
        private string name13;

        [System.ComponentModel.Category("Tab1|Category1")]
        public string Name1 { get => this.name1; set { this.name1 = value; this.RaisePropertyChanged(nameof(Name1)); } }

        [System.ComponentModel.Category("Tab1|Category2")]
        public string Name2 { get => this.name2; set { this.name2 = value; this.RaisePropertyChanged(nameof(Name2)); } }

        [System.ComponentModel.Category("Tab2|Category3")]
        public string Name3 { get => this.name3; set { this.name3 = value; this.RaisePropertyChanged(nameof(Name3)); } }

        [PropertyTools.DataAnnotations.Category("Tab1|Category1 (PropertyTools.DataAnnotations)")]
        public string Name11 { get => this.name11; set { this.name11 = value; this.RaisePropertyChanged(nameof(Name11)); } }

        [PropertyTools.DataAnnotations.Category("Tab1|Category2 (PropertyTools.DataAnnotations)")]
        public string Name12 { get => this.name12; set { this.name12 = value; this.RaisePropertyChanged(nameof(Name12)); } }

        [PropertyTools.DataAnnotations.Category("Tab2|Category3 (PropertyTools.DataAnnotations)")]
        public string Name13 { get => this.name13; set { this.name13 = value; this.RaisePropertyChanged(nameof(Name13)); } }
    }
}