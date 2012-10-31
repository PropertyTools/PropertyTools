// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="PropertyTools">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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