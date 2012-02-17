namespace TestLibrary
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Security;

    public class TestPassword : TestBase
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Description("This is not yet working.")]
        public SecureString SecureString { get; set; }

        public override string ToString()
        {
            return "Password";
        }
    }
}