// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyGridExampleAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Used to identify PropertyGrid example classes.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    /// <summary>
    /// Used to identify PropertyGrid example classes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PropertyGridExampleAttribute : Attribute
    {
    }
}