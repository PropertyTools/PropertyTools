using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenControls
{
    public interface IPropertyState
    {
        void GetPropertyStates(PropertyState state);
    }

    public class PropertyState
    {
        public Dictionary<string, bool> EnabledProperties { get; private set; }
        public Dictionary<string, bool> VisibleProperties { get; private set; }

        public PropertyState()
        {
            EnabledProperties = new Dictionary<string, bool>();
            VisibleProperties = new Dictionary<string, bool>();
        }

        public void Enable(string propertyName, bool enable)
        {
            EnabledProperties[propertyName] = enable;
            //if (EnabledProperties.ContainsKey(propertyName))
            //    EnabledProperties[propertyName] = enable;
            //EnabledProperties.Add(propertyName,enable);    
        }

        public bool? IsEnabled(string propertyName)
        {
            if (EnabledProperties.ContainsKey(propertyName))
                return EnabledProperties[propertyName];
            return null;
        }
    }
}
