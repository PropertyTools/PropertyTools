using System;

namespace PropertyTools.Wpf
{
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