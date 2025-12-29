// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeaderPlacementAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class HeaderPlacementAttributeExample : Example
    {
        private string headerAbove;
        private string headerCollapsed;
        private string headerLeft;

        [Category("HeaderPlacement.Above")]
        [HeaderPlacement(HeaderPlacement.Above)]
        [Height(100)]
        public string HeaderAbove { get => this.headerAbove; set { this.headerAbove = value; this.RaisePropertyChanged(nameof(HeaderAbove)); } }

        [Category("HeaderPlacement.Collapsed")]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        [Height(100)]
        public string HeaderCollapsed { get => this.headerCollapsed; set { this.headerCollapsed = value; this.RaisePropertyChanged(nameof(HeaderCollapsed)); } }

        [Category("HeaderPlacement.Left")]
        [HeaderPlacement(HeaderPlacement.Left)]
        [Height(100)]
        public string HeaderLeft { get => this.headerLeft; set { this.headerLeft = value; this.RaisePropertyChanged(nameof(HeaderLeft)); } }        
    }
}