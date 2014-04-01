// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDropTarget.cs" company="PropertyTools">
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
        /// Determines whether the specified <paramref name="node"/> can be dropped here.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="dropPosition">
        /// The position where the <paramref name="node"/> should be dropped.
        /// </param>
        /// <param name="copy">
        /// copy the <paramref name="node"/> if set to <c>true</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if this instance accepts a drop of the specified <paramref name="node"/>; otherwise, <c>false</c>.
        /// </returns>
        bool CanDrop(object node, DropPosition dropPosition, bool copy);

        /// <summary>
        /// Drops the specified <paramref name="node"/> at this object.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="dropPosition">
        /// The position where the <paramref name="node"/> should be dropped.
        /// </param>
        /// <param name="copy">
        /// copy if set to <c>true</c>.
        /// </param>
        void Drop(object node, DropPosition dropPosition, bool copy);
    }
}