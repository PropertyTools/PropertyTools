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
        [Slidable(0, 360, 45, 1)]
        [FormatString("0.00")]
        public double Angle { get; set; }

        [Slidable(0, 360, 5, 1, true, 45)]
        [FormatString("{0:0}°")]
        public double AngleWithSnapping { get; set; }

        [Slidable(0, 365)]
        public int Days { get; set; }
    }
}