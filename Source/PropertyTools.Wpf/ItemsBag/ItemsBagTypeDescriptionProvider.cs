// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsBagTypeDescriptionProvider.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Provides a type description provider for the <see cref="ItemsBag"/>.
    /// </summary>
    public class ItemsBagTypeDescriptionProvider : TypeDescriptionProvider
    {
        #region Constants and Fields

        /// <summary>
        ///   The default type provider.
        /// </summary>
        private static readonly TypeDescriptionProvider DefaultTypeProvider =
            TypeDescriptor.GetProvider(typeof(ItemsBag));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ItemsBagTypeDescriptionProvider" /> class.
        /// </summary>
        public ItemsBagTypeDescriptionProvider()
            : base(DefaultTypeProvider)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a custom type descriptor for the given type and object.
        /// </summary>
        /// <param name="objectType">
        /// The type of object for which to retrieve the type descriptor.
        /// </param>
        /// <param name="instance">
        /// An instance of the type. Can be null if no instance was passed to the <see cref="T:System.ComponentModel.TypeDescriptor"/>.
        /// </param>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.ICustomTypeDescriptor"/> that can provide metadata for the type.
        /// </returns>
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            ICustomTypeDescriptor defaultDescriptor = base.GetTypeDescriptor(objectType, instance);

            return instance == null ? defaultDescriptor : new ItemsBagTypeDescriptor(defaultDescriptor, instance);
        }

        #endregion
    }
}