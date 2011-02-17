using System;
using System.Windows;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Define a datatemplate for a given type
    /// </summary>
    public class TypeEditor
    {
        public Type EditedType { get; set; }
        public DataTemplate EditorTemplate { get; set; }
        public bool AllowExpand { get; set; }
    }
}