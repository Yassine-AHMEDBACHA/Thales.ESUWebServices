using System;

namespace ESU.ActivationWS.Core
{
    public class WebRequestException : Exception
    {
        public WebRequestException(string message)
            : base(message)
        {
        }

        public WebRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
