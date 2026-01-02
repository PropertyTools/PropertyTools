// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SlidableAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class SlidableAttributeExample : Example
    {
        private double angle;
        private double angleWithSnapping;
        private int days;

        [Slidable(0, 360, 45, 1)]
        [FormatString("0.00")]
        public double Angle { get => this.angle; set { this.angle = value; this.RaisePropertyChanged(nameof(Angle)); } }

        [Slidable(0, 360, 5, 1, true, 45)]
        [FormatString("{0:0}°")]
        public double AngleWithSnapping { get => this.angleWithSnapping; set { this.angleWithSnapping = value; this.RaisePropertyChanged(nameof(AngleWithSnapping)); } }

        [Slidable(0, 365)]
        public int Days { get => this.days; set { this.days = value; this.RaisePropertyChanged(nameof(Days)); } }
    }
}