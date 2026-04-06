using System.ComponentModel.DataAnnotations;

namespace Utility.Attribute
{
    public class StrongPasswordAttribute : RegularExpressionAttribute
    {
        public StrongPasswordAttribute() : base(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).*$")
        {
            ErrorMessage = "Invalid password format. It must include uppercase letter, lowercase letter, digit, and special character.";
        }
    }
}