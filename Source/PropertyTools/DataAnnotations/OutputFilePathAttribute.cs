namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies that the decorated property is an output file.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OutputFilePathAttribute : InputFilePathAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFilePathAttribute" /> class.
        /// </summary>
        /// <param name="defaultExtension">The default extension.</param>
        /// <param name="filter">The filter.</param>
        public OutputFilePathAttribute(string defaultExtension, string filter)
            : base(defaultExtension, filter)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFilePathAttribute" /> class.
        /// </summary>
        /// <param name="defaultExtension">The default extension.</param>
        public OutputFilePathAttribute(string defaultExtension)
            : base(defaultExtension)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutputFilePathAttribute" /> class.
        /// </summary>
        public OutputFilePathAttribute()
        {
        }
    }
}