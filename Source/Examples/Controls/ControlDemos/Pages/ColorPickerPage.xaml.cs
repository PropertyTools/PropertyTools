// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorPickerPage.xaml.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
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
//   Interaction logic for ColorPickerPage
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ControlDemos
{
    using System.ComponentModel;
    using System.Windows.Media;

    using PropertyTools.Wpf;

    /// <summary>
    /// Interaction logic for ColorPickerPage
    /// </summary>
    public partial class ColorPickerPage : INotifyPropertyChanged
    {
        /// <summary>
        /// The color 1.
        /// </summary>
        private Color color1;

        /// <summary>
        /// The color 2.
        /// </summary>
        private Color color2;

        /// <summary>
        /// The color 3.
        /// </summary>
        private Color color3;

        /// <summary>
        /// The color 4.
        /// </summary>
        private Color? color4;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorPickerPage" /> class.
        /// </summary>
        public ColorPickerPage()
        {
            this.InitializeComponent();
            this.Color1 = Color.FromArgb(80, 255, 0, 0);
            this.Color2 = ColorHelper.UndefinedColor;
            this.Color3 = ColorHelper.Automatic;
            this.Color4 = null;
            this.Brush = Brushes.Blue;
            this.DataContext = this;
        }

        /// <summary>
        /// The property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the color 1.
        /// </summary>
        public Color Color1
        {
            get
            {
                return this.color1;
            }

            set
            {
                this.color1 = value;
                this.RaisePropertyChanged("Color1");
            }
        }

        /// <summary>
        /// Gets or sets the color 2.
        /// </summary>
        public Color Color2
        {
            get
            {
                return this.color2;
            }

            set
            {
                this.color2 = value;
                this.RaisePropertyChanged("Color2");
            }
        }

        /// <summary>
        /// Gets or sets the color 3.
        /// </summary>
        public Color Color3
        {
            get
            {
                return this.color3;
            }

            set
            {
                this.color3 = value;
                this.RaisePropertyChanged("Color3");
            }
        }

        /// <summary>
        /// Gets or sets the color 4.
        /// </summary>
        public Color? Color4
        {
            get
            {
                return this.color4;
            }

            set
            {
                this.color4 = value;
                this.RaisePropertyChanged("Color4");
            }
        }

        /// <summary>
        /// Gets or sets the brush.
        /// </summary>
        public SolidColorBrush Brush { get; set; }

        /// <summary>
        /// Raises the property changed event.
        /// </summary>
        /// <param name="property">The property.</param>
        protected void RaisePropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}