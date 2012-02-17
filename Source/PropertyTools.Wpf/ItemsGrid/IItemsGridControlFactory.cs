namespace PropertyTools.Wpf.ItemsGrid
{
    using System.Windows;

    /// <summary>
    /// Specifies a control factory for the ItemsGrid.
    /// </summary>
    public interface IItemsGridControlFactory
    {
        /// <summary>
        /// Creates the display control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The control.
        /// </returns>
        FrameworkElement CreateDisplayControl(PropertyDefinition property, object instance);

        /// <summary>
        /// Creates the edit control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// The control.
        /// </returns>
        FrameworkElement CreateEditControl(PropertyDefinition property, object instance);
    }
}