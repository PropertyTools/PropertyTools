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
        [DisplayName("Feature 1")]
        public bool Feature1 { get; set; }

        [IndentationLevel(1)]
        [EnableBy(nameof(Feature1))]
        [DisplayName("Feature 1.1")]
        public bool Feature11 { get; set; }

        [IndentationLevel(1)]
        [EnableBy(nameof(Feature1))]
        [DisplayName("Feature 1.2")]
        public bool Feature12 { get; set; }

        [DisplayName("Feature 2")]
        public bool Feature2 { get; set; }

        [IndentationLevel(1)]
        [EnableBy(nameof(Feature2))]
        [DisplayName("Feature 2.1")]
        public bool Feature21 { get; set; }

        [IndentationLevel(1)]
        [EnableBy(nameof(Feature2))]
        [DisplayName("Feature 2.2")]
        public bool Feature22 { get; set; }
    }
}