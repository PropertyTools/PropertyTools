// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyStateBag.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections.Generic;

    /// <summary>
    /// The property state bag.
    /// </summary>
    public class PropertyStateBag
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "PropertyStateBag" /> class.
        /// </summary>
        public PropertyStateBag()
        {
            this.EnabledProperties = new Dictionary<string, bool>();
            this.VisibleProperties = new Dictionary<string, bool>();
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets EnabledProperties.
        /// </summary>
        internal Dictionary<string, bool> EnabledProperties { get; private set; }

        /// <summary>
        ///   Gets VisibleProperties.
        /// </summary>
        internal Dictionary<string, bool> VisibleProperties { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The enable.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <param name="enable">
        /// The enable.
        /// </param>
        public void Enable(string propertyName, bool enable)
        {
            this.EnabledProperties[propertyName] = enable;

            // if (EnabledProperties.ContainsKey(propertyName))
            // EnabledProperties[propertyName] = enable;
            // EnabledProperties.Add(propertyName,enable);    
        }

        /// <summary>
        /// The is enabled.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        /// <returns>
        /// </returns>
        public bool? IsEnabled(string propertyName)
        {
            if (this.EnabledProperties.ContainsKey(propertyName))
            {
                return this.EnabledProperties[propertyName];
            }

            return null;
        }

        #endregion
    }
}