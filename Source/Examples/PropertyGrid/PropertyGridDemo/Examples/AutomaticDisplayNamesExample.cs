// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutomaticDisplayNamesExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    [PropertyGridExample]
    public class AutomaticDisplayNamesExample : Example
    {
        private string firstName;
        private string positionX;
        private string positionXYZ;
        private string xyz;

        public string FirstName { get => this.firstName; set { this.firstName = value; this.RaisePropertyChanged(nameof(FirstName)); } }
        public string PositionX { get => this.positionX; set { this.positionX = value; this.RaisePropertyChanged(nameof(PositionX)); } }
        public string PositionXYZ { get => this.positionXYZ; set { this.positionXYZ = value; this.RaisePropertyChanged(nameof(PositionXYZ)); } }
        public string XYZ { get => this.xyz; set { this.xyz = value; this.RaisePropertyChanged(nameof(XYZ)); } }
    }
}