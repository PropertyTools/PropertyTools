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
        [System.ComponentModel.DataAnnotations.Editable(true)]
        public string Editable { get; set; }

        [System.ComponentModel.DataAnnotations.Editable(false)]
        public string NotEditable { get; set; }
    }
}