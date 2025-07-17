// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyDialog.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Represents a property editing dialog.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Windows;

    /// <summary>
    /// Represents a property editing dialog.
    /// </summary>
    public partial class PropertyDialog : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDialog" /> class.
        /// </summary>
        public PropertyDialog()
        {
            this.InitializeComponent();
            this.MaxWidth = SystemParameters.PrimaryScreenWidth * 0.9;
            this.MaxHeight = SystemParameters.PrimaryScreenHeight * 0.9;
            this.ApplyButton.Visibility = Visibility.Collapsed;
            this.CloseButton.Visibility = Visibility.Collapsed;
            this.HelpButton.Visibility = Visibility.Collapsed;
            this.DataContextChanged += this.PropertyDialogDataContextChanged;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the apply button is visible.
        /// </summary>
        /// <value><c>true</c> if this instance can apply; otherwise, <c>false</c> .</value>
        public bool CanApply
        {
            get
            {
                return this.ApplyButton.Visibility == Visibility.Visible;
            }

            set
            {
                this.ApplyButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Gets the property control.
        /// </summary>
        /// <value>The property control.</value>
        public PropertyGrid PropertyControl
        {
            get
            {
                return this.PropertyGrid1;
            }
        }

        /// <summary>
        /// This stores the current "copy" of the object.
        /// If it is non-<c>null</c>, then we are in the middle of an
        /// editable operation.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// Clone of current object
        /// </returns>
        protected virtual Dictionary<string, object> GetFieldValues(object obj)
        {
            return
                obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(
                    pi => pi.CanRead && pi.GetIndexParameters().Length == 0).Select(
                        pi => new { Key = pi.Name, Value = pi.GetValue(obj, null) }).ToDictionary(
                            k => k.Key, k => k.Value);
        }

        /// <summary>
        /// This restores the state of the current object from the passed clone object.
        /// </summary>
        /// <param name="fieldValues">Object to restore state from</param>
        /// <param name="obj"></param>
        protected virtual void RestoreFieldValues(Dictionary<string, object> fieldValues, object obj)
        {
            foreach (var pi in
                obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(
                    pi => pi.CanWrite && pi.GetIndexParameters().Length == 0))
            {
                object value;
                if (fieldValues.TryGetValue(pi.Name, out value))
                {
                    pi.SetValue(obj, value, null);
                }
                else
                {
                    Debug.WriteLine(
                        "Failed to restore property " + pi.Name
                        + " from cloned values, property not found in Dictionary.");
                }
            }
        }

        /// <summary>
        /// Clones the object memberwise.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <returns>
        /// The memberwise clone.
        /// </returns>
        private static object MemberwiseClone(object src)
        {
            var t = src.GetType();
            var clone = Activator.CreateInstance(t);
            foreach (var pi in
                t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(
                    pi => pi.CanWrite && pi.GetIndexParameters().Length == 0))
            {
                pi.SetValue(clone, pi.GetValue(src, null), null);
            }

            return clone;
        }

        /// <summary>
        /// The apply button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void ApplyButtonClick(object sender, RoutedEventArgs e)
        {
            this.CommitChanges();
        }

        /// <summary>
        /// Begins the edit.
        /// </summary>
        private void BeginEdit()
        {
            var editableDataContext = this.DataContext as IEditableObject;

            if (editableDataContext != null)
            {
                editableDataContext.BeginEdit();
            }
            else
            {
                this.PropertyControl.DataContext = MemberwiseClone(this.DataContext);
            }
        }

        /// <summary>
        /// The cancel button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.CancelEdit();
            this.Close();
        }

        /// <summary>
        /// Cancels the edit.
        /// </summary>
        private void CancelEdit()
        {
            var editableDataContext = this.DataContext as IEditableObject;

            if (editableDataContext != null)
            {
                editableDataContext.CancelEdit();
            }
        }

        /// <summary>
        /// The close button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Commits the changes.
        /// </summary>
        private void CommitChanges()
        {
            // copy changes from cloned object (stored in PropertyEditor.DataContext)
            // to the original object (stored in DataContext)
            var clone = this.PropertyControl.DataContext;
            if (clone == null)
            {
                return;
            }

            foreach (var pi in
                clone.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).
                    Where(pi => pi.CanWrite && pi.GetIndexParameters().Length == 0))
            {
                var newValue = pi.GetValue(clone, null);
                var oldValue = pi.GetValue(this.DataContext, null);

                if (oldValue == null && newValue == null)
                {
                    continue;
                }

                if (oldValue != null && !oldValue.Equals(newValue))
                {
                    pi.SetValue(this.DataContext, newValue, null);
                }
            }
        }

        /// <summary>
        /// Ends the edit.
        /// </summary>
        private void EndEdit()
        {
            var editableDataContext = this.DataContext as IEditableObject;

            if (editableDataContext != null)
            {
                editableDataContext.EndEdit();
            }
            else
            {
                this.CommitChanges();
            }
        }

        /// <summary>
        /// The help button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void HelpButtonClick(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// The ok button click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.EndEdit();
            this.Close();
        }

        /// <summary>
        /// The property dialog data context changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void PropertyDialogDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.BeginEdit();
        }
    }
}