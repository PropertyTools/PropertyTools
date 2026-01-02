// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageSourceExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class ImageSourceExample : Example
    {
        private ImageSource image;
        private string path;

        [Height(double.NaN, 0, 160)]
        public ImageSource Image { get => this.image; set { this.image = value; this.RaisePropertyChanged(nameof(Image)); } }

        [InputFilePath(".png", "Images files|*.png;*.jpg")]
        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
                this.RaisePropertyChanged(nameof(Path));
                this.Image = new BitmapImage(new Uri(this.Path, UriKind.Absolute));
            }
        }

        public ImageSourceExample()
        {
            var uri = new Uri("pack://application:,,,/PropertyGridDemo;component/Examples/sheep.png", UriKind.Absolute);
            var sri = Application.GetResourceStream(uri);
            var bitmap = new BitmapImage(uri);
            this.Image = bitmap;
        }        
    }
}