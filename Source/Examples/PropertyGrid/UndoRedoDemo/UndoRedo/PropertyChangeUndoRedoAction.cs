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
            this.Instance = instance;
            this.Property = property;
            this.OldValue = oldValue;
        }

        public override string ToString()
        {
            return "Change " + this.Property;
        }

        public IUndoRedoAction Execute()
        {
            var type = this.Instance.GetType();
            var pi = type.GetProperty(this.Property);
            var value = pi.GetValue(this.Instance, null);
            var ii = this.Instance as ISupportInitialize;
            if (ii != null)
            {
                ii.BeginInit();
            }

            pi.SetValue(this.Instance, this.OldValue, null);
            if (ii != null)
            {
                ii.EndInit();
            }

            return new PropertyChangeUndoRedoAction(this.Instance, this.Property, value);
        }
    }
}