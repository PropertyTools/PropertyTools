// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultipleSelectionModeViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace TreeListBoxDemo.Examples.MultipleSelectionModeExample
{
    /// <summary>
    /// ViewModel for the Multiple Selection Mode Example
    /// </summary>
    public class MultipleSelectionModeViewModel : Observable
    {
        public ObservableCollection<NodeViewModel> Roots { get; set; }

        public string Title => "Multiple Selection Mode Example";

        public MultipleSelectionModeViewModel()
        {
            Roots = new ObservableCollection<NodeViewModel>();

            // Create a tree structure to demonstrate multiple selection mode
            var root1 = CreateTree("Projects", 3, 2);
            var root2 = CreateTree("Resources", 3, 2);
            var root3 = CreateTree("Archives", 3, 2);

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
                var child = new CompositeNode { Name = $"{prefix} - Folder {i + 1}" };
                parent.Children.Add(child);
                
                if (depth > 1)
                {
                    AddChildren(child, breadth, depth - 1, child.Name);
                }
            }
        }
    }
}
