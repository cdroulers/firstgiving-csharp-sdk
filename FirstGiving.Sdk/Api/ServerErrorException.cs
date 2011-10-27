using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FirstGiving.Sdk.Api
{
    public class ServerErrorException : FirstGivingException
    {
        public ServerErrorException(Exception innerException, XDocument responseContent)
            : base(GetErrorMessage(responseContent), innerException, responseContent)
        {
        }

        private static string GetErrorMessage(XDocument responseContent)
        {
            var node = responseContent.Descendants("firstGivingResponse").First();
            return "Server error. Message was:" + Environment.NewLine + node.Attributes("verboseErrorMessage").First().Value;
        }
    }
}
