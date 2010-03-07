using System;
using System.Windows.Media;

namespace PropertyEditorLibrary
{
    /// <summary>
    /// Provides images for PropertyEditor tab icons.
    /// Used in PropertyEditor.ImageProvider
    /// </summary>
    public interface IImageProvider
    {
        ImageSource GetImage(Type type, string key);
    }
}