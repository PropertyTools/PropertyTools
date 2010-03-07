using System.Collections.Generic;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Updates the property enabled/visible states
    /// This update is done after every property change of the same instance
    /// </summary>
    public interface IPropertyState
    {
        void UpdatePropertyStates(PropertyStates states);
    }

    public class PropertyStates
    {
        public Dictionary<string, bool> EnabledProperties { get; private set; }
        public Dictionary<string, bool> VisibleProperties { get; private set; }

        public PropertyStates()
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
