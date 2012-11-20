namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the decorated property is an output file.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OutputFilePathAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFilePathAttribute" /> class.
        /// </summary>
        /// <param name="defaultExtension">The default extension.</param>
        /// <param name="filter">The filter.</param>
        public OutputFilePathAttribute(string defaultExtension, string filter)
        {
            this.DefaultExtension = defaultExtension;
            this.Filter = filter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFilePathAttribute" /> class.
        /// </summary>
        /// <param name="defaultExtension">The default extension.</param>
        public OutputFilePathAttribute(string defaultExtension)
        {
            this.DefaultExtension = defaultExtension;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFilePathAttribute" /> class.
        /// </summary>
        public OutputFilePathAttribute()
        {
        }

        /// <summary>
        /// Gets or sets the default extension.
        /// </summary>
        /// <value>The default extension.</value>
        public string DefaultExtension { get; set; }

        /// <summary>
        /// Gets or sets the filter.
        /// </summary>
        /// <value>The filter.</value>
        public string Filter { get; set; }
    }
}