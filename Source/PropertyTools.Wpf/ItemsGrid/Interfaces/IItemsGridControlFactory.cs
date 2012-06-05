// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IItemsGridControlFactory.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Specifies a control factory for the ItemsGrid.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Windows;

    /// <summary>
    /// Specifies a control factory for the ItemsGrid.
    /// </summary>
    public interface IItemsGridControlFactory
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates the display control and sets the binding.
        /// </summary>
        /// <param name="d">
        /// The property definition. 
        /// </param>
        /// <param name="index">
        /// The index (if bound to a list element). 
        /// </param>
        /// <returns>
        /// The control. 
        /// </returns>
        FrameworkElement CreateDisplayControl(PropertyDefinition d, int index);

        /// <summary>
        /// Creates the edit control and sets the binding.
        /// </summary>
        /// <param name="d">
        /// The property definition. 
        /// </param>
        /// <param name="index">
        /// The index (if bound to a list element). 
        /// </param>
        /// <returns>
        /// The control. 
        /// </returns>
        FrameworkElement CreateEditControl(PropertyDefinition d, int index);

        #endregion
    }
}