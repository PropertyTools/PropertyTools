// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndentationLevelAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class IndentationLevelAttributeExample : Example
    {
        private bool feature1;
        private bool feature11;
        private bool feature12;
        private bool feature2;
        private bool feature21;
        private bool feature22;

        [DisplayName("Feature 1")]
        public bool Feature1 { get => this.feature1; set { this.feature1 = value; this.RaisePropertyChanged(nameof(Feature1)); } }

        [IndentationLevel(1)]
        [EnableBy(nameof(Feature1))]
        [DisplayName("Feature 1.1")]
        public bool Feature11 { get => this.feature11; set { this.feature11 = value; this.RaisePropertyChanged(nameof(Feature11)); } }

        [IndentationLevel(1)]
        [EnableBy(nameof(Feature1))]
        [DisplayName("Feature 1.2")]
        public bool Feature12 { get => this.feature12; set { this.feature12 = value; this.RaisePropertyChanged(nameof(Feature12)); } }

        [DisplayName("Feature 2")]
        public bool Feature2 { get => this.feature2; set { this.feature2 = value; this.RaisePropertyChanged(nameof(Feature2)); } }

        [IndentationLevel(1)]
        [EnableBy(nameof(Feature2))]
        [DisplayName("Feature 2.1")]
        public bool Feature21 { get => this.feature21; set { this.feature21 = value; this.RaisePropertyChanged(nameof(Feature21)); } }

        [IndentationLevel(1)]
        [EnableBy(nameof(Feature2))]
        [DisplayName("Feature 2.2")]
        public bool Feature22 { get => this.feature22; set { this.feature22 = value; this.RaisePropertyChanged(nameof(Feature22)); } }
    }
}