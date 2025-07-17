// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtendedToolkitDataGridControlFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Implements a data grid control factory for the Extended.Toolkit.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.ExtendedToolkit
{
    using System;

    /// <summary>
    /// Implements a data grid control factory for the Extended.Toolkit.
    /// </summary>
    public class ExtendedToolkitDataGridControlFactory : DataGridControlFactory
    {
        /// <summary>
        /// The lazy instance
        /// </summary>
        private static readonly Lazy<ExtendedToolkitDataGridControlFactory> LazyInstance = new Lazy<ExtendedToolkitDataGridControlFactory>();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ExtendedToolkitDataGridControlFactory Instance => LazyInstance.Value;
    }
}
