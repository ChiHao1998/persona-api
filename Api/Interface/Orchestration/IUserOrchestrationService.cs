using Api.Model.Dto.Request;
using Api.Model.Entity;
using Common.Model;
using OneOf;

namespace Api.Interface.Orchestration
{
    public interface IUserOrchestrationService
    {
        ValueTask<OneOf<User, Error>> OrchestrateRegisterUserAsync(PostRegisterUserRequestDto postRegisterUserRequest);
    }
}