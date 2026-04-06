using System.ComponentModel.DataAnnotations;

namespace Utility.Attribute
{
    public class GenericStringLengthAttribute : StringLengthAttribute
    {
        private readonly int maximumLength;
        public GenericStringLengthAttribute(int maximumLength) : base(maximumLength)
        {
            this.maximumLength = maximumLength;
            ErrorMessage = "{0} field must not exceed maximum length of" + $"{maximumLength} characters";
        }
    }
}