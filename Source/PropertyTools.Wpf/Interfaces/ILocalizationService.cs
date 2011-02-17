using System;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Localize tab/category/property names and tooltips.
    /// Used in PropertyEditor.LocalizationService
    /// </summary>
    public interface ILocalizationService
    {
        string GetString(Type instanceType, string key);
        object GetTooltip(Type instanceType, string key);        
    }
}