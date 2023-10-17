namespace Base.Shared
{
    public class ApiErrorException : Exception
    {
        public ApiErrorException(string errorCode)
        : base(errorCode + " => " + BaseErrorCodes.ErrorMessages[errorCode])
        { }

        public ApiErrorException(string errorCode, string message)
        : base(errorCode + " => " + message)
        { }

    }
}
