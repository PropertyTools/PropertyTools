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
        /// <param name="copy">copy the <paramref name="node" /> if set to <c>true</c>.</param>
        /// <returns>
        /// <c>true</c> if this instance accepts a drop of the specified <paramref name="node" />; otherwise, <c>false</c>.
        /// </returns>
        bool CanDrop(object node, DropPosition dropPosition, bool copy);

        /// <summary>
        /// Drops the specified <paramref name="node" /> at this object.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="dropPosition">The position where the <paramref name="node" /> should be dropped.</param>
        /// <param name="copy">copy if set to <c>true</c>.</param>
        void Drop(object node, DropPosition dropPosition, bool copy);
    }
}