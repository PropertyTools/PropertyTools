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
        private string text;
        private double number;

        [AutoUpdateText]
        public string Text { get => this.text; set { this.text = value; this.RaisePropertyChanged(nameof(Text)); } }

        [AutoUpdateText]
        public double Number { get => this.number; set { this.number = value; this.RaisePropertyChanged(nameof(Number)); } }
    }
}