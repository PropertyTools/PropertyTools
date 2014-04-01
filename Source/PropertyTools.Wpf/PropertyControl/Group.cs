// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Group.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
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
//   Represents a group in a <see cref="PropertyGrid" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System.Collections.Generic;
    using System.Windows.Media;

    /// <summary>
    /// Represents a group in a <see cref="PropertyGrid" />.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Group" /> class.
        /// </summary>
        public Group()
        {
            this.Properties = new List<PropertyItem>();
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value> The description. </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value> The header. </value>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value> The icon. </value>
        public ImageSource Icon { get; set; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        public List<PropertyItem> Properties { get; private set; }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns> The to string. </returns>
        public override string ToString()
        {
            return this.Header;
        }
    }
}