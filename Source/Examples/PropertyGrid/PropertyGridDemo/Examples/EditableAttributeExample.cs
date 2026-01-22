// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableAttributeExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    [PropertyGridExample]
    public class EditableAttributeExample : Example
    {
        private string editable;
        private string notEditable;

        [System.ComponentModel.DataAnnotations.Editable(true)]
        public string Editable { get => this.editable; set { this.editable = value; this.RaisePropertyChanged(nameof(Editable)); } }

        [System.ComponentModel.DataAnnotations.Editable(false)]
        public string NotEditable { get => this.notEditable; set { this.notEditable = value; this.RaisePropertyChanged(nameof(NotEditable)); } }
    }
}