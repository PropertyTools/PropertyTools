// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionExample.xaml.cs" company="PropertyTools">
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
//   Interaction logic for ColorPickerPage.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using PropertyTools.Wpf;

namespace ExampleBrowser
{
    /// <summary>
    /// Interaction logic for ColorPickerPage.xaml
    /// </summary>
    public partial class CollectionExample : Page
    {
        public Book SelectedBook { get; set; }

        public CollectionExample()
        {
            InitializeComponent();
            SelectedBook = new Book();
            DataContext = this;
        }
    }

    public enum SortIndex { Sorted, NotSorted };

    public class Author
    {
        public string Name { get; set; }
    }

    public class AuthorList : ObservableCollection<Author>
    {
        public ICommand AddAuthor { get; set; }

        public AuthorList()
        {
            AddAuthor = new DelegateCommand(() => this.Add(new Author()), () => Count < 5);
        }
    }

    public class Book
    {
        public AuthorList Authors { get; set; }
        public bool Group { get; set; }
        public bool Show { get; set; }
        public SortIndex SortIndex { get; set; }

        public Book()
        {
            Authors = new AuthorList
                        {
                            new Author {Name = "William Shakespeare"},
                            new Author {Name = "Agatha Christie"}
                        };
        }
    }

}