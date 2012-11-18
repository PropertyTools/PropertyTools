namespace PropertyTools.DataAnnotations
{
    using System;

    /// <summary>
    /// Specifies the item header property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ItemHeaderPropertyAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the item header property.
        /// </summary>
        /// <value>The item header property.</value>
        public string ItemHeaderProperty { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemHeaderPropertyAttribute"/> class.
        /// </summary>
        /// <param name="itemHeaderProperty">The item header property.</param>
        public ItemHeaderPropertyAttribute(string itemHeaderProperty)
        {
            this.ItemHeaderProperty = itemHeaderProperty;
        }
    }
}