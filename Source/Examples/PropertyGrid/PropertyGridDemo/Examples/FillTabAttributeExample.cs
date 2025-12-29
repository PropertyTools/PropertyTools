// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FillTabAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class FillTabAttributeExample : Example
    {
        private string text;
        private string text2;

        [Category("Header|Group category is not shown!")]
        [FillTab]
        public string Text { get => this.text; set { this.text = value; this.RaisePropertyChanged(nameof(Text)); } }

        [Category("No header|Group category is not shown!")]
        [FillTab]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public string Text2 { get => this.text2; set { this.text2 = value; this.RaisePropertyChanged(nameof(Text2)); } }
    }
}