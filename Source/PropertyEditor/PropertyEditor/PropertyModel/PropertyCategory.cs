using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OpenControls
{
    public class PropertyCategory : PropertyBase
    {
        public string Name { get; set; }
        public ObservableCollection<PropertyBase> Properties { get; private set; }

        public PropertyCategory()
        {
            Properties = new ObservableCollection<PropertyBase>();
        }
    }
}
