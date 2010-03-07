using System.Collections.Generic;

namespace PropertyEditorLibrary
{
    public class PropertyCategory : PropertyBase
    {
        public List<PropertyBase> Properties { get; private set; }

        public PropertyCategory(string categoryName, PropertyEditor owner)
            : base(owner)
        {
            Name = Header = categoryName;
            Properties = new List<PropertyBase>();
        }

    }
}
