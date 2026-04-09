using Api.Model.Entity;
using Common.Model;
using OneOf;

namespace Api.Interface.Processing
{
    public interface IUserProcessingService
    {
        ValueTask<OneOf<User, Error>> ProcessAddUserAsync(string email, string password);
    }
}