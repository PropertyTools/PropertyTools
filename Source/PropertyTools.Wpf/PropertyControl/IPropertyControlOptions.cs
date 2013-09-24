namespace PropertyTools.Wpf
{
    using System;

    /// <summary>
    /// Specifies options for the PropertyControl
    /// </summary>
    public interface IPropertyControlOptions
    {
        /// <summary>
        /// Gets the required attribute.
        /// </summary>
        /// <value> The required attribute. </value>
        Type RequiredAttribute { get; }

        /// <summary>
        /// Gets a value indicating whether to show declared properties only.
        /// </summary>
        /// <value> <c>true</c> if only declared properties should be shown; otherwise, <c>false</c> . </value>
        bool ShowDeclaredOnly { get; }

        /// <summary>
        /// Gets a value indicating whether to show read only properties.
        /// </summary>
        /// <value> <c>true</c> if read only properties should be shown; otherwise, <c>false</c> . </value>
        bool ShowReadOnlyProperties { get; }
    }
}