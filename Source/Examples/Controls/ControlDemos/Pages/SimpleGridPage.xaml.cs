// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleGridPage.xaml.cs" company="PropertyTools">
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
//   Interaction logic for SimpleGridPage.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ControlsDemo
{
    /// <summary>
    /// Interaction logic for SimpleGridPage.xaml
    /// </summary>
    public partial class SimpleGridPage : Page
    {
        private readonly SimpleGridViewModel vm = new SimpleGridViewModel();

        public SimpleGridPage()
        {
            InitializeComponent();
            DataContext = vm;
        }
    }

    public class SimpleGridViewModel : INotifyPropertyChanged
    {
        public Collection<Item> Items { get; set; }

        public SimpleGridViewModel()
        {
            Items = new Collection<Item>();
            for (int i = 0; i < 10; i++)
                Items.Add(new Item() { String = "String" + i, Double = i * 0.1, Int = i * 10 });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }

    public class Item
    {
        public string String { get; set; }
        public double Double { get; set; }
        public int Int { get; set; }
        public bool Bool { get; set; }
        public Fruit Enum { get; set; }
    }
}