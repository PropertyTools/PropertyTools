// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrowsableAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class BrowsableTrueAttributeExample : Example
    {
        [Category("System.ComponentModel (not portable)")]
        [System.ComponentModel.Browsable(true)]
        public string Browsable1 { get; set; }

        public string NotBrowsable1 { get; set; }

        [Category("PropertyTools.DataAnnotations (portable)")]
        [Browsable(true)]
        public string Browsable2 { get; set; }

        public string NotBrowsable2 { get; set; }
    }
}