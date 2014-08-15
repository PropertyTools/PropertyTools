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