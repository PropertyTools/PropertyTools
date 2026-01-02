// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BrowsableAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class BrowsableAttributeExample : Example
    {
        private string @default;
        private string browsable1;
        private string notBrowsable1;
        private string browsable2;
        private string notBrowsable2;

        [Category("No attribute")]
        public string Default { get => this.@default; set { this.@default = value; this.RaisePropertyChanged(nameof(Default)); } }

        [Category("System.ComponentModel (not portable)")]
        [System.ComponentModel.Browsable(true)]
        public string Browsable1 { get => this.browsable1; set { this.browsable1 = value; this.RaisePropertyChanged(nameof(Browsable1)); } }

        [System.ComponentModel.Browsable(false)]
        public string NotBrowsable1 { get => this.notBrowsable1; set { this.notBrowsable1 = value; this.RaisePropertyChanged(nameof(NotBrowsable1)); } }

        [Category("PropertyTools.DataAnnotations (portable)")]
        [Browsable(true)]
        public string Browsable2 { get => this.browsable2; set { this.browsable2 = value; this.RaisePropertyChanged(nameof(Browsable2)); } }

        [Browsable(false)]
        public string NotBrowsable2 { get => this.notBrowsable2; set { this.notBrowsable2 = value; this.RaisePropertyChanged(nameof(NotBrowsable2)); } }
    }
}