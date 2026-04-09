using Api.Model.Entity;

namespace Api.Interface.Foundation
{
    public interface IUserFoundationService
    {
        ValueTask<User> AddUserAsync(User user);
        IQueryable<User> RetrieveUserAsync(bool asNoTracking = true);
    }
}