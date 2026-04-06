using System.ComponentModel.DataAnnotations;

namespace Utility.Attribute
{
    public class NoSpaceAllowedAttribute : RegularExpressionAttribute
    {
        public NoSpaceAllowedAttribute() : base(@"[^\s]+")
        {
            ErrorMessage = "No Space Allowed for {0} attribute";
        }
    }
}