// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Range.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace CustomFactoryDemo
{
    public class Range
    {
        public double Minimum { get; set; }
        public double Maximum { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}", this.Minimum, this.Maximum);
        }
    }
}