using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESU.ActivationWS.Core
{
    public class MsException : Exception
    {
        public MsException(string message)
            : base(message)
        {
        }

        public MsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
