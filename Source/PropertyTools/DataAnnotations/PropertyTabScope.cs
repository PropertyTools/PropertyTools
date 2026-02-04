// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyTabScope.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Specifies the scope of the tab, when associated with a PropertyTabAttribute.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    /// <summary>
    /// Specifies the scope of the tab, when associated with a PropertyTabAttribute.
    /// </summary>
    public enum PropertyTabScope
    {
        /// <summary>
        /// This tab is specific to the current document.
        /// </summary>
        Document = 0,

        /// <summary>
        /// This tab is specific to the current component.
        /// </summary>
        Component = 1,

        /// <summary>
        /// This tab is added to the Property Browser and can be removed only explicitly by a parent component.
        /// </summary>
        Global = 2
    }
}
