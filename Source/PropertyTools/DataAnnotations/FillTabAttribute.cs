// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FillTabAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies that the control representing the property should fill the entire size of the tab page.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the control representing the property should fill the entire size of the tab page.
    /// </summary>
    /// <remarks>This requires only one property on the tab page.</remarks>
    [AttributeUsage(AttributeTargets.Property)]
    public class FillTabAttribute : Attribute
    {
    }
}