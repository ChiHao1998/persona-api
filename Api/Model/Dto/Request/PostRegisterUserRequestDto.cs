using System.ComponentModel.DataAnnotations;
using Utility.Attribute;

namespace Api.Model.Dto.Request
{
    public class PostRegisterUserRequestDto
    {
        [GenericRequired]
        [EmailAddress]
        [GenericStringLength(254)]
        public string Email { get; set; } = string.Empty;

        [GenericRequired]
        [GenericMinLength(8)]
        [GenericStringLength(20)]
        [StrongPassword]
        public string Password { get; set; } = string.Empty;
    }
}