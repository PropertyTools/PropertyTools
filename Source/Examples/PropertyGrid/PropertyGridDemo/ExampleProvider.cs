// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Examples.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides examples defined in the assembly.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemo
{
    using ExampleLibrary;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides examples defined in the assembly.
    /// </summary>
    public static class ExampleProvider
    {
        /// <summary>
        /// Gets a collection of all PropertyGrid examples in the assembly.
        /// </summary>
        /// <returns>
        /// A list of object instances.
        /// </returns>
        public static IEnumerable<object> GetPropertyGridExamples()
        {
            return typeof(ExampleProvider).Assembly.GetTypes().Where(type => type.GetCustomAttributes(typeof(PropertyGridExampleAttribute), false).Any()).OrderBy(type => type.Name).Select(Activator.CreateInstance);
        }
    }
}