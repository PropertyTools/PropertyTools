// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsBagTypeDescriptor.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides a custom type descriptor for the ItemsBag.
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