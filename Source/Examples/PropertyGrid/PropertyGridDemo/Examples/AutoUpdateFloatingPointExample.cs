// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoUpdateFloatingPointExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class AutoUpdateFloatingPointExample : Example
    {
        private double l0;

        [AutoUpdateText]
        public double L0 { get => this.l0; set { this.l0 = value; this.RaisePropertyChanged(nameof(L0)); } }
    }
}
