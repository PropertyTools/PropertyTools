// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordExample.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System.ComponentModel.DataAnnotations;
    using System.Security;

    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class PasswordExample : Example
    {
        private string password;
        private SecureString secureString;

        [DataType(DataType.Password)]
        public string Password { get => this.password; set { this.password = value; this.RaisePropertyChanged(nameof(Password)); } }

        [Description("This is not yet working.")]
        public SecureString SecureString { get => this.secureString; set { this.secureString = value; this.RaisePropertyChanged(nameof(SecureString)); } }
    }
}