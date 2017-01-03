// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    [PropertyGridExample]
    public class ReadOnlyAttributeExample : Example
    {
        [System.ComponentModel.Category("Private setter")]
        public string Property1 { get; private set; }

        public bool Check1 { get; private set; }

        [System.ComponentModel.Category("System.ComponentModel")]
        [System.ComponentModel.ReadOnly(true)]
        public string Property2 { get; set; }

        [System.ComponentModel.ReadOnly(true)]
        public bool Check2 { get; set; }

        [System.ComponentModel.Category("PropertyTools.DataAnnotations")]
        [PropertyTools.DataAnnotations.ReadOnly(true)]
        public string Property3 { get; set; }

        [PropertyTools.DataAnnotations.ReadOnly(true)]
        public bool Check3 { get; set; }

        public ReadOnlyAttributeExample()
        {
            this.Check1 = this.Check2 = this.Check3 = true;
            this.Property1 = this.Property2 = this.Property3 = "Read only";
        }
    }
}