// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsBagTypeDescriptionProvider.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Provides a type description provider for the ItemsBag.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Provides a type description provider for the <see cref="ItemsBag" />.
    /// </summary>
    public class ItemsBagTypeDescriptionProvider : TypeDescriptionProvider
    {
        /// <summary>
        /// The default type provider.
        /// </summary>
        private static readonly TypeDescriptionProvider DefaultTypeProvider =
            TypeDescriptor.GetProvider(typeof(ItemsBag));

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsBagTypeDescriptionProvider" /> class.
        /// </summary>
        public ItemsBagTypeDescriptionProvider()
            : base(DefaultTypeProvider)
        {
        }

        /// <summary>
        /// Gets a custom type descriptor for the given type and object.
        /// </summary>
        /// <param name="objectType">The type of object for which to retrieve the type descriptor.</param>
        /// <param name="instance">An instance of the type. Can be <c>null</c> if no instance was passed to the <see cref="T:System.ComponentModel.TypeDescriptor" /> .</param>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.ICustomTypeDescriptor" /> that can provide metadata for the type.
        /// </returns>
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            ICustomTypeDescriptor defaultDescriptor = base.GetTypeDescriptor(objectType, instance);

            return instance == null ? defaultDescriptor : new ItemsBagTypeDescriptor(defaultDescriptor, instance);
        }
    }
}