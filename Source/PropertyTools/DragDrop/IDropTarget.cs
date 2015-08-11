// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDropTarget.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Allows an object to receive dropped items.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Specifies the current state of the modifier keys (SHIFT, CTRL, and ALT), as well as the state of the mouse buttons.
    /// </summary>
    [Flags]
    public enum DragDropKeyStates
    {
        /// <summary>
        /// No modifier keys or mouse buttons are pressed.
        /// </summary>
        None = 0,
        
        /// <summary>
        /// The left mouse button is pressed.
        /// </summary>
        LeftMouseButton = 1,
        
        /// <summary>
        /// The right mouse button is pressed.
        /// </summary>
        RightMouseButton = 2,
        
        /// <summary>
        /// The shift key is pressed.
        /// </summary>
        ShiftKey = 4,
        
        /// <summary>
        /// The control key is pressed.
        /// </summary>
        ControlKey = 8,
        
        /// <summary>
        /// The middle mouse button is pressed.
        /// </summary>
        MiddleMouseButton = 16,

        /// <summary>
        /// The alt key is pressed.
        /// </summary>
        AltKey = 32,
    }

    /// <summary>
    /// Specifies the effects of a drag-and-drop operation.
    /// </summary>
    public enum DragDropEffect
    {
        /// <summary>
        /// The drop target does not accept the data.
        /// </summary>
        None = 0,

        /// <summary>
        /// The data is copied to the drop target.
        /// </summary>
        Copy = 1,
        
        /// <summary>
        /// The data from the drag source is moved to the drop target.
        /// </summary>
        Move = 2,
        
        /// <summary>
        /// The data from the drag source is linked to the drop target.
        /// </summary>
        Link = 4
    }

    /// <summary>
    /// Allows an object to receive dropped items.
    /// </summary>
    public interface IDropTarget
    {
        /// <summary>
        /// Determines whether the specified <paramref name="node" /> can be dropped here.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="dropPosition">The position where the <paramref name="node" /> should be dropped.</param>
        /// <param name="effect">The drag/drop effect.</param>
        /// <returns><c>true</c> if this instance accepts a drop of the specified <paramref name="node" />; otherwise, <c>false</c>.</returns>
        bool CanDrop(IDragSource node, DropPosition dropPosition, DragDropEffect effect);

        /// <summary>
        /// Drops the specified <paramref name="items" /> at this object.
        /// </summary>
        /// <param name="items">The items to drop.</param>
        /// <param name="dropPosition">The position where the <paramref name="items" /> should be dropped.</param>
        /// <param name="effect">The drag/drop effect.</param>
        /// <param name="initialKeyStates">The initial drag/drop key states.</param>
        void Drop(IEnumerable<IDragSource> items, DropPosition dropPosition, DragDropEffect effect, DragDropKeyStates initialKeyStates);
    }
}