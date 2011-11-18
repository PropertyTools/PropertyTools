namespace SimpleDemo
{
    using System.Windows.Media;

    public class MainWindowViewModel
    {
        public Person SelectedPerson { get; set; }

        public MainWindowViewModel()
        {
            this.SelectedPerson = new Person()
                {
                    FirstName = "Anders",
                    LastName = "Andersen",
                    Age = 9,
                    Gender = Genders.Male,
                    Weight = new Mass { Value = 70 },
                    Height = 1.82,
                    HairColor = Colors.Brown
                };
        }
    }
}