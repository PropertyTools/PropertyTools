// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsBagTypeDescriptor.cs" company="PropertyTools">
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
//   Provides a custom type descriptor for the <see cref="ItemsBag" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Provides a custom type descriptor for the <see cref="ItemsBag" />.
    /// </summary>
    public class ItemsBagTypeDescriptor : CustomTypeDescriptor
    {
        /// <summary>
        /// The bag.
        /// </summary>
        private readonly ItemsBag bag;

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsBagTypeDescriptor" /> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="instance">The instance.</param>
        public ItemsBagTypeDescriptor(ICustomTypeDescriptor parent, object instance)
            : base(parent)
        {
            this.bag = (ItemsBag)instance;
        }

        /// <summary>
        /// Get the properties of the items bag.
        /// </summary>
        /// <returns>
        /// The property descriptor collection.
        /// </returns>
        public override PropertyDescriptorCollection GetProperties()
        {
            var result = new List<PropertyDescriptor>();
            foreach (PropertyDescriptor pd in TypeDescriptor.GetProperties(this.bag.BiggestType))
            {
                result.Add(new ItemsBagPropertyDescriptor(pd, this.bag.BiggestType));
            }

            return new PropertyDescriptorCollection(result.ToArray());
        }
    }
}