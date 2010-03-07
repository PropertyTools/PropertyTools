using System.Collections.ObjectModel;
using System.Windows;
using PropertyEditorLibrary;

namespace DatagridDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public CarCollection MyCollection { get; set; }
        public Window1()
        {
            InitializeComponent();
            MyCollection = new CarCollection();
            DataContext = this;
        }
    }

    public class CarCollection
    {
        [WideProperty]
        public ObservableCollection<Car> Cars { get; private set; }

        public CarCollection()
        {
            Cars = new ObservableCollection<Car>();
            // http://gadgetophilia.com/10-best-environment-friendly-cars-green-cars-in-2010/
            Cars.Add(new Car { Make = "Chevrolet", Model = "Volt 230", Fuel = FuelTypes.Hybrid, Price = 40000, Image = "http://gadgetophilia.com/wp-content/uploads/2010/02/2010-chevy-volt.jpg" });
            Cars.Add(new Car { Make = "Toyota", Model = "Prius", Fuel = FuelTypes.Hybrid, Price = 22000, Image = "http://gadgetophilia.com/wp-content/uploads/2010/02/prius-toyota-2010.jpg" });
            Cars.Add(new Car { Make = "Honda", Model = "Insight", Fuel = FuelTypes.Hybrid, Price = 20000, Image = "http://gadgetophilia.com/wp-content/uploads/2010/02/2010_honda_insight.jpg" });
            Cars.Add(new Car { Make = "Ford", Model = "Fusion", Fuel = FuelTypes.Hybrid, Price = 31500, Image = "http://gadgetophilia.com/wp-content/uploads/2010/02/ford-fusion-hybrid.jpg" });
        }
    }

    public enum FuelTypes
    {
        Electric,
        Hybrid,
        Diesel,
        Petrol, // or Gasoline
        BioDiesel,
        NaturalGas,
        Hydrogen
    } ;

    public class Car
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public FuelTypes Fuel { get; set; }
        public bool IsAvailable { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public override string ToString()
        {
            return Make + " " + Model;
        }
    }

}
