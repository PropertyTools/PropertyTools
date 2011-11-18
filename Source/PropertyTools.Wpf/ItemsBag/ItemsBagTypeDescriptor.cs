// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsBagTypeDescriptor.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Provides a custom type descriptor for the <see cref="ItemsBag"/>.
    /// </summary>
    public class ItemsBagTypeDescriptor : CustomTypeDescriptor
    {
        #region Constants and Fields

        /// <summary>
        ///   The bag.
        /// </summary>
        private readonly ItemsBag bag;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsBagTypeDescriptor"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent.
        /// </param>
        /// <param name="instance">
        /// The instance.
        /// </param>
        public ItemsBagTypeDescriptor(ICustomTypeDescriptor parent, object instance)
            : base(parent)
        {
            this.bag = (ItemsBag)instance;
        }

        #endregion

        #region Public Methods

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
                result.Add(new ItemsBagPropertyDescriptor(pd));
            }

            return new PropertyDescriptorCollection(result.ToArray());
        }

        #endregion
    }
}