// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="PropertyTools">
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
// --------------------------------------------------------------------------------------------------------------------
namespace TreeListBoxDemo
{
    using System.Collections;
    using System.Collections.Generic;

    public class MainViewModel : Observable
    {
        private CompositeNode Model { get; set; }

        public NodeViewModel RootModel { get; set; }

        public IEnumerable Root
        {
            get
            {
                yield return RootModel;
            }
        }

        public IEnumerable Children
        {
            get
            {
                return RootModel.Children;
            }
        }

        public string Title { get; set; }

        public int Count { get; set; }

        public MainViewModel()
        {
            this.Model = new CompositeNode { Name = "Item" };
            this.AddRecursive(this.Model, 4, 4);
            this.Title = "TreeListBox (N=" + this.Count + ")";
            this.RootModel = new NodeViewModel(this.Model, null);
        }

        private void AddRecursive(CompositeNode model, int n, int levels)
        {
            for (int i = 0; i < n; i++)
            {
                var m2 = new CompositeNode { Name = model.Name + (char)('A' + i) };
                model.Children.Add(m2);
                this.Count++;
                if (levels > 0)
                {
                    this.AddRecursive(m2, n, levels - 1);
                }
            }
        }

        public void Select(int count)
        {
            var children = this.RootModel.Children as IList<NodeViewModel>;
            for (int i = 0; i < count; i++)
            {
                children[i].IsSelected = true;
            }
        }
    }
}