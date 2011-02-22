using System;
using System.Windows;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Define display/edit data templates for a given type
    /// </summary>
    public class TypeTemplates
    {
        public Type Type { get; set; }
        public DataTemplate DisplayTemplate { get; set; }
        public DataTemplate EditTemplate { get; set; }
    }
}