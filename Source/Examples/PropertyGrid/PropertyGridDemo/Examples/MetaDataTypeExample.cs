namespace ExampleLibrary
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    [PropertyGridExample]
    [MetadataType(typeof(PersonMetadata))]
    public class MetaDataTypeExample : Example
    {
        private string firstName;
        private string lastName;
        private int age;

        public string FirstName { get => this.firstName; set { this.firstName = value; this.RaisePropertyChanged(nameof(FirstName)); } }

        public string LastName { get => this.lastName; set { this.lastName = value; this.RaisePropertyChanged(nameof(LastName)); } }

        public int Age { get => this.age; set { this.age = value; this.RaisePropertyChanged(nameof(Age)); } }
    }

    public class PersonMetadata
    {
        [Category("Data")]
        [DisplayName("Given Name")]
        public string FirstName { get; set; }

        [DisplayName("Surname")]
        public string LastName { get; set; }

        [DisplayName("Years")]
        public int Age { get; set; }
    }
}