// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SlidableAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class ProgressAttributeExample : Example
    {
        [Progress]
        public double Progress { get; set; }

        [Progress(0, 100)]
        public double Percentage { get; set; }

        [Slidable(0, 1)]
        public double ProgressSlider { get => this.Progress; set { this.Progress = value; } }

        [Slidable(0, 100)]
        public double PercentageSlider { get => this.Percentage; set { this.Percentage = value; } }
    }
}