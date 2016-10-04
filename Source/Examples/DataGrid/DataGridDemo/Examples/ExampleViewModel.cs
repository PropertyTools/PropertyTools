namespace DataGridDemo
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    public class ExampleViewModel
    {
        static ExampleViewModel()
        {
            StaticItemsSource = new ObservableCollection<ExampleObject>();
            for (int i = 0; i < 50; i++)
            {
                StaticItemsSource.Add(
                    new ExampleObject
                    {
                        Boolean = true,
                        DateTime = DateTime.Now,
                        Color = Colors.Blue,
                        Number = Math.PI,
                        Fruit = Fruit.Apple,
                        Integer = 7,
                        Selector = null,
                        String = "Hello"
                    });
                StaticItemsSource.Add(
                    new ExampleObject
                    {
                        Boolean = false,
                        DateTime = DateTime.Now.AddDays(-1),
                        Color = Colors.Gold,
                        Number = Math.E,
                        Fruit = Fruit.Pear,
                        Integer = -1,
                        Selector = null,
                        String = "World"
                    });
            }
        }
        public ObservableCollection<ExampleObject> ItemsSource => StaticItemsSource;

        public static ObservableCollection<ExampleObject> StaticItemsSource;

    }
}