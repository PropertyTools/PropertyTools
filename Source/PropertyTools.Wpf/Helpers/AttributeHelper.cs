using System;
using System.ComponentModel;
using System.Linq;

namespace PropertyTools.Wpf
{
    public static class AttributeHelper
    {
        /// <summary>
        /// Return the first attribute of a given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public static T GetFirstAttribute<T>(PropertyDescriptor descriptor) where T : Attribute
        {
            return descriptor.Attributes.OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Check if an attribute collection contains an attribute of the given type
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        public static bool ContainsAttributeOfType(AttributeCollection attributes, Type attributeType)
        {
            // return attributes.Cast<object>().Any(a => attributeType.IsAssignableFrom(a.GetType()));))))
            foreach (object a in attributes)
                if (attributeType.IsAssignableFrom(a.GetType()))
                    return true;
            return false;
        }

    }

    public class PropertyInfoHelper {

        public static void SetProperty(object instance, string propertyName, object value)
        {
            var pi = instance.GetType().GetProperty(propertyName);
            pi.SetValue(instance, value, null);
        }

        public static object GetProperty(object instance, string propertyName)
        {
            var pi = instance.GetType().GetProperty(propertyName);
            return pi.GetValue(instance, null);
        }

    }
}