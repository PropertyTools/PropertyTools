// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UndoableObject.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
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