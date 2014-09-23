// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryPathAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies that the property is a directory path.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the property is a directory path.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DirectoryPathAttribute : Attribute
    {
    }
}