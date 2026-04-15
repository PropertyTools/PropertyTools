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
		/// <param name="tabSortIndex">The category sort index (tab scope).</param>
		/// <param name="groupSortIndex">The category sort index (group scope).</param>
		public CategoryAttribute(string category, uint tabSortIndex = 0, uint groupSortIndex = 0)
		{
			this.Category = category;
			this.TabSortIndex = tabSortIndex;
			this.GroupSortIndex = groupSortIndex;			
		}

		/// <summary>
		/// Gets the category.
		/// </summary>
		/// <value>The category.</value>
		public virtual string Category { get; private set; }

		/// <summary>
		/// Gets the category sort index (tab scope)
		/// </summary>
		/// <value>The category sort index (tab scope).</value>
		public virtual uint? TabSortIndex { get; private set; }

		/// <summary>
		/// Gets the category sort index (group scope)
		/// </summary>
		/// <value>The category sort index (group scope).</value>
		public virtual uint? GroupSortIndex { get; private set; }
	}
}