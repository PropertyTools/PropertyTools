// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnabledByAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.ComponentModel;

    [PropertyGridExample]
    public class EnabledByAttributeExample : Example
    {
        private int age;
        private bool isAgeEnabled;
        private string name;
        private bool isNameEnabled;
        private TestEnumeration color;
        private string enableByIsColorOkProperty;
        private string enableByBlueColor;

        [Category("Enable by convention")]
        [Description("The IsAgeEnabled property controls the enable state.")]
        public int Age { get => this.age; set { this.age = value; this.RaisePropertyChanged(nameof(Age)); } }

        public bool IsAgeEnabled { get => this.isAgeEnabled; set { this.isAgeEnabled = value; this.RaisePropertyChanged(nameof(IsAgeEnabled)); } }

        [Description("The IsNameEnabled property controls the enable state. This property is also optional.")]
        [PropertyTools.DataAnnotations.Optional]
        public string Name { get => this.name; set { this.name = value; this.RaisePropertyChanged(nameof(Name)); } }

        public bool IsNameEnabled { get => this.isNameEnabled; set { this.isNameEnabled = value; this.RaisePropertyChanged(nameof(IsNameEnabled)); } }

        [Category("Enable by attribute")]
        [Description("Select green or blue to enable the string")]
        public TestEnumeration Color { get => this.color; set { this.color = value; this.RaisePropertyChanged(nameof(Color)); this.RaisePropertyChanged(nameof(IsColorOk)); } }

        [PropertyTools.DataAnnotations.EnableBy("IsColorOk")]
        [Description("The IsColorOk property controls the enable state. The property should be enabled when the color is green or blue.")]
        public string EnableByIsColorOkProperty { get => this.enableByIsColorOkProperty; set { this.enableByIsColorOkProperty = value; this.RaisePropertyChanged(nameof(EnableByIsColorOkProperty)); } }

        [PropertyTools.DataAnnotations.EnableBy("Color", TestEnumeration.Blue)]
        [Description("The control is enabled when Color = Blue.")]
        public string EnableByBlueColor { get => this.enableByBlueColor; set { this.enableByBlueColor = value; this.RaisePropertyChanged(nameof(EnableByBlueColor)); } }

        [Browsable(false)]
        public bool IsColorOk => this.Color == TestEnumeration.Green || this.Color == TestEnumeration.Blue;
    }
}