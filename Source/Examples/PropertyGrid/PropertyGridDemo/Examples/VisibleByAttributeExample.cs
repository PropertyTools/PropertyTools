// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VisibleByAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class VisibleByAttributeExample : Example
    {
        private double weight;
        private bool isWeightVisible;
        private TestEnumeration color2;
        private string greenOrBlue;
        private string green;
        private string notRed;

        [Category("Visibility by convention")]
        public double Weight { get => this.weight; set { this.weight = value; this.RaisePropertyChanged(nameof(Weight)); } }

        public bool IsWeightVisible { get => this.isWeightVisible; set { this.isWeightVisible = value; this.RaisePropertyChanged(nameof(IsWeightVisible)); } }

        [Category("Visibility by attribute")]
        [Description("Select green or blue to make the string visible")]
        public TestEnumeration Color2 { get => this.color2; set { this.color2 = value; this.RaisePropertyChanged(nameof(Color2)); this.RaisePropertyChanged(nameof(IsColor2Ok)); } }

        [Browsable(false)]
        public bool IsColor2Ok => this.Color2 == TestEnumeration.Green || this.Color2 == TestEnumeration.Blue;

        [VisibleBy("IsColor2Ok")]
        public string GreenOrBlue { get => this.greenOrBlue; set { this.greenOrBlue = value; this.RaisePropertyChanged(nameof(GreenOrBlue)); } }

        [VisibleBy("Color2", TestEnumeration.Green)]
        public string Green { get => this.green; set { this.green = value; this.RaisePropertyChanged(nameof(Green)); } }

        [Category("Collapsing group")]
        [Description("This group is collapsing when String3 is not visible.")]
        [VisibleBy("IsColor2Ok")]
        public string NotRed { get => this.notRed; set { this.notRed = value; this.RaisePropertyChanged(nameof(NotRed)); } }

        public VisibleByAttributeExample()
        {
            this.IsWeightVisible = true;
        }

   }
}