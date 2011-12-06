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
        /// <returns>The control.</returns>
        FrameworkElement CreateDisplayControl(PropertyDefinition property);

        /// <summary>
        /// Creates the edit control.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The control.</returns>
        FrameworkElement CreateEditControl(PropertyDefinition property);
    }
}