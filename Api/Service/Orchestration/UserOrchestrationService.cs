using Api.Context.PersonaBackend;
using Api.Interface.Orchestration;
using Api.Interface.Processing;
using Api.Model.Dto.Request;
using Api.Model.Entity;
using Common.Interface;
using Common.Model;
using DatabaseBroker.Interface;
using OneOf;

namespace Api.Service.Orchestration
{
    public class UserOrchestrationService(
        IUserProcessingService iUserProcessingService,
        IUnitOfWorkBrokerService<PersonaContext> iUnitOfWorkBrokerService
    ) : IUserOrchestrationService, IScopedService
    {
        public async ValueTask<OneOf<User, Error>> OrchestrateRegisterUserAsync(PostRegisterUserRequestDto postRegisterUserRequest)
        {
            if (!(await iUserProcessingService.ProcessAddUserAsync(postRegisterUserRequest.Email, postRegisterUserRequest.Password))
            .TryPickT0(out User user, out Error error))
                return error;

            await iUnitOfWorkBrokerService.SaveChangesAsync();

            return user;
        }
    }
}