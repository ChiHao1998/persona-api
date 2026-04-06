using System.ComponentModel.DataAnnotations;

namespace Utility.Attribute
{
    public class GenericRequiredAttribute : RequiredAttribute
    {
        public GenericRequiredAttribute()
        {
            ErrorMessage = "{0} is required";
        }
    }
}