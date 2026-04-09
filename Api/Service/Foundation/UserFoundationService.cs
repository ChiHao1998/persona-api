using Api.Interface.Broker;
using Api.Interface.Foundation;
using Api.Model.Entity;
using Common.Interface;

namespace Api.Service.Foundation
{
    public class UserFoundationService(
        IPersonaStorageBrokerService iPersonaStorageBrokerService
    ) : IUserFoundationService, IScopedService
    {
        public async ValueTask<User> AddUserAsync(User user) => await iPersonaStorageBrokerService.InsertUserAsync(user);

        public IQueryable<User> RetrieveUserAsync(bool asNoTracking = true)
        => iPersonaStorageBrokerService.SelectUser(asNoTracking);
    }
}