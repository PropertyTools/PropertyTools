// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyChangeUndoRedoAction.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace UndoRedoDemo
{
    using System.ComponentModel;

    public class PropertyChangeUndoRedoAction : IUndoRedoAction
    {
        public object Instance { get; set; }
        public string Property { get; set; }
        public object OldValue { get; set; }

        public PropertyChangeUndoRedoAction(object instance, string property, object oldValue)
        {
            Instance = instance;
            Property = property;
            OldValue = oldValue;
        }

        public override string ToString()
        {
            return "Change " + this.Property;
        }

        public IUndoRedoAction Execute()
        {
            var type = Instance.GetType();
            var pi = type.GetProperty(Property);
            var value = pi.GetValue(Instance, null);
            var ii = Instance as ISupportInitialize;
            if (ii != null)
            {
                ii.BeginInit();
            }

            pi.SetValue(Instance, OldValue, null);
            if (ii != null)
            {
                ii.EndInit();
            }

            return new PropertyChangeUndoRedoAction(Instance, Property, value);
        }
    }
}