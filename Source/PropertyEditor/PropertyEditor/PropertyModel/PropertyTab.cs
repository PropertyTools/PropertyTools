using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace OpenControls
{
    public class PropertyTab :PropertyBase
    {
        public string Name { get; set; }
        public ObservableCollection<PropertyCategory> Categories { get; private set; }
        // public string Icon { get; set; }

        public PropertyTab()
        {
            Categories = new ObservableCollection<PropertyCategory>();
        }
    }
}
