namespace ExampleLibrary
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    [PropertyGridExample]
    [MetadataType(typeof(PersonMetadata))]
    public class MetaDataTypeExample : Example
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Age { get; set; }
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