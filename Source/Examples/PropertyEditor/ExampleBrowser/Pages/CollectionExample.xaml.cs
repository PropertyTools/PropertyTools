using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using PropertyTools.Wpf;

namespace ExampleBrowser
{
    /// <summary>
    /// Interaction logic for ColorPickerPage.xaml
    /// </summary>
    public partial class CollectionExample : Page
    {
        public Book SelectedBook { get; set; }

        public CollectionExample()
        {
            InitializeComponent();
            SelectedBook = new Book();
            DataContext = this;
        }
    }

    public enum SortIndex { Sorted, NotSorted };

    public class Author
    {
        public string Name { get; set; }
    }

    public class AuthorList : ObservableCollection<Author>
    {
        public ICommand AddAuthor { get; set; }

        public AuthorList()
        {
            AddAuthor = new DelegateCommand(() => this.Add(new Author()), () => Count < 5);
        }
    }

    public class Book
    {
        public AuthorList Authors { get; set; }
        public bool Group { get; set; }
        public bool Show { get; set; }
        public SortIndex SortIndex { get; set; }

        public Book()
        {
            Authors = new AuthorList
                        {
                            new Author {Name = "William Shakespeare"},
                            new Author {Name = "Agatha Christie"}
                        };
        }
    }

}
