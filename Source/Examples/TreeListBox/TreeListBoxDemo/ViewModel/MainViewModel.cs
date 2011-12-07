namespace TreeListBoxDemo
{
    using System.Collections;
    using System.Collections.Generic;

    public class MainViewModel : Observable
    {
        private CompositeNode Model { get; set; }

        public NodeViewModel RootModel { get; set; }

        public IEnumerable Root
        {
            get
            {
                yield return RootModel;
            }
        }

        public IEnumerable Children
        {
            get
            {
                return RootModel.Children;
            }
        }

        public string Title { get; set; }

        public int Count { get; set; }

        public MainViewModel()
        {
            this.Model = new CompositeNode { Name = "Item" };
            this.AddRecursive(this.Model, 3);
            Title = "TreeListBox (N=" + Count + ")";
            RootModel = new NodeViewModel(Model, null);
        }

        private void AddRecursive(CompositeNode model, int levels)
        {
            for (int i = 0; i < 3; i++)
            {
                var m2 = new CompositeNode { Name = model.Name + (char)('A' + i) };
                model.Children.Add(m2);
                Count++;
                if (levels > 0)
                    this.AddRecursive(m2, levels - 1);
            }
        }

        public void Select(int count)
        {
            var children = this.RootModel.Children as IList<NodeViewModel>;
            for (int i = 0; i < count; i++)
                children[i].IsSelected = true;
        }
    }
}