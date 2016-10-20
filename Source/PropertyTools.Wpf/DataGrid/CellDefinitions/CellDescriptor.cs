namespace PropertyTools.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Contains all the data that can be used to create a <see cref="CellDefinition" /> in a <see cref="CellDefinitionFactory" />.
    /// </summary>
    public class CellDescriptor
    {
        public PropertyDefinition PropertyDefinition { get; set; }

        public PropertyDescriptor Descriptor { get; set; }

        public Type PropertyType { get; set; }

        public object Item { get; set; }

        public string BindingPath { get; set; }

        public IEnumerable<Attribute> Attributes => this.Descriptor?.Attributes.Cast<Attribute>() ?? new Attribute[0];

        public object BindingSource { get; set; }
    }
}