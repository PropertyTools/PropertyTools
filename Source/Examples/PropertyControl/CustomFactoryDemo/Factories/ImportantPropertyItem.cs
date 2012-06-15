namespace CustomFactoryDemo
{
    using System.ComponentModel;

    using PropertyTools.Wpf;

    /// <summary>
    /// Provides a custom PropertyItem class for properties decorated by the ImportantAttribute.
    /// </summary>
    /// <remarks>
    /// You need to sub-class the PropertyItemFactory to get this to work.
    /// </remarks>
    public class ImportantPropertyItem : PropertyItem
    {
        public ImportantPropertyItem(PropertyDescriptor pd, object instance)
            : base(pd, instance)
        {
        }
    }
}