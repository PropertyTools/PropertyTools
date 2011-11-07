namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies a converter that should be used for the property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ConverterAttribute : Attribute
    {
        public Type ConverterType { get; set; }

        public ConverterAttribute(Type converterType)
        {
            this.ConverterType = converterType;
        }
    }
}