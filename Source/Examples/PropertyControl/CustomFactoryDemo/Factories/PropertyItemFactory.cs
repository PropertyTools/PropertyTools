namespace CustomFactoryDemo
{
    using System;
    using System.ComponentModel;

    using PropertyTools.Wpf;

    /// <summary>
    /// Provides a custom property item factory.
    /// </summary>
    public class PropertyItemFactory : DefaultPropertyItemFactory
    {
        protected override PropertyItem CreateCore(PropertyDescriptor pd, object instance)
        {
            // Check if the property is decorated with an "ImportantAttribute"
            var ia = AttributeHelper.GetFirstAttribute<ImportantAttribute>(pd);
            if (ia != null)
            {
                // Create a custom PropertyItem instance
                return new ImportantPropertyItem(pd, instance);
            }

            return base.CreateCore(pd, instance);
        }

        protected override string GetDisplayName(PropertyDescriptor pd, Type declaringType)
        {
            // Use the property name as display name - this will be passed to the GetLocalizedString later
            return pd.Name;
        }

        protected override string GetLocalizedString(string key, Type declaringType)
        {
            // Add a star to show that we have handled this
            // A localization mechanism can be used to localize the strings
            return key + "*";
        }
    }
}