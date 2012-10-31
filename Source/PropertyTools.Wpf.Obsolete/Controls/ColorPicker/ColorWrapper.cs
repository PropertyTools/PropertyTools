// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorWrapper.cs" company="PropertyTools">
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
//   Represents a color.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf.Controls.ColorPicker
{
    using System.ComponentModel;
    using System.Windows.Media;

    /// <summary>
    /// Represents a color.
    /// </summary>
    /// <remarks>
    /// Wrapper class for colors - needed to get unique items in the persistent color list
    /// since Color.XXX added in multiple positions results in multiple items being selected.
    /// Also needed to implement the INotifyPropertyChanged for binding support.
    /// </remarks>
    public class ColorWrapper : INotifyPropertyChanged
    {
        /// <summary>
        /// The color.
        /// </summary>
        private Color color;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorWrapper"/> class.
        /// </summary>
        /// <param name="c">
        /// The c.
        /// </param>
        public ColorWrapper(Color c)
        {
            this.Color = c;
        }

        /// <summary>
        /// The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets Color.
        /// </summary>
        public Color Color
        {
            get
            {
                return this.color;
            }

            set
            {
                this.color = value;
                this.OnPropertyChanged("Color");
            }
        }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="propertyName">
        /// The property name.
        /// </param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}