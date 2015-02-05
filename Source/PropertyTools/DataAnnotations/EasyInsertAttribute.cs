// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EasyInsertAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies that it should be easy to insert new items in a List property. When the DataGrid control is used, the EasyInsert property will be set.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that it should be easy to insert new items in a List property. When the DataGrid control is used, the EasyInsert property will be set.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EasyInsertAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EasyInsertAttribute" /> class.
        /// </summary>
        /// <param name="easyInsert">if set to <c>true</c> [easy insert].</param>
        public EasyInsertAttribute(bool easyInsert)
        {
            this.EasyInsert = easyInsert;
        }

        /// <summary>
        /// Gets or sets a value indicating whether easy insert is enabled.
        /// </summary>
        /// <value><c>true</c> if [easy insert]; otherwise, <c>false</c>.</value>
        public bool EasyInsert { get; set; }
    }
}