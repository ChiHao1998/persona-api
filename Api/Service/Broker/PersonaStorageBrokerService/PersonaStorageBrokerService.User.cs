using Api.Context.PersonaBackend;
using Api.Interface.Broker;
using Api.Model.Entity;
using Common.Interface;
using Configuration;

namespace Api.Service.Broker.PersonaStorageBrokerService
{
    public partial class PersonaStorageBrokerService(
        PersonaContext personaContext
    ) : DatabaseBroker<PersonaContext>(personaContext), IPersonaStorageBrokerService, IScopedService
    {
        public async ValueTask<User> InsertUserAsync(User user) => await InsertAsync(user);
        public IQueryable<User> SelectUser(bool asNoTracking) => Select<User>(asNoTracking);
    }
}