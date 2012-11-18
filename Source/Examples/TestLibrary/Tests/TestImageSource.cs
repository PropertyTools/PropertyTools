namespace TestLibrary
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using PropertyTools.DataAnnotations;

    public class TestImageSource : TestBase
    {
        [Height(double.NaN, 0, 160)]
        public ImageSource Image { get; set; }

        private string path;

        [FilePath("Images files|*.png;*.jpg", ".png")]
        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
                this.Image = new BitmapImage(new Uri(Path, UriKind.Absolute));
            }
        }

        public TestImageSource()
        {
            var uri = new Uri("pack://application:,,,/TestLibrary;component/Images/sheep.png", UriKind.Absolute);
            var sri = Application.GetResourceStream(uri);
            var bitmap = new BitmapImage(uri);
            Image = bitmap;
        }

        public override string ToString()
        {
            return "ImageSource";
        }
    }
}