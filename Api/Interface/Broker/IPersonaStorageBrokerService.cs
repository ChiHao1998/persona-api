using Api.Model.Entity;

namespace Api.Interface.Broker
{
    public interface IPersonaStorageBrokerService
    {
        ValueTask<User> InsertUserAsync(User user);
        IQueryable<User> SelectUser(bool asNoTracking);
    }
}