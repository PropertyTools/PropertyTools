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
    /// Specifies that it should be easy to insert new items in a List property. When the DataGrid control is used, the easy insert properties will be set.
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
            this.EasyInsertByKeyboard = easyInsert;
            this.EasyInsertByMouse = easyInsert;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EasyInsertAttribute" /> class.
        /// </summary>
        /// <param name="easyInsertByKeyboard">enable easy insert by keyboard if set to <c>true</c>.</param>
        /// <param name="easyInsertByMouse">enable easy insert by mouse if set to <c>true</c>.</param>
        public EasyInsertAttribute(bool easyInsertByKeyboard, bool easyInsertByMouse)
        {
            this.EasyInsertByKeyboard = easyInsertByKeyboard;
            this.EasyInsertByMouse = easyInsertByMouse;
        }

        /// <summary>
        /// Gets a value indicating whether easy insert by keyboard is enabled.
        /// </summary>
        /// <value><c>true</c> if easy insert is enabled; otherwise, <c>false</c>.</value>
        public bool EasyInsertByKeyboard { get; }

        /// <summary>
        /// Gets a value indicating whether easy insert by mouse is enabled.
        /// </summary>
        /// <value><c>true</c> if easy insert is enabled; otherwise, <c>false</c>.</value>
        public bool EasyInsertByMouse { get; }
    }
}