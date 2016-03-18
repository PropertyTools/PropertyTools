namespace DataGridDemo
{
    using System.Collections.ObjectModel;

    using PropertyTools;

    public partial class EnumExampleWindow
    {
        public EnumExampleWindow()
        {
            this.InitializeComponent();
            this.ItemsSource = new ObservableCollection<EnumObject> { new EnumObject() };
            this.DataContext = this;
        }

        public ObservableCollection<EnumObject> ItemsSource { get; }

        public class EnumObject : Observable
        {
            private Fruit fruit;

            private Fruit? nullableFruit;

            public Fruit Fruit
            {
                get
                {
                    return this.fruit;
                }

                set
                {
                    this.SetValue(ref this.fruit, value, "Fruit");
                }
            }

            public Fruit? NullableFruit
            {
                get
                {
                    return this.nullableFruit;
                }

                set
                {
                    this.SetValue(ref this.nullableFruit, value, "NullableFruit");
                }
            }
        }
    }
}