using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstGiving.Sdk.Api
{
    public class ServerErrorException : FirstGivingException
    {
        public ServerErrorException(string errorMessage, Exception innerException)
            : base(errorMessage, innerException)
        {
        }
    }
}
