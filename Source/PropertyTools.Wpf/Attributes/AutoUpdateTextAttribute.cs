using System;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The AutoUpdateTextAttribute forces the binding to update the value at every change.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoUpdateTextAttribute : Attribute
    {
    }
}