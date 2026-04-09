using Api.Model.Dto.Request;
using Common.Model;

namespace Api.Interface.Aggregation
{
    public interface IUserAggregationService
    {
        ValueTask<ResponseWrapper> AggregateRegisterUserAsync(PostRegisterUserRequestDto postRegisterUserRequest);
    }
}