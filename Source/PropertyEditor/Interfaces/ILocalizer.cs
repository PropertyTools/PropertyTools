using System;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Localize tab/category/property names and tooltips.
    /// Used in PropertyEditor.Localizer
    /// </summary>
    public interface ILocalizer
    {
        string GetString(Type instanceType, string key);
        object GetTooltip(Type instanceType, string key);        
    }
}