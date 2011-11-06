// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeDefinition.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Windows;

    /// <summary>
    /// Define formatting and templates for a given type
    /// </summary>
    public class TypeDefinition
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets DisplayTemplate.
        /// </summary>
        public DataTemplate DisplayTemplate { get; set; }

        /// <summary>
        /// Gets or sets EditTemplate.
        /// </summary>
        public DataTemplate EditTemplate { get; set; }

        /// <summary>
        /// Gets or sets FormatString.
        /// </summary>
        public string FormatString { get; set; }

        /// <summary>
        /// Gets or sets HorizontalAlignment.
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// Gets or sets Type.
        /// </summary>
        public Type Type { get; set; }

        #endregion
    }
}