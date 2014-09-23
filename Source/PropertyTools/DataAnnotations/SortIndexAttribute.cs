// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SortIndexAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the property sorting index number.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the property sorting index number.
    /// </summary>
    /// <remarks>The sort index is used to sort the tabs, categories and properties.</remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class SortIndexAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortIndexAttribute" /> class.
        /// </summary>
        /// <param name="sortIndex">Index of the sort.</param>
        public SortIndexAttribute(int sortIndex)
        {
            this.SortIndex = sortIndex;
        }

        /// <summary>
        /// Gets or sets the index of the sort.
        /// </summary>
        /// <value>The index of the sort.</value>
        public int SortIndex { get; set; }
    }
}