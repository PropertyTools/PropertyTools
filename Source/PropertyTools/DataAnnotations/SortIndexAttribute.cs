// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SortIndexAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the property sorting index number.
    /// </summary>
    /// <remarks>
    /// The sort index is used to sort the tabs, categories and properties.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class SortIndexAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SortIndexAttribute"/> class.
        /// </summary>
        /// <param name="SortIndex">
        /// Index of the sort.
        /// </param>
        public SortIndexAttribute(int SortIndex)
        {
            this.SortIndex = SortIndex;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the index of the sort.
        /// </summary>
        /// <value>The index of the sort.</value>
        public int SortIndex { get; set; }

        #endregion
    }
}