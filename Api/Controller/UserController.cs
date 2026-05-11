using Api.Interface.Aggregation;
using Api.Model.Dto.Request;
using Common.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controller
{
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [EnableRateLimiting("Ip")]
    public class UserController(
        IUserAggregationService iUserAggregationService
    ) : ControllerBase
    {
        [HttpPost("register")]
        [SwaggerOperation(Summary = "To register a new user")]
        [ProducesResponseType(typeof(ResponseWrapper), StatusCodes.Status201Created)]
        public async ValueTask<IActionResult> PostRegisterUserAsync(PostRegisterUserRequestDto postRegisterUserRequestDto)
        {
            ResponseWrapper response = await iUserAggregationService.AggregateRegisterUserAsync(postRegisterUserRequestDto);

            return StatusCode((int)response.StatusCode, response);
        }
    }
}