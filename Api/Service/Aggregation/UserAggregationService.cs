using System.Net;
using Api.Interface.Aggregation;
using Api.Interface.Orchestration;
using Api.Model.Dto.Request;
using Api.Model.Entity;
using Common.Interface;
using Common.Model;
using Common.Model.CustomEnum;
using OneOf;

namespace Api.Service.Aggregation
{
    public class UserAggregationService(
        IUserOrchestrationService iUserOrchestrationService
    ) : IUserAggregationService, IScopedService
    {
        public async ValueTask<ResponseWrapper> AggregateRegisterUserAsync(PostRegisterUserRequestDto postRegisterUserRequest)
        {
            try
            {
                OneOf<User, Error> orchestrateRegisterUserResult = await iUserOrchestrationService.OrchestrateRegisterUserAsync(postRegisterUserRequest);

                return orchestrateRegisterUserResult.Match(
                    user => ResponseBuilder
                    .CreateSuccess(HttpStatusCode.Created, "User registered successfully.")
                    .Build(),
                    error => ResponseBuilder
                    .CreateFail(error)
                    .Build()
                );
            }
            catch (Exception)
            {
                return ResponseBuilder
                .CreateFail(new Error(ErrorCodeEnum.INTERNAL_ERROR, "An unexpected error occurred while registering the user."))
                .Build();
            }
        }
    }
}