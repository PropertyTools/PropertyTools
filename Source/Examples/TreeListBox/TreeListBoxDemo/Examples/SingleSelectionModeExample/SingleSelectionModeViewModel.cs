// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleSelectionModeViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace TreeListBoxDemo.Examples.SingleSelectionModeExample
{
    /// <summary>
    /// ViewModel for the Single Selection Mode Example
    /// </summary>
    public class SingleSelectionModeViewModel : Observable
    {
        public ObservableCollection<NodeViewModel> Roots { get; set; }

        public string Title => "Single Selection Mode Example";

        public SingleSelectionModeViewModel()
        {
            Roots = new ObservableCollection<NodeViewModel>();

            // Create a tree structure to demonstrate single selection mode
            var root1 = CreateTree("Documents", 2, 2);
            var root2 = CreateTree("Pictures", 2, 2);
            var root3 = CreateTree("Music", 2, 2);

            Roots.Add(root1);
            Roots.Add(root2);
            Roots.Add(root3);
        }

        /// <summary>
        /// Creates a tree structure with the specified depth and breadth
        /// </summary>
        private NodeViewModel CreateTree(string rootName, int breadth, int depth)
        {
            var rootModel = new CompositeNode { Name = rootName };
            AddChildren(rootModel, breadth, depth, rootName);
            return new NodeViewModel(rootModel, null);
        }

        private void AddChildren(CompositeNode parent, int breadth, int depth, string prefix)
        {
            if (depth <= 0) return;

            for (int i = 0; i < breadth; i++)
            {
                var child = new CompositeNode { Name = $"{prefix} - Item {i + 1}" };
                parent.Children.Add(child);
                
                if (depth > 1)
                {
                    AddChildren(child, breadth, depth - 1, child.Name);
                }
            }
        }
    }
}
