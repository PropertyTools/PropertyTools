using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Media;

namespace CustomEditorDemo
{
    public class ColorList : List<Tuple<string, Color>>
    {
        public ColorList()
        {
            var fields = typeof (Colors).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var fi in fields)
            {
                var c = (Color) fi.GetValue(null, null);
                Add(new Tuple<string, Color>(fi.Name, c));
            }
        }
    }
}