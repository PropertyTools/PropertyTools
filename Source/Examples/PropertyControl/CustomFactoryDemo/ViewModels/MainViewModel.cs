namespace CustomFactoryDemo
{
    public class MainViewModel
    {
        public TestObject TestObject { get; set; }

        public PropertyControlFactory PropertyControlFactory { get; set; }

        public PropertyItemFactory PropertyItemFactory { get; set; }

        public MainViewModel()
        {
            this.PropertyControlFactory = new PropertyControlFactory();
            this.PropertyItemFactory = new PropertyItemFactory();
            this.TestObject = new TestObject { Title = "Title", Range1 = new Range { Minimum = 0, Maximum = 100 } };
        }
    }
}