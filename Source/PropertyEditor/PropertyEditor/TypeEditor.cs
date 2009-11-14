using System;
using System.Windows;

namespace OpenControls
{
    public class TypeEditor
    {
        public Type EditedType { get; set; }
        public DataTemplate EditorTemplate { get; set; }
        public bool AllowExpand { get; set; }
    }
}