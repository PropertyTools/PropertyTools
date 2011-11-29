﻿// -----------------------------------------------------------------------
// <copyright file="DirectoryViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DirectoryDemo
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.IO;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
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