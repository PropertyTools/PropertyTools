// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryViewModel.cs" company="PropertyTools">
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
//   ViewModel for categories.
//   The categories can be shown as GroupBox, Expander or by the Header.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PropertyTools.Wpf
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    /// <summary>
    /// ViewModel for categories.
    /// The categories can be shown as GroupBox, Expander or by the Header.
    /// </summary>
    public class CategoryViewModel : ViewModelBase
    {
        /// <summary>
        /// The is enabled.
        /// </summary>
        private bool isEnabled = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryViewModel"/> class.
        /// </summary>
        /// <param name="categoryName">
        /// The category name.
        /// </param>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public CategoryViewModel(string categoryName, PropertyEditor owner)
            : base(owner)
        {
            this.Name = this.Header = categoryName;
            this.Properties = new List<PropertyViewModel>();
        }

        /// <summary>
        /// Gets or sets a value indicating whether IsEnabled.
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return this.isEnabled;
            }

            set
            {
                this.isEnabled = value;
                this.NotifyPropertyChanged("IsEnabled");
            }
        }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets Properties.
        /// </summary>
        public List<PropertyViewModel> Properties { get; private set; }

        /// <summary>
        /// Gets Visibility.
        /// </summary>
        public Visibility Visibility
        {
            get
            {
                return Visibility.Visible;
            }
        }

        /// <summary>
        /// The sort.
        /// </summary>
        public void Sort()
        {
            this.Properties = this.Properties.OrderBy(p => p.SortIndex).ToList();
        }

    }
}