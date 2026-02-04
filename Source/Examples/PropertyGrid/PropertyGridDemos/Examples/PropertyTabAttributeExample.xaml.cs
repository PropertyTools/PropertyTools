// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyTabAttributeExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for PropertyTabAttributeExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using PropertyTools;
    using PropertyTools.DataAnnotations;
    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for PropertyTabAttributeExample.
    /// </summary>
    public partial class PropertyTabAttributeExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyTabAttributeExample" /> class.
        /// </summary>
        public PropertyTabAttributeExample()
        {
            this.InitializeComponent();
        }
    }

    public class PropertyTabAttributeExampleViewModel : Observable
    {
        static object StaticInstance = new PropertyTabAttributeExampleModel
        {
            FirstName = "John",
            LastName = "Doe",
            Age = 30,
            Email = "john.doe@example.com",
            Phone = "+1-555-0100",
            Street = "123 Main St",
            City = "Springfield",
            State = "IL",
            ZipCode = "62701",
            Country = "USA"
        };

        public PropertyTabAttributeExampleViewModel()
        {
            this.SelectedObject = StaticInstance;
        }

        private object selectedObject;

        public object SelectedObject
        {
            get => this.selectedObject;
            internal set => this.SetValue(ref this.selectedObject, value);
        }
    }

    public class PropertyTabAttributeExampleModel : Observable
    {
        private string firstName;
        private string lastName;
        private int age;
        private string email;
        private string phone;
        private string street;
        private string city;
        private string state;
        private string zipCode;
        private string country;

        [PropertyTab("Personal Info")]
        [Category("Name")]
        [DisplayName("First Name")]
        [Description("The person's first name")]
        public string FirstName
        {
            get => this.firstName;
            set => this.SetValue(ref this.firstName, value);
        }

        [PropertyTab("Personal Info")]
        [Category("Name")]
        [DisplayName("Last Name")]
        [Description("The person's last name")]
        public string LastName
        {
            get => this.lastName;
            set => this.SetValue(ref this.lastName, value);
        }

        [PropertyTab("Personal Info")]
        [Category("Details")]
        [DisplayName("Age")]
        [Description("The person's age in years")]
        public int Age
        {
            get => this.age;
            set => this.SetValue(ref this.age, value);
        }

        [PropertyTab("Contact")]
        [Category("Electronic")]
        [DisplayName("Email Address")]
        [Description("The person's email address")]
        public string Email
        {
            get => this.email;
            set => this.SetValue(ref this.email, value);
        }

        [PropertyTab("Contact")]
        [Category("Electronic")]
        [DisplayName("Phone Number")]
        [Description("The person's phone number")]
        public string Phone
        {
            get => this.phone;
            set => this.SetValue(ref this.phone, value);
        }

        [PropertyTab("Address")]
        [Category("Location")]
        [DisplayName("Street")]
        [Description("Street address")]
        public string Street
        {
            get => this.street;
            set => this.SetValue(ref this.street, value);
        }

        [PropertyTab("Address")]
        [Category("Location")]
        [DisplayName("City")]
        [Description("City name")]
        public string City
        {
            get => this.city;
            set => this.SetValue(ref this.city, value);
        }

        [PropertyTab("Address")]
        [Category("Location")]
        [DisplayName("State")]
        [Description("State or province")]
        public string State
        {
            get => this.state;
            set => this.SetValue(ref this.state, value);
        }

        [PropertyTab("Address")]
        [Category("Location")]
        [DisplayName("ZIP Code")]
        [Description("ZIP or postal code")]
        public string ZipCode
        {
            get => this.zipCode;
            set => this.SetValue(ref this.zipCode, value);
        }

        [PropertyTab("Address")]
        [Category("Location")]
        [DisplayName("Country")]
        [Description("Country name")]
        public string Country
        {
            get => this.country;
            set => this.SetValue(ref this.country, value);
        }
    }
}
