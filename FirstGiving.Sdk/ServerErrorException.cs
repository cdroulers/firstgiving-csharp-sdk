using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FirstGiving.Sdk
{
    public class ServerErrorException : FirstGivingException
    {
        public ServerErrorException(string errorMessage, Exception innerException, XDocument responseContent)
            : base(errorMessage, innerException, responseContent)
        {
        }
    }
}
