// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryViewModel.cs" company="PropertyTools">
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

namespace DirectoryDemo
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;

    public class DirectoryViewModel
    {
        public string DirectoryPath { get; set; }

        public string Directory
        {
            get
            {
                return System.IO.Path.GetDirectoryName(DirectoryPath);
            }
        }

        public string Name
        {
            get
            {
                var name = System.IO.Path.GetFileName(DirectoryPath);
                return String.IsNullOrEmpty(name) ? this.DirectoryPath : name;
            }
            set
            {
                this.DirectoryPath = Path.Combine(Directory, value);
            }
        }

        private ObservableCollection<DirectoryViewModel> subDirectories;

        public ObservableCollection<DirectoryViewModel> SubDirectories
        {
            get
            {
                if (subDirectories == null)
                {
                    subDirectories = new ObservableCollection<DirectoryViewModel>();
                    try
                    {
                        foreach (var dir in System.IO.Directory.GetDirectories(DirectoryPath))
                        {
                            subDirectories.Add(new DirectoryViewModel(dir));
                        }
                    }
                    catch
                    {

                    }
                }
                return subDirectories;
            }
        }

        public bool HasItems
        {
            get
            {
                return SubDirectories.Count > 0;
            }
        }

        public bool IsSelected { get; set; }

        public bool IsExpanded { get; set; }

        public override string ToString()
        {
            return DirectoryPath;
        }

        public DirectoryViewModel(string path)
        {
            this.DirectoryPath = path;
        }
    }
}