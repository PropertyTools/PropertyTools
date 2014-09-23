// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImportantAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies that the decorated property is important.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace CustomFactoryDemo
{
    using System;

    /// <summary>
    /// Specifies that the decorated property is important.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ImportantAttribute : Attribute
    {
    }
}