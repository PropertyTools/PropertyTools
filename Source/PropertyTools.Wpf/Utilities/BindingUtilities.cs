namespace PropertyTools.Wpf
{
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// Provides binding utility extension methods.
    /// </summary>
    public static class BindingUtilities
    {
        /// <summary>
        /// The value to boolean converter.
        /// </summary>
        private static readonly ValueToBooleanConverter ValueToBooleanConverter = new ValueToBooleanConverter();

        /// <summary>
        /// Binds the IsEnabled property of the specified element to the specified property and value.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="enabledByProperty">The enabled by property.</param>
        /// <param name="enabledByValue">The value that enables the element.</param>
        public static void SetIsEnabledBinding(this FrameworkElement element, string enabledByProperty, object enabledByValue)
        {
            var isEnabledBinding = new Binding(enabledByProperty);
            if (enabledByValue != null)
            {
                isEnabledBinding.ConverterParameter = enabledByValue;
                isEnabledBinding.Converter = ValueToBooleanConverter;
            }

            element.SetBinding(UIElement.IsEnabledProperty, isEnabledBinding);
        }
    }
}