﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window504.xaml.cs" company="PropertyTools">
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
//   Interaction logic for Window504.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    /// <summary>
    /// Interaction logic for Window504.xaml
    /// </summary>
    public partial class Window504
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Window504" /> class.
        /// </summary>
        public Window504()
        {
            this.InitializeComponent();

            this.ItemsSource = new ObservableCollection<ObservableCollection<Color>>
                                {
                                    new ObservableCollection<Color>
                                        {
                                            Colors
                                                .Blue,
                                            Colors
                                                .Red,
                                            Colors
                                                .Green
                                        },
                                    new ObservableCollection<Color>
                                        {
                                            Colors
                                                .Honeydew,
                                            Colors
                                                .Red,
                                            Colors
                                                .AliceBlue
                                        },
                                    new ObservableCollection<Color>
                                        {
                                            Colors
                                                .Blue,
                                            Colors
                                                .Red,
                                            Colors
                                                .LightBlue
                                        }
                                };

            this.DataContext = this;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        public ObservableCollection<ObservableCollection<Color>> ItemsSource { get; set; }
    }
}