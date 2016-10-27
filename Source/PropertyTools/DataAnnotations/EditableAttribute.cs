// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EditableAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies whether the property this attribute is bound to is editable.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies whether the property this attribute is bound to is editable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EditableAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyAttribute" /> class.
        /// </summary>
        /// <param name="allowEdit">Allow edit if set to <c>true</c>.</param>
        public EditableAttribute(bool allowEdit)
        {
            this.AllowEdit = allowEdit;
        }

        /// <summary>
        /// Indicates whether or not the field/property allows editing of the
        /// value.
        /// </summary>
        /// <value>
        /// When <c>true</c>, the field/property is editable.
        /// <para>
        /// When <c>false</c>, the field/property is not editable.
        /// </para>
        /// </value>
        public bool AllowEdit { get; private set; }
    }
}