namespace Common.Extension
{
    public static class ExceptionExtension
    {
        public static string FindRootExceptionMessage(this Exception ex)
        {
            return (ex.InnerException != null) ? FindRootExceptionMessage(ex.InnerException) : ex.Message;
        }
    }
}