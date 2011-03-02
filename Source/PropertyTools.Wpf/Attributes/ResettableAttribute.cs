using System;

namespace PropertyTools.Wpf
{
    /// <summary>
    ///   The ResettableAttribute is used for resettable properties.
    ///   Properties marked with [Resettable] will have a reset button.
    ///   The button will reset the property to the configured reset value.
    ///   Example usage:
    ///   [Resettable]                  // Button label is "Reset"
    ///   [Resettable("Default")]       // Button label is "Default"
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ResettableAttribute : Attribute
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "ResettableAttribute" /> class.
        /// </summary>
        public ResettableAttribute()
        {
            ButtonLabel = "Reset";
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "ResettableAttribute" /> class.
        /// </summary>
        /// <param name = "label">The label.</param>
        public ResettableAttribute(string label)
        {
            ButtonLabel = label;
        }

        public object ButtonLabel { get; set; }
    }
}