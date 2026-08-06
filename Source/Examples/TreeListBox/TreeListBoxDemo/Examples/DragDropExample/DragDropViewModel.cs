// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DragDropViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace TreeListBoxDemo.Examples.DragDropExample
{
    /// <summary>
    /// ViewModel for the Drag/Drop Example, demonstrating that only the item currently
    /// under the mouse is highlighted as a drop target (fix for stale IsDropTarget state).
    /// </summary>
    public class DragDropViewModel : Observable
    {
        public ObservableCollection<NodeViewModel> Roots { get; }

        public DragDropViewModel()
        {
            Roots = new ObservableCollection<NodeViewModel>();

            var fruits = CreateFolder("Fruits", "Apple", "Banana", "Cherry", "Date", "Elderberry");
            var vegetables = CreateFolder("Vegetables", "Artichoke", "Broccoli", "Carrot", "Daikon", "Edamame");
            var grains = CreateFolder("Grains", "Amaranth", "Barley", "Corn", "Durum", "Emmer");

            Roots.Add(fruits);
            Roots.Add(vegetables);
            Roots.Add(grains);
        }

        private static NodeViewModel CreateFolder(string folderName, params string[] children)
        {
            var folder = new CompositeNode { Name = folderName };
            foreach (var child in children)
            {
                folder.Children.Add(new CompositeNode { Name = child });
            }
            return new NodeViewModel(folder, null);
        }
    }
}
