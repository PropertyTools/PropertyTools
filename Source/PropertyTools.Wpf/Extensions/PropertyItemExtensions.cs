using System.Collections;
using System.Linq;

namespace PropertyTools.Wpf.Extensions
{
    public static class PropertyItemExtensions
    {
        public static int GetItemsSourceCount(this PropertyItem property, object bindingSource)
        {
            if (property.ItemsSource is IEnumerable items)
            {
                return items.Cast<object>().Count();
            }

            var itemsSourceProperty = property.ItemsSourceDescriptor?.Name;
            if (!string.IsNullOrEmpty(itemsSourceProperty) && bindingSource != null
                && ReflectionExtensions.TryGetFieldOrPropertyValue(bindingSource, itemsSourceProperty, out object objItems)
                && objItems is IEnumerable items2
                )
            {
                return items2.Cast<object>().Count();
            }

            return 0;
        }
    }
}
