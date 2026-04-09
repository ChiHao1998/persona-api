using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Api.Model.Entity
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        private User() { }

        internal User(string email, string passwordHash)
        {
            Email = email;
            PasswordHash = passwordHash;
            CreatedOn = DateTimeOffset.UtcNow;
        }

        [Key]
        public Guid Id { get; set; } = UUID.New();

        [Required]
        [StringLength(254)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
    }

    public interface IUserEmailStage
    {
        IUserPasswordStage WithEmail(string email);
    }

    public interface IUserPasswordStage
    {
        IUserBuildStage WithPasswordHash(string passwordHash);
    }

    public interface IUserBuildStage
    {
        User Build();
    }

    public class UserBuilder :
        IUserEmailStage,
        IUserPasswordStage,
        IUserBuildStage
    {
        private string? Email;
        private string? PasswordHash;

        private UserBuilder() { }

        public static IUserEmailStage Create() => new UserBuilder();

        public IUserPasswordStage WithEmail(string email)
        {
            Email = email;
            return this;
        }

        public IUserBuildStage WithPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
            return this;
        }

        public User Build() => new(Email!, PasswordHash!);
    }

}