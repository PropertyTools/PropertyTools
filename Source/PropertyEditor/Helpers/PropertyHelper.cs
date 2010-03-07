using System;
using System.ComponentModel;

namespace PropertyEditorLibrary
{
    public static class PropertyHelper
    {
        /// <summary>
        /// Return the first attribute of a given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="descriptor"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(PropertyDescriptor descriptor) where T : Attribute
        {
            foreach (Attribute a in descriptor.Attributes)
            {
                var oa = a as T;
                if (oa != null)
                {
                    return oa;
                }
            }
            return null;
        }

        #region Set/get of properties
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
        #endregion

    }
}