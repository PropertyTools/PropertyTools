// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AddRemoveDemo
{
    using System;
    using System.Collections;
    using System.Windows.Input;

    using PropertyTools;
    using PropertyTools.Wpf;

    public class MainWindowViewModel : Observable
    {
        private Node selectedItem;

        private Random r = new Random();

        private readonly string[] firstNames = { "Adam", "Bill", "Christopher", "Dan", "Elmer", "Fred", "George", "Hubert", "Ian", "John", "Kevin", "Larry", "Mike", "Nigel" };

        private readonly string[] lastNames = { "Black", "Brown", "Green", "Gray", "Red", "Orange" };

        public MainWindowViewModel()
        {
            this.Root = new Node { Name = "Root" };

            this.SelectedItem = this.Root;

            this.AddCommand = new DelegateCommand(
                () =>
                {
                    this.SelectedItem.SubNodes.Add(new Node(this.SelectedItem) { Name = this.CreateRandomName() });
                    this.SelectedItem.ShowChildren = true;
                },
                () => this.SelectedItem != null);

            this.RemoveCommand = new DelegateCommand(
                () =>
                {
                    var parent = this.SelectedItem.Parent;
                    var index = parent.SubNodes.IndexOf(this.SelectedItem);
                    parent.SubNodes.RemoveAt(index);
                    if (parent.SubNodes.Count > 0)
                    {
                        index = index < parent.SubNodes.Count
                                    ? index
                                    : parent.SubNodes.Count - 1;
                        this.SelectedItem = parent.SubNodes[index];
                    }
                    else
                    {
                        this.SelectedItem = parent;
                    }
                },
                () => this.SelectedItem != null && this.SelectedItem.Parent != null);

            this.ReplaceCommand = new DelegateCommand(
                () =>
                {
                    var parent = this.SelectedItem.Parent;
                    var index = parent.SubNodes.IndexOf(this.SelectedItem);
                    parent.SubNodes[index] = new Node(parent) { Name = this.CreateRandomName() };
                    this.SelectedItem = parent.SubNodes[index];
                },
                () => this.SelectedItem != null && this.SelectedItem.Parent != null);

            this.ClearCommand = new DelegateCommand(
                () => this.SelectedItem.SubNodes.Clear(),
                () => this.SelectedItem != null && this.SelectedItem.SubNodes.Count > 0);

            this.ToggleExpandCommand = new DelegateCommand(
                () => this.SelectedItem.ShowChildren = !this.SelectedItem.ShowChildren,
                () => this.SelectedItem != null && this.SelectedItem.SubNodes.Count > 0);
        }

        public Node Root { get; private set; }

        public IEnumerable HierarchySource
        {
            get
            {
                yield return this.Root;
            }
        }

        public Node SelectedItem
        {
            get
            {
                return this.selectedItem;
            }

            set
            {
                this.SetValue(ref this.selectedItem, value);
            }
        }

        public ICommand AddCommand { get; private set; }

        public ICommand RemoveCommand { get; private set; }

        public ICommand ReplaceCommand { get; private set; }

        public ICommand ClearCommand { get; private set; }

        public ICommand ToggleExpandCommand { get; private set; }

        private string CreateRandomName()
        {
            return string.Join(" ", this.GetRandomItem(this.firstNames), this.GetRandomItem(this.lastNames));
        }

        private string GetRandomItem(string[] list)
        {
            return list[this.r.Next(list.Length)];
        }
    }
}