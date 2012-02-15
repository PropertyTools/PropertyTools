using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media.Imaging;
using CsvDemo;

namespace CapitalsDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Capitals = CsvDocument.Load<Capital>("Capitals.csv");
            DataContext = this;
        }

        private void DownloadFlags()
        {
            var client = new WebClient();
            Directory.CreateDirectory("png22px");
            Directory.CreateDirectory("png125px");
            Directory.CreateDirectory("svg");
            foreach (var c in Capitals)
            {
                try
                {
                    var uri22 = c.FlagUrl;
                    var uri125 = uri22.Replace("22px", "125px");
                    var urisvg = uri22.Replace("thumb/", "");
                    urisvg = urisvg.Substring(0, urisvg.IndexOf("22px") - 1);

                    client.DownloadFile(new Uri(uri22), string.Format(@"png22px\{0}.png", c.Country));
                    client.DownloadFile(new Uri(uri125), string.Format(@"png125px\{0}.png", c.Country));
                    client.DownloadFile(new Uri(urisvg), string.Format(@"svg\{0}.svg", c.Country));
                }
                catch
                {
                    Debug.WriteLine(c.Country);
                }
            }
        }

        public Collection<Capital> Capitals { get; set; }
    }

    public class Capital
    {
        public bool Selected { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Population { get; set; }
        
        // Attributing Browsable=false hides the column from the grid.
        [Browsable(false)]
        public string FlagUrl { get; set; }

        private BitmapSource flagImage;

        [DisplayName("Flag")]
        public BitmapSource FlagImage
        {
            get
            {
                if (flagImage == null && !String.IsNullOrEmpty(FlagUrl))
                {
                    flagImage = new BitmapImage(new Uri(FlagUrl));
                }
                return flagImage;
            }
        }

    }
}