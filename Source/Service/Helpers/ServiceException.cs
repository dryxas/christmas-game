namespace Christmas2016.Service.Helpers
{
    using System.Diagnostics;

    [DebuggerStepThrough]
    public sealed class ServiceException : System.Exception
    {
        public ServiceException(string message)
            : base(message)
        {
        }

        public ServiceException(System.Exception innerException)
            : this(null, innerException)
        {
        }

        public ServiceException(string message, System.Exception innerException)
            : base(message, innerException)
        {
        }
    }
}