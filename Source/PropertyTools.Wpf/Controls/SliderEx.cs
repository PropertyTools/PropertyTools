﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SliderEx.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.ComponentModel;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;

    /// <summary>
    /// Represents a slider that calls IEditableObject.BeginEdit/EndEdit when thumb dragging.
    /// </summary>
    public class SliderEx : Slider
    {
        #region Methods

        /// <summary>
        /// The on thumb drag completed.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnThumbDragCompleted(DragCompletedEventArgs e)
        {
            base.OnThumbDragCompleted(e);
            var editableObject = this.DataContext as IEditableObject;
            if (editableObject != null)
            {
                editableObject.EndEdit();
            }
        }

        /// <summary>
        /// The on thumb drag started.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnThumbDragStarted(DragStartedEventArgs e)
        {
            base.OnThumbDragStarted(e);
            var editableObject = this.DataContext as IEditableObject;
            if (editableObject != null)
            {
                editableObject.BeginEdit();
            }
        }

        #endregion
    }
}