using System.Windows.Media;

namespace SimpleDemo
{
    using System.ComponentModel;

    public enum Genders
    {
        Male,
        Female
    }

    public class Person
    {
        [Category("Data")]
        [DisplayName("Given name")]
        public string FirstName { get; set; }

        [DisplayName("Family name")]
        public string LastName { get; set; }

        public int Age { get; set; }

        public double Height { get; set; }

        public Mass Weight { get; set; }

        public Genders Gender { get; set; }

        public Color HairColor { get; set; }

        [Description("Check the box if the person owns a bicycle.")]
        public bool OwnsBicycle { get; set; }
    }
}