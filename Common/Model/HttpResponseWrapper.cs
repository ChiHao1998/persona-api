using System.Net;
using System.Text.Json.Serialization;
using Common.Extension;
using Common.Model.CustomEnum;

namespace Common.Model
{
    public abstract class HttpResponseWrapperBase(HttpStatusCode statusCode, string message, string? errorCode)
    {
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; } = statusCode;
        public string Message { get; } = message;
        public string? ErrorCode { get; } = errorCode;
    }


    public sealed class ResponseWrapper : HttpResponseWrapperBase
    {
        internal ResponseWrapper(HttpStatusCode statusCode, string message, string? errorCode)
            : base(statusCode, message, errorCode)
        {
        }
    }

    public sealed class ResponseWrapper<T> : HttpResponseWrapperBase where T : class
    {
        public T Data { get; }

        internal ResponseWrapper(
            HttpStatusCode statusCode,
            string message,
            string? errorCode,
            T data)
            : base(statusCode, message, errorCode)
        {
            Data = data;
        }
    }

    public sealed class ResponseBuilder
    {
        private HttpStatusCode StatusCode;
        private string Message = string.Empty;
        private string? ErrorCode;

        private ResponseBuilder() { }

        public static ResponseBuilder CreateSuccess(HttpStatusCode statusCode, string message)
        {
            var builder = new ResponseBuilder();
            builder.WithStatus(statusCode);
            builder.WithMessage(message);
            return builder;
        }

        public static ResponseBuilder CreateFail(Error error)
        {
            ResponseBuilder builder = new();

            HttpStatusCode status = Enum.IsDefined(error.Code)
                ? error.Code.GetStatusCode()
                : HttpStatusCode.InternalServerError;

            builder.WithStatus(status);
            builder.WithMessage(error.Message);
            builder.WithErrorCode(error.Code);

            return builder;
        }

        private ResponseBuilder WithStatus(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            return this;
        }

        private ResponseBuilder WithMessage(string message)
        {
            Message = message;
            return this;
        }

        private ResponseBuilder WithErrorCode(ErrorCodeEnum errorCode)
        {
            ErrorCode = errorCode.ToString();
            return this;
        }

        public ResponseWrapper Build() => new(StatusCode, Message, ErrorCode);
    }

    public sealed class ResponseBuilder<T> where T : class
    {
        private HttpStatusCode StatusCode;
        private string Message = string.Empty;
        private string? ErrorCode;
        private T? Data;

        private ResponseBuilder() { }

        public static ResponseBuilder<T> CreateSuccess(HttpStatusCode statusCode, string message)
        {
            ResponseBuilder<T> builder = new();
            builder.WithStatus(statusCode);
            builder.WithMessage(message);
            return builder;
        }

        public static ResponseBuilder<T> CreateFail(Error error)
        {
            ResponseBuilder<T> builder = new();

            HttpStatusCode status = Enum.IsDefined(error.Code) ?
            error.Code.GetStatusCode() :
            HttpStatusCode.InternalServerError;

            builder.WithStatus(status);
            builder.WithMessage(error.Message);
            builder.WithErrorCode(error.Code);

            return builder;
        }

        private ResponseBuilder<T> WithStatus(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
            return this;
        }

        private ResponseBuilder<T> WithMessage(string message)
        {
            Message = message;
            return this;
        }

        private ResponseBuilder<T> WithErrorCode(ErrorCodeEnum errorCode)
        {
            ErrorCode = nameof(errorCode);
            return this;
        }

        public ResponseBuilder<T> WithData(T data)
        {
            Data = data;
            return this;
        }

        public ResponseWrapper<T> Build()
        {
            if (Data is null)
                throw new InvalidOperationException("Data must be provided for generic response.");

            return new(StatusCode, Message, ErrorCode, Data);
        }
    }

}