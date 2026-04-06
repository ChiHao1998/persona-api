using Common.Model.CustomEnum;

namespace Common.Model
{
    public sealed class Error(ErrorCodeEnum errorCode, string message)
    {
        public ErrorCodeEnum Code { get; } = errorCode;
        public string Message { get; } = message;
    }
}
