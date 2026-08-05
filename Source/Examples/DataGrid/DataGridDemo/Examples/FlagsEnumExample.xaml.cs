// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FlagsEnumExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for FlagsEnumExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo
{
    using System;
    using System.Collections.ObjectModel;

    using PropertyTools;

    /// <summary>
    /// Interaction logic for FlagsEnumExample.
    /// </summary>
    public partial class FlagsEnumExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlagsEnumExample"/> class.
        /// </summary>
        public FlagsEnumExample()
        {
            this.InitializeComponent();
            this.DataContext = new ViewModel();
        }

        /// <summary>File permissions flags enum used as a DataGrid column type.</summary>
        [Flags]
        public enum Permission
        {
            None = 0,
            Read = 1,
            Write = 2,
            Execute = 4,
            All = Read | Write | Execute,
        }

        /// <summary>Represents a file with an associated permission set.</summary>
        public class Item : Observable
        {
            private string name;
            private Permission access;

            /// <summary>
            /// Initializes a new instance of the <see cref="Item"/> class.
            /// </summary>
            public Item()
            {
                this.name = string.Empty;
                this.access = Permission.None;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Item"/> class.
            /// </summary>
            /// <param name="name">The file name.</param>
            /// <param name="access">The initial permissions.</param>
            public Item(string name, Permission access)
            {
                this.name = name;
                this.access = access;
            }

            /// <summary>Gets or sets the file name.</summary>
            public string Name
            {
                get => this.name;
                set => this.SetValue(ref this.name, value);
            }

            /// <summary>Gets or sets the file permissions (flags enum — edited with CheckBoxList in the DataGrid).</summary>
            public Permission Access
            {
                get => this.access;
                set => this.SetValue(ref this.access, value);
            }
        }

        /// <summary>View model that provides sample items for the <see cref="FlagsEnumExample"/> window.</summary>
        public class ViewModel
        {
            /// <summary>Shared static collection so all open windows observe the same data.</summary>
            private static readonly ObservableCollection<Item> sharedItems = new ObservableCollection<Item>
            {
                new Item("file.txt", Permission.Read),
                new Item("script.sh", Permission.Read | Permission.Execute),
                new Item("data.bin", Permission.Read | Permission.Write),
                new Item("secret.key", Permission.None),
            };

            /// <summary>Gets the collection of items displayed in the DataGrid.</summary>
            public ObservableCollection<Item> Items => sharedItems;
        }
    }
}
