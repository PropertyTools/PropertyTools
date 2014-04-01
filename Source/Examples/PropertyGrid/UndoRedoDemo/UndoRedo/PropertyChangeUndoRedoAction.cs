// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyChangeUndoRedoAction.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
//   
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//   
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
            return Property;
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