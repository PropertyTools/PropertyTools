using System;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The FilePathAttribute is used for path properties.
    /// The filter and default extension will be used by the open/save file dialogs.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FilePathAttribute : Attribute
    {
        public FilePathAttribute(string filter, string defaultExt, bool useOpenDialog = true)
        {
            Filter = filter;
            DefaultExtension = defaultExt;
            UseOpenDialog = useOpenDialog;
        }

        public bool UseOpenDialog { get; set; }
        public string Filter { get; set; }
        public string DefaultExtension { get; set; }
    }
}