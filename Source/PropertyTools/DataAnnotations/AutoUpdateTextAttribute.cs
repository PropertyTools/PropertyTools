// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AutoUpdateTextAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies that the text binding should be triggered at every change.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the text binding should be triggered at every change.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoUpdateTextAttribute : Attribute
    {
    }
}