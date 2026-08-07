// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlagsCellDefinition.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Defines a cell for editing a [Flags] enum with checkboxes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Windows.Controls;

    using PropertyTools.DataAnnotations;

    /// <summary>
    /// Defines a cell for editing a <see cref="FlagsAttribute"/> enum with a <see cref="CheckBoxList"/> control.
    /// </summary>
    public class FlagsCellDefinition : CellDefinition
    {
        /// <summary>
        /// Gets or sets the underlying enum type (never nullable).
        /// </summary>
        public Type EnumType { get; set; }

        /// <summary>
        /// Gets or sets an optional filter that restricts which enum values are shown.
        /// </summary>
        public EnumFilterAttribute EnumFilter { get; set; }

        /// <summary>
        /// Gets or sets the orientation of the <see cref="CheckBoxList"/> shown in the edit cell.
        /// Defaults to <see cref="Orientation.Horizontal"/>.
        /// </summary>
        public Orientation Orientation { get; set; } = Orientation.Horizontal;
    }
}
