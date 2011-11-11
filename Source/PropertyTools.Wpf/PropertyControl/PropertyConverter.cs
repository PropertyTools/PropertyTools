namespace PropertyTools.Wpf
{
    using System;
    using System.ComponentModel;
    using System.Windows.Data;

    /// <summary>
    /// Represents a property converter.
    /// </summary>
    /// <remarks>
    /// Add property converters to the PropertyControl.Converters collection.
    /// </remarks>
    public class PropertyConverter
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref = "TypeEditor" /> class.
        /// </summary>
        public PropertyConverter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeEditor"/> class.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="converter">The converter.</param>
        public PropertyConverter(Type propertyType, IValueConverter converter)
        {
            this.PropertyType = propertyType;
            this.Converter = converter;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the type to edit.
        /// </summary>
        public Type PropertyType { get; set; }

        /// <summary>
        ///   Gets or sets template for this type.
        /// </summary>
        public IValueConverter Converter { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Determines whether the specified type is assignable to the EditedType.
        /// </summary>
        /// <param name = "type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is assignable; otherwise, <c>false</c>.
        /// </returns>
        public bool IsAssignable(Type type)
        {
            return this.PropertyType.IsAssignableFrom(type);
        }

        /// <summary>
        /// Gets the target type of the converter.
        /// </summary>
        /// <returns></returns>
        public Type GetTargetType()
        {
            foreach (var a in TypeDescriptor.GetAttributes(this.Converter.GetType()))
            {
                var vca = a as ValueConversionAttribute;
                if (vca != null) return vca.TargetType;
            }
            return null;
        }
        #endregion
    }
}