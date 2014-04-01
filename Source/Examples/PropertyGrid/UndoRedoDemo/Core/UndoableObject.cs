// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UndoableObject.cs" company="PropertyTools">
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
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;

    public class UndoableObject : INotifyPropertyChanged, ISupportInitialize
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName, object before, object after)
        {
            if (!isInitializing)
            {
                ShellViewModel.UndoRedoService.Add(new PropertyChangeUndoRedoAction(this, propertyName, before));
                Trace.WriteLine(string.Format("Changed {0} from {1} to {2}.", propertyName, FormatValue(before), FormatValue(after)));
            }
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private object FormatValue(object value)
        {
            if (value == null) return "null";
            if (value is string)
                return string.Format("'{0}'", value).Replace("\r\n", "\\n");
            return String.Format(CultureInfo.InvariantCulture, "{0}", value);
        }

        private bool isInitializing;
        public void BeginInit()
        {
            isInitializing = true;
        }

        public void EndInit()
        {
            isInitializing = false;
        }
    }
}