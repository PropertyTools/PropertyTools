using System;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FilePathAttribute : Attribute
    {
        public FilePathAttribute(string filter, string defaultExt)
        {
            Filter = filter;
            DefaultExtension = defaultExt;
        }

        public string Filter { get; set; }
        public string DefaultExtension { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class DirectoryPathAttribute : Attribute
    {
    }
}