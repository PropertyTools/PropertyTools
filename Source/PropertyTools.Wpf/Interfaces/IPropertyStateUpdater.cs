using System;
using System.Collections.Generic;

namespace PropertyTools.Wpf
{
    // todo: consider to solve this in other ways...

    /// <summary>
    /// Implement this interface on your model class to be able to updates the 
    /// property enabled/visible states of the properties. 
    /// This update method is called after every property change of the same instance.
    /// </summary>
    public interface IPropertyStateUpdater
    {
        void UpdatePropertyStates(PropertyStateBag stateBag);
    }

    public class PropertyStateBag
    {
        internal Dictionary<string, bool> EnabledProperties { get; private set; }
        internal Dictionary<string, bool> VisibleProperties { get; private set; }

        public PropertyStateBag()
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
