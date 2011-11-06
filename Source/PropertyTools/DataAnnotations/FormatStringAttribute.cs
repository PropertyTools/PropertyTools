// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FormatStringAttribute.cs" company="PropertyTools">
//   http://propertytools.codeplex.com, license: Ms-PL
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies a format string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FormatStringAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FormatStringAttribute"/> class.
        /// </summary>
        /// <param name="fs">
        /// The fs.
        /// </param>
        public FormatStringAttribute(string fs)
        {
            this.FormatString = fs;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the format string.
        /// </summary>
        /// <value>The format string.</value>
        public string FormatString { get; set; }

        #endregion
    }
}