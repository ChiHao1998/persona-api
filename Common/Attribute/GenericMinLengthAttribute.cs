using System.ComponentModel.DataAnnotations;

namespace Utility.Attribute
{
    public class GenericMinLengthAttribute : MinLengthAttribute
    {
        private readonly int minimumLength;
        public GenericMinLengthAttribute(int minimumLength) : base(minimumLength)
        {
            this.minimumLength = minimumLength;
            ErrorMessage = "{0} field must at least have length of " + $"{minimumLength} characters";
        }
    }
}