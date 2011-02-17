using System;
using System.Windows.Media;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// Provides images for PropertyEditor tab icons.
    /// Used in PropertyEditor.ImageProvider
    /// </summary>
    public interface IImageProvider
    {
        /// <summary>
        /// Return the image 
        /// </summary>
        /// <param name="type">Type of the instance being edited</param>
        /// <param name="key">Tab name/key</param>
        /// <returns></returns>
        ImageSource GetImage(Type type, string key);
    }
}