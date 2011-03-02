using System;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The HeightAttribute is used to control the height of TextBoxes.
    /// Example usage: 
    ///   [Height(100)] 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class HeightAttribute : Attribute
    {
        public HeightAttribute(double height)
        {
            Height = height;
        }

        public double Height { get; set; }
    }
}
