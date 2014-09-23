// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaxLengthAttribute.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools
{
    using System.ComponentModel.DataAnnotations;

    public class MaxLengthAttribute : ValidationAttribute
    {
        private readonly int maxLength;

        public MaxLengthAttribute(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            return value.ToString().Length <= this.maxLength;
        }
    }
}