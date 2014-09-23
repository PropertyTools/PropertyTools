// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DirectoryDemo
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();

            tree1.ChildrenPath = "SubDirectories";

            this.RootDirectories = new List<DirectoryViewModel>();
            foreach (var di in DriveInfo.GetDrives())
            {
                this.RootDirectories.Add(new DirectoryViewModel(di.RootDirectory.FullName));
            }

            this.DataContext = this;
        }

        public List<DirectoryViewModel> RootDirectories { get; private set; }
    }
}