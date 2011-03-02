using System;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The DirectoryPathAttribute is used for directory properties.
    /// A "Browse" button will be added, and a FolderBrowser dialog will be used to edit the directory path.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DirectoryPathAttribute : Attribute
    {
    }
}