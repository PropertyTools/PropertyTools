// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumPropertyViewModel.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections;
    using System.ComponentModel;

    /// <summary>
    /// The enum property view model.
    /// </summary>
    internal class EnumPropertyViewModel : PropertyViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumPropertyViewModel"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public EnumPropertyViewModel(object instance, PropertyDescriptor descriptor, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets EnumValues.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public object EnumValues
        {
            get
            {
                object value;
                if (this.IsEnumerable)
                {
                    var list = this.Instance as IEnumerable;
                    if (list == null)
                    {
                        throw new InvalidOperationException("Instance should be an enumerable.");
                    }

                    return this.GetValue(this.FirstInstance);
                }
                else
                {
                    value = this.GetValue(this.Instance);
                }

                if (!string.IsNullOrEmpty(this.FormatString))
                {
                    return this.FormatValue(value);
                }

                return value;
            }
        }

        #endregion
    }
}