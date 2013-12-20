// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyStateBag.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2012 Oystein Bjorke
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   The property state bag.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System.Collections.Generic;

    /// <summary>
    /// The property state bag.
    /// </summary>
    public class PropertyStateBag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref = "PropertyStateBag" /> class.
        /// </summary>
        public PropertyStateBag()
        {
            this.EnabledProperties = new Dictionary<string, bool>();
            this.VisibleProperties = new Dictionary<string, bool>();
        }

        /// <summary>
        /// Gets EnabledProperties.
        /// </summary>
        internal Dictionary<string, bool> EnabledProperties { get; private set; }

        /// <summary>
        /// Gets VisibleProperties.
        /// </summary>
        internal Dictionary<string, bool> VisibleProperties { get; private set; }

        /// <summary>
        /// The enable.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="enable">
        /// The enable.
        /// </param>
        public void Enable(string propertyName, bool enable)
        {
            this.EnabledProperties[propertyName] = enable;

            // if (EnabledProperties.ContainsKey(propertyName))
            // EnabledProperties[propertyName] = enable;
            // EnabledProperties.Add(propertyName,enable);
        }

        /// <summary>
        /// The is enabled.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// </returns>
        public bool? IsEnabled(string propertyName)
        {
            if (this.EnabledProperties.ContainsKey(propertyName))
            {
                return this.EnabledProperties[propertyName];
            }

            return null;
        }

    }
}