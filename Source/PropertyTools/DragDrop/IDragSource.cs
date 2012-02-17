// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDragSource.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// <summary>
//   Allows an object to be dragged.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools
{
    /// <summary>
    /// Allows an object to be dragged.
    /// </summary>
    public interface IDragSource
    {
        #region Public Properties

        /// <summary>
        ///   Gets a value indicating whether this instance is draggable.
        /// </summary>
        bool IsDraggable { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Detaches this instance (for move and drop somewhere else).
        /// </summary>
        void Detach();

        #endregion
    }
}