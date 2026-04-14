// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the name of the category in which to group the property or event when displayed in a PropertyGrid control.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
	using System;

    /// <summary>
    /// Specifies the name of the category in which to group the property or event when displayed in a PropertyGrid control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class CategoryAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryAttribute"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        public CategoryAttribute(string category)
        {
            this.Category = category;
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="CategoryAttribute"/> class.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <param name="sortIndex">The category sort index.</param>
		public CategoryAttribute(string category, uint sortIndex)
		{
			this.Category = category;
			this.SortIndex = sortIndex;
		}

		/// <summary>
		/// Gets the category.
		/// </summary>
		/// <value>The category.</value>
		public virtual string Category { get; private set; }

		/// <summary>
		/// Gets the category sort index
		/// </summary>
		/// <value>The category sort index.</value>
		public virtual uint? SortIndex { get; private set; }
	}
}