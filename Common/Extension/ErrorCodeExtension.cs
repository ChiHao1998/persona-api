using System.Net;
using Common.Model.CustomEnum;

namespace Common.Extension
{
    public static class ErrorCodeExtensions
    {
        public static HttpStatusCode GetStatusCode(this ErrorCodeEnum errorCode) => errorCode switch
        {
            ErrorCodeEnum.ALREADY_EXISTED => HttpStatusCode.BadRequest,
            ErrorCodeEnum.NOT_FOUND => HttpStatusCode.NotFound,
            ErrorCodeEnum.INTERNAL_ERROR => HttpStatusCode.InternalServerError,
            _ => HttpStatusCode.InternalServerError
        };
    }
}