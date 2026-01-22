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
        private double progress;
        private double percentage;

        [Progress]
        public double Progress { get => this.progress; set { this.progress = value; this.RaisePropertyChanged(nameof(Progress)); this.RaisePropertyChanged(nameof(ProgressSlider)); } }

        [Progress(0, 100)]
        public double Percentage { get => this.percentage; set { this.percentage = value; this.RaisePropertyChanged(nameof(Percentage)); this.RaisePropertyChanged(nameof(PercentageSlider)); } }

        [Slidable(0, 1)]
        public double ProgressSlider { get => this.Progress; set { this.Progress = value; } }

        [Slidable(0, 100)]
        public double PercentageSlider { get => this.Percentage; set { this.Percentage = value; } }
    }
}