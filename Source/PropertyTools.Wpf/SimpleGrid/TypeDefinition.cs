using System;
using System.Windows;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Define formatting and templates for a given type
    /// </summary>
    public class TypeDefinition
    {
        public Type Type { get; set; }
        
        public string StringFormat { get; set; }
        
        public HorizontalAlignment HorizontalAlignment { get; set; }

        public DataTemplate DisplayTemplate { get; set; }
        
        public DataTemplate EditTemplate { get; set; }
    }
}