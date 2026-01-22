// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    [PropertyGridExample]
    public class ReadOnlyExample : Example
    {
        private bool boolean;

        public bool ReadOnlyBoolean => this.Boolean;
        public bool Boolean { get => this.boolean; set { this.boolean = value; this.RaisePropertyChanged(nameof(Boolean)); this.RaisePropertyChanged(nameof(ReadOnlyBoolean)); } }
    }
}