// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CutPasteViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace TreeListBoxDemo.Examples.CutPasteExample
{
    /// <summary>
    /// View model for reproducing TreeListBox cut/paste behavior when IsExpanded is preserved on paste.
    /// </summary>
    public class CutPasteViewModel : Observable
    {
        private int nextNodeId = 3;

        private CutPasteNode clipboardNode;

        public CutPasteViewModel()
        {
            this.RootNodes = new ObservableCollection<CutPasteNode>();
            this.ResetScenario();
        }

        public ObservableCollection<CutPasteNode> RootNodes { get; }

        public void ResetScenario()
        {
            this.RootNodes.Clear();
            this.clipboardNode = null;
            this.nextNodeId = 3;

            var node1 = new CutPasteNode("Node 1", null);
            var node2 = new CutPasteNode("Node 2", node1);
            node1.Children.Add(node2);
            this.RootNodes.Add(node1);
        }

        public CutPasteNode AddRoot()
        {
            var root = new CutPasteNode($"Node {this.nextNodeId++}", null);
            this.RootNodes.Add(root);
            return root;
        }

        public CutPasteNode AddChild(CutPasteNode parent)
        {
            if (parent == null)
            {
                return null;
            }

            var child = new CutPasteNode($"Node {this.nextNodeId++}", parent);
            parent.Children.Add(child);
            parent.IsExpanded = true;
            return child;
        }

        public void Cut(CutPasteNode node)
        {
            if (node == null)
            {
                return;
            }

            this.clipboardNode = this.Clone(node, null, true);

            if (node.Parent == null)
            {
                this.RootNodes.Remove(node);
                return;
            }

            node.Parent.Children.Remove(node);
        }

        public CutPasteNode Paste(CutPasteNode targetParent)
        {
            if (this.clipboardNode == null)
            {
                return null;
            }

            CutPasteNode pasted;
            if (targetParent == null)
            {
                pasted = this.Clone(this.clipboardNode, null, true);
                this.RootNodes.Add(pasted);
            }
            else
            {
                pasted = this.Clone(this.clipboardNode, targetParent, true);
                targetParent.Children.Add(pasted);
                targetParent.IsExpanded = true;
            }

            return pasted;
        }

        private CutPasteNode Clone(CutPasteNode source, CutPasteNode parent, bool cloneIsExpanded)
        {
            var clone = new CutPasteNode(source.Name, parent)
            {
                IsExpanded = cloneIsExpanded && source.IsExpanded
            };

            foreach (var child in source.Children)
            {
                clone.Children.Add(this.Clone(child, clone, cloneIsExpanded));
            }

            return clone;
        }
    }

    /// <summary>
    /// Node model used by the cut/paste example.
    /// </summary>
    public class CutPasteNode : Observable
    {
        private bool isExpanded;

        public CutPasteNode(string name, CutPasteNode parent)
        {
            this.Name = name;
            this.Parent = parent;
            this.Children = new ObservableCollection<CutPasteNode>();
        }

        public ObservableCollection<CutPasteNode> Children { get; }

        public string Name { get; set; }

        public CutPasteNode Parent { get; }

        public bool IsExpanded
        {
            get
            {
                return this.isExpanded;
            }

            set
            {
                if (this.isExpanded == value)
                {
                    return;
                }

                this.isExpanded = value;
                this.RaisePropertyChanged(nameof(this.IsExpanded));
            }
        }
    }
}
