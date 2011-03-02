using System;

namespace PropertyTools.Wpf
{
    /// <summary>
    /// The PasswordAttribute is used for password properties.
    /// A PasswordBox control will be used to edit the password.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PasswordAttribute : Attribute
    {
        public char PasswordChar { get; set; }

        public PasswordAttribute(char passwordChar = '*')
        {
            PasswordChar = passwordChar;
        }
    }
}