// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;

    /// <summary>
    /// Attribute to mark a class as an example with title, description, and tags.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExampleAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleAttribute"/> class.
        /// </summary>
        /// <param name="title">The title of the example.</param>
        /// <param name="description">The description of the example.</param>
        public ExampleAttribute(string title, string description = null)
        {
            this.Title = title;
            this.Description = description;
        }

        /// <summary>
        /// Gets the title of the example.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the description of the example.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets or sets the tags associated with the example.
        /// </summary>
        public string[] Tags { get; set; }
    }
}
