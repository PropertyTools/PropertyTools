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
        private string property1;
        private bool check1;
        private string property2;
        private bool check2;
        private string property3;
        private bool check3;

        [System.ComponentModel.Category("Private setter")]
        public string Property1 { get => this.property1; private set { this.property1 = value; this.RaisePropertyChanged(nameof(Property1)); } }

        public bool Check1 { get => this.check1; private set { this.check1 = value; this.RaisePropertyChanged(nameof(Check1)); } }

        [System.ComponentModel.Category("System.ComponentModel")]
        [System.ComponentModel.ReadOnly(true)]
        public string Property2 { get => this.property2; set { this.property2 = value; this.RaisePropertyChanged(nameof(Property2)); } }

        [System.ComponentModel.ReadOnly(true)]
        public bool Check2 { get => this.check2; set { this.check2 = value; this.RaisePropertyChanged(nameof(Check2)); } }

        [System.ComponentModel.Category("PropertyTools.DataAnnotations")]
        [PropertyTools.DataAnnotations.ReadOnly(true)]
        public string Property3 { get => this.property3; set { this.property3 = value; this.RaisePropertyChanged(nameof(Property3)); } }

        [PropertyTools.DataAnnotations.ReadOnly(true)]
        public bool Check3 { get => this.check3; set { this.check3 = value; this.RaisePropertyChanged(nameof(Check3)); } }

        public ReadOnlyAttributeExample()
        {
            this.Check1 = this.Check2 = this.Check3 = true;
            this.Property1 = this.Property2 = this.Property3 = "Read only";
        }
    }
}