// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExampleInfo.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ExampleLibrary;

namespace DemoLauncher
{
    /// <summary>
    /// Information about a discovered example window.
    /// </summary>
    public class ExampleInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleInfo"/> class.
        /// </summary>
        /// <param name="type">The type of the example window.</param>
        public ExampleInfo(Type type)
        {
            this.Type = type;
            this.LoadMetadata();
        }

        /// <summary>
        /// Gets the type of the example window.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the title of the example.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the description of the example.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the tags associated with the example.
        /// </summary>
        public IReadOnlyList<string> Tags { get; private set; } = new List<string>();

        /// <summary>
        /// Gets the assembly name of the example.
        /// </summary>
        public string AssemblyName => this.Type.Assembly.GetName().Name;

        /// <summary>
        /// Creates a new instance of the example window.
        /// </summary>
        /// <returns>A new window instance.</returns>
        public Window CreateInstance()
        {
            return (Window)Activator.CreateInstance(this.Type);
        }

        /// <summary>
        /// Shows the example window.
        /// </summary>
        public void Show()
        {
            var window = this.CreateInstance();
            window.Show();
        }

        private void LoadMetadata()
        {
            // Try to get metadata from ExampleAttribute
            var attribute = this.Type.GetCustomAttributes(typeof(ExampleAttribute), false)
                .FirstOrDefault() as ExampleAttribute;

            if (attribute != null)
            {
                this.Title = attribute.Title;
                this.Description = attribute.Description;
                this.Tags = attribute.Tags ?? Array.Empty<string>();
            }
            else
            {
                // Fall back to creating an instance and getting the Title property
                try
                {
                    var instance = this.CreateInstance();
                    this.Title = instance.Title ?? this.Type.Name;
                    instance.Close();
                }
                catch
                {
                    this.Title = this.Type.Name;
                }

                this.Description = string.Empty;
                this.Tags = Array.Empty<string>();
            }
        }

        public override string ToString()
        {
            return this.Title;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ExampleInfo other)
            {
                return this.Type == other.Type;
            }
            return false;
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return this.Type.GetHashCode();
        }
    }
}
