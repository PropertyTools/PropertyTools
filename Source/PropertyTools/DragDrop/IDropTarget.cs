// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDropTarget.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
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
        #region Public Methods

        /// <summary>
        /// Determines whether the specified PropertyTools.Wpf can be dropped here.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="mode">
        /// The drop position mode.
        /// </param>
        /// <param name="copy">
        /// copy the PropertyTools.Wpf if set to <c>true</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if this instance accepts a drop of the specified PropertyTools.Wpf; otherwise, <c>false</c>.
        /// </returns>
        bool CanDrop(object node, DropPosition mode, bool copy);

        /// <summary>
        /// Drops the specified PropertyTools.Wpf at this object.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="mode">
        /// The drop position mode.
        /// </param>
        /// <param name="copy">
        /// copy if set to <c>true</c>.
        /// </param>
        void Drop(object node, DropPosition mode, bool copy);

        #endregion
    }
}