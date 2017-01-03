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
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Description("This is not yet working.")]
        public SecureString SecureString { get; set; }
    }
}