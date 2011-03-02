using System;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The SortOrderAttribute is used to sort the tabs, categories and properties.
    /// Example usage: 
    ///   [SortOrder(100)]
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SortOrderAttribute : Attribute
    {
        public SortOrderAttribute(int sortOrder)
        {
            SortOrder = sortOrder;
        }

        public int SortOrder { get; set; }
    }
}