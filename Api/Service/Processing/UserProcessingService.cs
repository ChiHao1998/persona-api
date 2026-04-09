using Api.Interface.Foundation;
using Api.Interface.Processing;
using Api.Model.Entity;
using BCryptPasswordHasher.Interface;
using Common.Interface;
using Common.Model;
using Common.Model.CustomEnum;
using OneOf;

namespace Api.Service.Processing
{
    public class UserProcessingService(
        IBCryptPasswordHasherBrokerService iBCryptPasswordHasherBrokerService,
        IUserFoundationService iUserFoundationService
    ) : IUserProcessingService, IScopedService
    {
        public async ValueTask<OneOf<User, Error>> ProcessAddUserAsync(string email, string password)
        {
            if (iUserFoundationService.RetrieveUserAsync().Any(user => user.Email == email))
                return new Error(ErrorCodeEnum.ALREADY_EXISTED, $"User with email {email} already exists.");

            User user = UserBuilder.Create()
            .WithEmail(email)
            .WithPasswordHash(iBCryptPasswordHasherBrokerService.Hash(password))
            .Build();

            await iUserFoundationService.AddUserAsync(user);

            return user;
        }
    }
}