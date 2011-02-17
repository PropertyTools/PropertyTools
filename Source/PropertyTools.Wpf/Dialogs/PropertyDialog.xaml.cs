using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Automatic Property Dialog
    /// Set the DataContext of the Dialog to the instance you want to edit.
    /// </summary>
    public partial class PropertyDialog : Window
    {
        public PropertyDialog()
        {
            InitializeComponent();
            MaxWidth = SystemParameters.PrimaryScreenWidth*0.9;
            MaxHeight = SystemParameters.PrimaryScreenHeight*0.9;
            ApplyButton.Visibility = Visibility.Collapsed;
            CloseButton.Visibility = Visibility.Collapsed;
            HelpButton.Visibility = Visibility.Collapsed;
            DataContextChanged += PropertyDialog_DataContextChanged;
        }

        #region Cinch

        /// <summary>
        /// This stores the current "copy" of the object. 
        /// If it is non-null, then we are in the middle of an 
        /// editable operation.
        /// </summary>
        //  private Dictionary<string, object> _savedState;       
        /// <summary>
        /// This is used to clone the object.  
        /// Override the method to provide a more efficient clone.  
        /// The default implementation simply reflects across 
        /// the object copying every field.
        /// </summary>
        /// <returns>Clone of current object</returns>
        protected virtual Dictionary<string, object> GetFieldValues(object obj)
        {
            return obj.GetType().GetProperties(BindingFlags.Public |
                                               BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(pi => pi.CanRead && pi.GetIndexParameters().Length == 0)
                .Select(pi => new {Key = pi.Name, Value = pi.GetValue(obj, null)})
                .ToDictionary(k => k.Key, k => k.Value);
        }

        /// <summary>
        /// This restores the state of the current object from the passed clone object.
        /// </summary>
        /// <param name="fieldValues">Object to restore state from</param>
        /// <param name="obj"></param>
        protected virtual void RestoreFieldValues(Dictionary<string, object> fieldValues, object obj)
        {
            foreach (PropertyInfo pi in obj.GetType().GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(pi => pi.CanWrite && pi.GetIndexParameters().Length == 0))
            {
                object value;
                if (fieldValues.TryGetValue(pi.Name, out value))
                    pi.SetValue(obj, value, null);
                else
                {
                    Debug.WriteLine("Failed to restore property " +
                                    pi.Name + " from cloned values, property not found in Dictionary.");
                }
            }
        }

        #endregion


        public PropertyEditor PropertyControl
        {
            get { return propertyControl1; }
        }

        public bool CanApply
        {
            get { return ApplyButton.Visibility == Visibility.Visible; }
            set { ApplyButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        private void PropertyDialog_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            BeginEdit();
        }

        private void BeginEdit()
        {
            var editableDataContext = DataContext as IEditableObject;

            if (editableDataContext != null)
            {
                editableDataContext.BeginEdit();
            }
            else
            {
                PropertyControl.DataContext = MemberwiseClone(DataContext);
            }
        }

        private void EndEdit()
        {
            var editableDataContext = DataContext as IEditableObject;

            if (editableDataContext != null)
            {
                editableDataContext.EndEdit();
            }
            else
            {
                CommitChanges();
            }
        }

        private void CancelEdit()
        {
            var editableDataContext = DataContext as IEditableObject;

            if (editableDataContext != null)
            {
                editableDataContext.CancelEdit();
            }
        }

        private static object MemberwiseClone(object src)
        {
            var t = src.GetType();
            var clone = Activator.CreateInstance(t);
            foreach (
                PropertyInfo pi in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(pi => pi.CanWrite && pi.GetIndexParameters().Length == 0))
            {
                pi.SetValue(clone, pi.GetValue(src, null), null);
            }
            return clone;
        }

        private void CommitChanges()
        {
            // copy changes from cloned object (stored in PropertyEditor.DataContext) 
            // to the original object (stored in DataContext)
            var clone = PropertyControl.DataContext;
            if (clone==null)
                return;

            foreach (
                var pi in
                    clone.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(pi => pi.CanWrite && pi.GetIndexParameters().Length == 0))
            {
                var newValue = pi.GetValue(clone, null);
                var oldValue = pi.GetValue(DataContext, null);
                
                if (oldValue==null && newValue==null)
                    continue;

                if (oldValue!=null && !oldValue.Equals(newValue))
                    pi.SetValue(DataContext, newValue, null);
            }
        }


        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // todo: only if modal dialog
            DialogResult = true;
            EndEdit();
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // RestoreFieldValues(_savedState, DataContext);
            // todo: only if modal dialog
            DialogResult = false;
            CancelEdit();
            Close();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            CommitChanges();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}