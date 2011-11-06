// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WidePropertyViewModel.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// Properties marked [WideProperty] are using the full width of the control
    /// </summary>
    public class WidePropertyViewModel : PropertyViewModel
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WidePropertyViewModel"/> class.
        /// </summary>
        /// <param name="instance">
        /// The instance.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor.
        /// </param>
        /// <param name="showHeader">
        /// The show header.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public WidePropertyViewModel(
            object instance, PropertyDescriptor descriptor, bool showHeader, PropertyEditor owner)
            : base(instance, descriptor, owner)
        {
            this.HeaderVisibility = showHeader ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets HeaderVisibility.
        /// </summary>
        public Visibility HeaderVisibility { get; private set; }

        #endregion
    }
}