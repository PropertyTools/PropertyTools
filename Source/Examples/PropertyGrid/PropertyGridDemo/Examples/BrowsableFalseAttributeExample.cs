// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrowsableAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class BrowsableFalseAttributeExample : Example
    {
        [Category("System.ComponentModel (not portable)")]
        public string Browsable1 { get; set; }

        [System.ComponentModel.Browsable(false)]
        public string NotBrowsable1 { get; set; }

        [Category("PropertyTools.DataAnnotations (portable)")]
        public string Browsable2 { get; set; }

        [Browsable(false)]
        public string NotBrowsable2 { get; set; }
    }
}