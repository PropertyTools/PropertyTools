// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeEditor.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Windows;

    /// <summary>
    /// Define a datatemplate for a given type
    /// </summary>
    public class TypeEditor
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether AllowExpand.
        /// </summary>
        public bool AllowExpand { get; set; }

        /// <summary>
        /// Gets or sets EditedType.
        /// </summary>
        public Type EditedType { get; set; }

        /// <summary>
        /// Gets or sets EditorTemplate.
        /// </summary>
        public DataTemplate EditorTemplate { get; set; }

        #endregion
    }
}