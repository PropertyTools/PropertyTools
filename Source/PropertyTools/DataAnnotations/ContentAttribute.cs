// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies that the value contains content that should be handled by a ContentControl.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the value contains content that should be handled by a ContentControl.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ContentAttribute : Attribute
    {
    }
}