// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemWithDisplayName.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using PropertyTools.DataAnnotations;

    /// <summary>
    /// An item type where the properties have <see cref="System.ComponentModel.DisplayNameAttribute"/> and
    /// <see cref="PropertyTools.DataAnnotations.DisplayNameAttribute"/> applied, demonstrating that
    /// auto-generated <see cref="PropertyTools.Wpf.DataGrid"/> column headers use the DisplayName attributes
    /// instead of the raw property name (see https://github.com/PropertyTools/PropertyTools/issues/191).
    /// </summary>
    public class ItemWithDisplayName
    {
        [System.ComponentModel.DisplayName("Custom name (System.ComponentModel)")]
        public string Name { get; set; }

        [DisplayName("Custom number (PropertyTools.DataAnnotations)")]
        public int Number { get; set; }

        public double Fraction { get; set; }
    }
}
