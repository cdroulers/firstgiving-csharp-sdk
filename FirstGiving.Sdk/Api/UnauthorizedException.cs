using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FirstGiving.Sdk.Api
{
    public class UnauthorizedException : FirstGivingException
    {
        public UnauthorizedException(string key, string secret, Exception inner, XDocument responseContent)
            : base(string.Format("You may not access the API with an application ID \"{0}\" and the secret \"{1}\".", secret, key), inner, responseContent)
        {
        }
    }
}
