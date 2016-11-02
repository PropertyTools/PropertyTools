// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Node.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace AddRemoveDemo
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using PropertyTools;

    public class Node : Observable
    {
        private bool showChildren;

        private bool isChecked;

        public Node(Node parent = null)
        {
            this.Parent = parent;
            this.SubNodes = new ObservableCollection<Node>();
        }

        public string Name { get; set; }

        public Node Parent { get; private set; }

        public IList<Node> SubNodes { get; private set; }

        public bool ShowChildren
        {
            get
            {
                return this.showChildren;
            }

            set
            {
                this.SetValue(ref this.showChildren, value);
            }
        }

        public bool IsChecked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                this.SetValue(ref this.isChecked, value);
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}