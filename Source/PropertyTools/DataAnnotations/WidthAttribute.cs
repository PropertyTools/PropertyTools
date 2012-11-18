namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the width of the editing control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class WidthAttribute : Attribute
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WidthAttribute"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        public WidthAttribute(double width)
        {
            this.Width = width;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>The width.</value>
        public double Width { get; set; }

        #endregion
    }
}