// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MultipleRootsViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace TreeListBoxDemo.Examples.MultipleRootsExample
{
    /// <summary>
    /// ViewModel for the Multiple Roots Example
    /// </summary>
    public class MultipleRootsViewModel : Observable
    {
        public ObservableCollection<NodeViewModel> Roots { get; set; }

        public string Title => "Multiple Roots Example";

        public MultipleRootsViewModel()
        {
            Roots = new ObservableCollection<NodeViewModel>();

            // Create multiple root items to demonstrate the fix for issue #282
            var root1 = CreateTree("Root 1", 3, 3);
            var root2 = CreateTree("Root 2", 3, 3);
            var root3 = CreateTree("Root 3", 3, 3);

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
                var child = new CompositeNode { Name = $"{prefix}.{(char)('A' + i)}" };
                parent.Children.Add(child);
                
                if (depth > 1)
                {
                    AddChildren(child, breadth, depth - 1, child.Name);
                }
            }
        }
    }
}
